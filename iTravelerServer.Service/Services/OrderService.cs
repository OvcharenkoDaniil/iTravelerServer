using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iTravelerServer.DAL;
using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Enum;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Domain.ViewModels.OrderVM;
using iTravelerServer.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iTravelerServer.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseRepository<OrderDetails> _orderDetailsRepository;
        private readonly IBaseRepository<FlightDetails> _flightDetailsRepository;
        private readonly IBaseRepository<Account> _accountRepository;
        private readonly IBaseRepository<Order> _orderRepository;
        private readonly IFlightService _flightService;
        private readonly IMailService _mailService;


        // private readonly IBaseRepository<Flight> _flightRepository;
        private readonly ApplicationDbContext _db;

        public OrderService(IBaseRepository<Account> accountRepository,
            IBaseRepository<Order> orderRepository, IBaseRepository<OrderDetails> orderDetailsRepository,
            IBaseRepository<FlightDetails> flightDetailsRepository, ApplicationDbContext db,
            IBaseRepository<Flight> flightRepository, IFlightService flightService,IMailService mailService
            //,ITicketService ticketService
        )
        {
            _db = db;
            _mailService = mailService;
            _flightService = flightService;
            _orderDetailsRepository = orderDetailsRepository;
            _flightDetailsRepository = flightDetailsRepository;
            _accountRepository = accountRepository;
            _orderRepository = orderRepository;
        }

        public async Task<BaseResponse<List<Order>>> GetAllOrders()
        {
            var baseResponse = new BaseResponse<IEnumerable<Order>>();
            try
            {
                var orders = await _orderRepository.GetAll().ToListAsync();
                return new BaseResponse<List<Order>>()
                {
                    Data = orders,
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<Order>>()
                {
                    Description = $"[GetOrders] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public void CheckBaggageExtraPayment(OrderDetails orderDetail, int NumberOfTickets, int flight_id,
            int extraBaggageWeight)
        {
            var flightDetails = (from flight in _db.Flight
                join details in _db.FlightDetails on flight.FlightDetail_id equals details.FlightDetail_id
                select new
                {
                    flight.Flight_id,
                    details
                }).FirstOrDefault(x => x.Flight_id == flight_id);

            int permittedNumberOfBaggagePlaces = 0;
            if (orderDetail.FlightClass == "StandardClass")
                permittedNumberOfBaggagePlaces = flightDetails.details.StandardBaggagePlacesForPerson;
            if (orderDetail.FlightClass == "BusinessClass")
                permittedNumberOfBaggagePlaces = flightDetails.details.BusinessBaggagePlacesForPerson;

            int extraPayment = 0;
            if (orderDetail.NumberOfBaggagePlaces > permittedNumberOfBaggagePlaces * NumberOfTickets)
            {
                extraPayment =
                    (orderDetail.NumberOfBaggagePlaces - (permittedNumberOfBaggagePlaces * NumberOfTickets)) *
                    flightDetails.details.ExtraBaggagePrice;
            }

            if (extraBaggageWeight > 0)
            {
                extraPayment += extraBaggageWeight * flightDetails.details.BaggageSurchargePrice;
            }

            orderDetail.Price += extraPayment;
            //return orderDetail;
        }

        private void SplitNumbersString(string elem, List<int> orderdetailList)
        {
            string[] numbers = elem.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var seatNumber in numbers)
            {
                orderdetailList.Add(Int32.Parse(seatNumber));
            }
        }

        public List<int> GetFwOrderDetail(int FlightId,string flightClass)
        {
            var orderdetails = (from orders in _db.Order
                join details in _db.OrderDetails on orders.FwOrderDetail_id equals details.Order_Detail_id
                select new
                {
                    orders.FwFlight_id,
                    details
                }).Where(x => x.FwFlight_id == FlightId && x.details.FlightClass==flightClass).ToList();

            var orderdetailList = new List<int>();
            foreach (var elem in orderdetails)
            {
                // string[] numbers = elem.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                // foreach (var seatNumber in numbers)
                // {
                //     orderdetailList.Add(Int32.Parse(seatNumber));
                // }
                SplitNumbersString(elem.details.SeatNumbers, orderdetailList);
            }

            return orderdetailList;
        }


        public List<int> GetBwOrderDetail(int FlightId,string flightClass)
        {
            var orderdetails = (from orders in _db.Order
                join details in _db.OrderDetails on orders.BwOrderDetail_id equals details.Order_Detail_id
                select new
                {
                    orders.BwFlight_id,
                    details
                }).Where(x => x.BwFlight_id == FlightId && x.details.FlightClass==flightClass).ToList();

            var orderdetailList = new List<int>();
            foreach (var elem in orderdetails)
            {
                // string[] numbers = elem.SeatNumbers.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                // foreach (var seatNumber in numbers)
                // {
                //     orderdetailList.Add(Int32.Parse(seatNumber));
                // }
                SplitNumbersString(elem.details.SeatNumbers, orderdetailList);
            }

            return orderdetailList;
        }

        public BaseResponse<OrderDetails> AddOrderDetails(OrderDetails orderDetail, List<int> listOfFreePlaces)
        {
            // var baseResponse = new BaseResponse<Ticket>();
            try
            {
                bool val = false;
                bool val2 = false;
                //var freeFwPlaces = GetFreeFwPlaces(ticketVm.FwFlight_id, ticketVm.FlightClass);
                List<int> orderdetailList = new List<int>();
                SplitNumbersString(orderDetail.SeatNumbers, orderdetailList);
                //var freeBwPlaces = GetFreeBwPlaces(ticketVm.BwFlight_id, ticketVm.FlightClass);
                // for (int i=0;i<orderDetail.NumberOfTickets;i++)
                // {
                foreach (var elem in orderdetailList)
                {
                    if (!IsPlaceAvailable(listOfFreePlaces, elem))
                    {
                        return new BaseResponse<OrderDetails>()
                        {
                            Description = "OrderDetail: place is not free",

                            StatusCode = StatusCode.NotFound
                        };
                    }
                }

                _orderDetailsRepository.Create(orderDetail);

                if (orderDetail.Order_Detail_id != 0)
                {
                    return new BaseResponse<OrderDetails>()
                    {
                        Description = "Ticket created",
                        Data = orderDetail,
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<OrderDetails>()
                {
                    Description = "OrderDetail was not created",

                    StatusCode = StatusCode.InternalServerError
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<OrderDetails>()
                {
                    Description = $"[CreateOrderDetail] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Order>> AddOrder(OrderDetails fwOrderDetails, OrderDetails bwOrderDetails,
            OrderDetailsVM orderDetailsVM)
        {
            try
            {
                var acc = new Account()
                {
                    Email = orderDetailsVM.Email
                };
                var account = _accountRepository.Get(acc);
                if (account == null || account.Password == null)
                    return null;

                var fwFlightData = (from flight in _db.Flight
                    select flight).Where(f => f.Flight_id == orderDetailsVM.FwFlight_id).FirstOrDefault();

                if (fwFlightData.DepartureDate.Date > DateTime.Today.AddDays(1).Date)
                {
                    var expirationDate = new DateTime();

                    if (DateTime.Today.AddDays(8).Date <= fwFlightData.DepartureDate.Date)
                        expirationDate = DateTime.Today.AddDays(7).Date;
                    else expirationDate = fwFlightData.DepartureDate.AddDays(-1).Date;

                    var order = new Order()
                    {
                        FwFlight_id = orderDetailsVM.FwFlight_id,
                        BwFlight_id = orderDetailsVM.BwFlight_id,
                        FwOrderDetail_id = fwOrderDetails.Order_Detail_id,
                        BwOrderDetail_id = bwOrderDetails.Order_Detail_id,
                        Account_id = account.Account_id,
                        NumberOfTickets = orderDetailsVM.NumberOfTickets,
                        CreationDate = DateTime.Today,
                        ExpirationDate = expirationDate
                    };
                    if (orderDetailsVM.IsOneSide)
                    {
                        order.NumberOfFlights = 1;
                        order.TotalPrice = fwOrderDetails.Price;
                    }
                    else
                    {
                        order.NumberOfFlights = 2;
                        order.TotalPrice = fwOrderDetails.Price + bwOrderDetails.Price;
                    }

                    await _orderRepository.Create(order);

                    ChangeFlightDetailData(fwOrderDetails.FlightClass, order.FwFlight_id, order.NumberOfTickets,
                        fwOrderDetails.NumberOfBaggagePlaces);

                    if (order.FwFlight_id != order.BwFlight_id)
                    {
                        ChangeFlightDetailData(bwOrderDetails.FlightClass, order.BwFlight_id, order.NumberOfTickets,
                            bwOrderDetails.NumberOfBaggagePlaces);
                    }

                    if (order.Order_id != 0)
                    {
                        fwOrderDetails.Order_id = order.Order_id;
                        bwOrderDetails.Order_id = order.Order_id;
                        _orderDetailsRepository.UpdateSync(fwOrderDetails);
                        _orderDetailsRepository.UpdateSync(bwOrderDetails);
                        // _mailService.MessageSender(account.Email,"Билет успешно забронирован! Уникальный номер заказа: "+order.Order_id+" Крайний срок оплаты:"+order.ExpirationDate+"\n" +
                                                                 // "Для оплаты необходимо обратиться в агентство, рассположенное по адресу: ул.Независимости д.12");
                        return new BaseResponse<Order>()
                        {
                            Description = "Order created",
                            Data = order,
                            StatusCode = StatusCode.OK
                        };
                    }
                }

                return new BaseResponse<Order>()
                {
                    Description = "Order doesn't created",
                    StatusCode = StatusCode.InvalidDate
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Order>()
                {
                    Description = $"[AddOrder] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        private void ChangeFlightDetailData(string FlightClass, int flight_id, int numberOfTickets, int numberOfBaggage)
        {
            var flightDetails = (from flight in _db.Flight
                join details in _db.FlightDetails on flight.FlightDetail_id equals details.FlightDetail_id
                where flight.Flight_id == flight_id
                select details).FirstOrDefault();

            flightDetails.BaggageСompartmentsLeft -= numberOfBaggage;
            if (FlightClass == "BusinessClass" && flightDetails.BusinessClassSeatLeft > numberOfTickets)
            {
                flightDetails.BusinessClassSeatLeft -= numberOfTickets;
                _flightDetailsRepository.UpdateSync(flightDetails);
            }

            if (FlightClass == "StandardClass" && flightDetails.StandardSeatLeft > numberOfTickets)
            {
                flightDetails.StandardSeatLeft -= numberOfTickets;
                _flightDetailsRepository.UpdateSync(flightDetails);
            }
        }

        private bool IsPlaceAvailable(List<int> listOfFreePlaces, int place)
        {
            if (place != 0)
            {
                foreach (var item in listOfFreePlaces)
                {
                    if (item == place)
                    {
                        return true;
                    }
                }

                return false;
            }

            return true;
        }
        
        private void AddFlightDetailData(string FlightClass, int flight_id, int numberOfTickets, int numberOfBaggage)
        {
            var flightDetails = (from flight in _db.Flight
                join details in _db.FlightDetails on flight.FlightDetail_id equals details.FlightDetail_id
                where flight.Flight_id == flight_id
                select details).FirstOrDefault();

            flightDetails.BaggageСompartmentsLeft += numberOfBaggage;
            if (FlightClass == "BusinessClass" && flightDetails.BusinessClassSeatLeft > numberOfTickets)
            {
                flightDetails.BusinessClassSeatLeft += numberOfTickets;
                _flightDetailsRepository.UpdateSync(flightDetails);
            }

            if (FlightClass == "StandardClass" && flightDetails.StandardSeatLeft > numberOfTickets)
            {
                flightDetails.StandardSeatLeft += numberOfTickets;
                _flightDetailsRepository.UpdateSync(flightDetails);
            }
        }
        public async Task<BaseResponse<Order>> DeleteOrder(int orderId)
        {
            var baseResponse = new BaseResponse<Order>();
            try
            {
                var existedOrder = _orderRepository.Get(orderId);
                //var existedTicket = _orderRepository.Get(existedOrder.Ticket_id);
                
                var fwOrderDetail = _orderDetailsRepository.Get(existedOrder.FwOrderDetail_id);
                var bwOrderDetail = _orderDetailsRepository.Get(existedOrder.BwOrderDetail_id);
                
                AddFlightDetailData(fwOrderDetail.FlightClass, existedOrder.FwFlight_id, existedOrder.NumberOfTickets,
                    fwOrderDetail.NumberOfBaggagePlaces);
                if (existedOrder.FwFlight_id != existedOrder.BwFlight_id)
                {
                    AddFlightDetailData(bwOrderDetail.FlightClass, existedOrder.BwFlight_id, existedOrder.NumberOfTickets,
                        bwOrderDetail.NumberOfBaggagePlaces);
                }
                
                
                _orderRepository.Delete(existedOrder);
                existedOrder = _orderRepository.Get(orderId);
                if (existedOrder == null)
                {
                    _orderDetailsRepository.Delete(fwOrderDetail);
                    _orderDetailsRepository.Delete(bwOrderDetail);
                    
                    return new BaseResponse<Order>()
                    {
                        Description = "Order deleted",
                        StatusCode = StatusCode.OK
                    };
                }
        
                return new BaseResponse<Order>()
                {
                    Description = "Order was not deleted",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Order>()
                {
                    Description = $"[DeleteOrder] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        // public BaseResponse<List<TicketListVM>> ChangePrices(BaseResponse<List<TicketListVM>> listOfOrders,
        //     string email)
        // {
        //     try
        //     {
        //         var acc = new Account()
        //         {
        //             Email = email
        //         };
        //         var account = _accountRepository.Get(acc);
        //         if (account == null || account.Password == null)
        //             return null;
        //
        //         var listOfPrices = (from order in _db.Order
        //             join ticket in _db.Ticket on order.Ticket_id equals ticket.Ticket_id
        //             join flight in _db.Flight on ticket.FwFlight_id equals flight.Flight_id
        //             select new
        //             {
        //                 order.User_id,
        //                 ticket.Price
        //             }).Where(f => f.User_id == account.Account_id).ToList();
        //
        //         List<TicketListVM> listOfTickets = listOfOrders.Data;
        //
        //         if (listOfTickets != null && listOfPrices != null)
        //         {
        //             for (int i = 0; i < listOfTickets.Count; i++)
        //             {
        //                 listOfTickets[i].TotalPrice = listOfPrices[i].Price;
        //             }
        //         }
        //
        //         return new BaseResponse<List<TicketListVM>>()
        //         {
        //             Data = listOfTickets,
        //             StatusCode = StatusCode.OK
        //         };
        //     }
        //     catch (Exception ex)
        //     {
        //         return new BaseResponse<List<TicketListVM>>()
        //         {
        //             Description = $"[ChangePrices] : {ex.Message}",
        //             StatusCode = StatusCode.InternalServerError
        //         };
        //     }
        // }
        public bool IsAvailable(OrderDetailsVM orderDetailsVM)
        {
            var acc = new Account()
            {
                Email = orderDetailsVM.Email
            };
            var account = _accountRepository.Get(acc);
            if (account == null || account.Password == null)
                return false;
            

            var userOrders = (from orders in _db.Order
                select orders).Where(d => d.Account_id == account.Account_id).ToList();

            foreach (var order in userOrders)
            {
                if (order.FwFlight_id==orderDetailsVM.FwFlight_id)
                {
                    return false;
                }
                if (!orderDetailsVM.IsOneSide && order.BwFlight_id==orderDetailsVM.BwFlight_id)
                {
                    return false;
                }
            }
            return true;
        }
        public BaseResponse<List<TicketListVM>> GetOrdersList(List<FlightListVM> flightList, string email)
        {
            try
            {
                var acc = new Account()
                {
                    Email = email
                };
                var account = _accountRepository.Get(acc);
                if (account == null || account.Password == null)
                    return null;

                //var fwOrderList = new List<FlightListVM>();
                var orderList = new List<TicketListVM>();

                //var numberOfTickets = new List<int>();
                //var orderIdList = new List<int>();
                //var bwOrderList = new List<FlightListVM>();
                //var ticketList = new List<TicketListVM>();
                var userOrders = (from orders in _db.Order
                    select orders).Where(d => d.Account_id == account.Account_id).ToList();
                
                if (userOrders.Count != 0)
                {
                    int id = 0;
                    foreach (var order in userOrders)
                    {
                        var userFwFlight = (from orders in _db.Order
                            join flights in _db.Flight on orders.FwFlight_id equals flights.Flight_id
                            join orderDetails in _db.OrderDetails on orders.FwOrderDetail_id equals orderDetails.Order_Detail_id
                            select new
                            {
                                orders,
                                orderDetails,
                                flights
                            }).Where(d => d.orders.Order_id == order.Order_id).FirstOrDefault();
                        
                        var userBwFlight = (from orders in _db.Order
                            join flights in _db.Flight on orders.BwFlight_id equals flights.Flight_id
                            join orderDetails in _db.OrderDetails on orders.BwOrderDetail_id equals orderDetails.Order_Detail_id
                            select new
                            {
                                orders,
                                orderDetails,
                                flights
                            }).Where(d => d.orders.Order_id == order.Order_id).FirstOrDefault();
                        
                        var ticket = new TicketListVM();
                        foreach (var flightElem in flightList)
                        {
                            if (flightElem.Flight_id == order.FwFlight_id)
                            {
                                ticket.TicketElem_id = id;
                                
                                ticket.order_id = order.Order_id;
                                ticket.FlightClass = userFwFlight.orderDetails.FlightClass;
                                ticket.isOneSide = true;
                                _flightService.IsPriceSet(flightElem, userFwFlight.orderDetails.FlightClass, order.NumberOfTickets);
                                _flightService.fillFwTicket(flightElem, ticket,userFwFlight.orders.NumberOfTickets);
                                ticket.BwFlight = ticket.FwFlight;
                                ticket.FwFlight.SeatNumbers = userFwFlight.orderDetails.SeatNumbers;
                                ticket.FwFlight.TotalExtraBaggageWeight = userFwFlight.orderDetails.TotalExtraBaggageWeight;
                                ticket.FwFlight.NumberOfBaggagePlaces = userFwFlight.orderDetails.NumberOfBaggagePlaces;
                                ticket.TotalPrice += ticket.FwFlight.Price;
                                id++;

                            }

                            if (order.NumberOfFlights == 2)
                            {
                                if (flightElem.Flight_id == order.BwFlight_id)
                                {
                                                                  //ticket.TotalPrice += userBwFlight.orderDetails.Price;
                                    ticket.FlightClass = userBwFlight.orderDetails.FlightClass;
                                    ticket.isOneSide = false;
                                    _flightService.IsPriceSet(flightElem, userBwFlight.orderDetails.FlightClass, order.NumberOfTickets);
                                    ticket = _flightService.fillBwTicket(flightElem, ticket);
                                    ticket.BwFlight.SeatNumbers = userBwFlight.orderDetails.SeatNumbers;
                                    ticket.BwFlight.TotalExtraBaggageWeight = userBwFlight.orderDetails.TotalExtraBaggageWeight;
                                    ticket.BwFlight.NumberOfBaggagePlaces = userBwFlight.orderDetails.NumberOfBaggagePlaces;      
                                    ticket.TotalPrice += ticket.BwFlight.Price;
                                }
                            }
                        }

                        if (ticket.FwFlight!=null)
                        {
                            orderList.Add(ticket);   
                        }
                    }
                }

                if (orderList.Count != 0)
                {

                    return new BaseResponse<List<TicketListVM>>()
                    {
                        Data = orderList,
                        StatusCode = StatusCode.OK
                    };
                }

                return new BaseResponse<List<TicketListVM>>()
                {
                    Description = "there is not such tickets",
                    StatusCode = StatusCode.NotFound
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<TicketListVM>>()
                {
                    Description = $"[GetTicketList] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }
}