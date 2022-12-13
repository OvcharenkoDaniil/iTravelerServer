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
        private readonly IBaseRepository<Order> _orderRepository;
        private readonly IBaseRepository<Account> _accountRepository;

        private readonly IBaseRepository<Plane> _planeRepository;

        //private readonly ITicketService _ticketService;
        private readonly IBaseRepository<Flight> _flightRepository;
        private readonly ApplicationDbContext _db;

        public OrderService(IBaseRepository<Order> orderRepository, IBaseRepository<Account> accountRepository,
            IBaseRepository<Ticket> ticketRepository, ApplicationDbContext db, IBaseRepository<Plane> planeRepository,
            IBaseRepository<Flight> flightRepository
            //,ITicketService ticketService
        )
        {
            _orderRepository = orderRepository;
            _accountRepository = accountRepository;
            _db = db;
            _planeRepository = planeRepository;
            _flightRepository = flightRepository;
            //_ticketService = ticketService;
        }

        public async Task<BaseResponse<IEnumerable<Order>>> GetOrders()
        {
            var baseResponse = new BaseResponse<IEnumerable<Order>>();
            try
            {
                var orders = await _orderRepository.GetAll().ToListAsync();


                baseResponse.Data = orders;
                return baseResponse;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Order>>()
                {
                    Description = $"[GetOrders] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }

        public async Task<BaseResponse<Order>> AddOrder(OrderVM orderVm)
        {
            var baseResponse = new BaseResponse<Order>();
            try
            {
                var acc = new Account()
                {
                    Email = orderVm.userEmail
                };
                var account = _accountRepository.Get(acc);
                if (account == null || account.Password == null)
                    return null;

                var order = new Order()
                {
                    FlightClass = orderVm.FlightClass,
                    Ticket_id = orderVm.ticket_id,
                    User_id = account.Account_id,
                    NumberOfTickets = orderVm.numberOfTickets,
                    CreationDate = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddDays(7)
                };


                // var fwPlane = (from ticket in _db.Ticket
                //     join flight in _db.Flight on ticket.FwFlight_id equals flight.Flight_id
                //     join plane in _db.Plane on flight.Plane_id equals plane.Plane_id
                //     select plane).FirstOrDefault<Plane>();
                // var bwPlane = (from ticket in _db.Ticket
                //     join flight in _db.Flight on ticket.BwFlight_id equals flight.Flight_id
                //     join plane in _db.Plane on flight.Plane_id equals plane.Plane_id
                //     select plane).FirstOrDefault<Plane>();
                var bwFlight = (from ticket in _db.Ticket
                    join flight in _db.Flight on ticket.BwFlight_id equals flight.Flight_id
                    select flight).FirstOrDefault<Flight>();
                var fwFlight = (from ticket in _db.Ticket
                    join flight in _db.Flight on ticket.FwFlight_id equals flight.Flight_id
                    select flight).FirstOrDefault<Flight>();

                if (order.FlightClass == "FirstClass")
                {
                    fwFlight.FirstClassTicketsLeft -= order.NumberOfTickets;
                    bwFlight.FirstClassTicketsLeft -= order.NumberOfTickets;
                    _flightRepository.UpdateSync(fwFlight);
                    _flightRepository.UpdateSync(bwFlight);
                }

                if (order.FlightClass == "StandardClass")
                {
                    fwFlight.FirstClassTicketsLeft -= order.NumberOfTickets;
                    bwFlight.FirstClassTicketsLeft -= order.NumberOfTickets;
                    _flightRepository.UpdateSync(fwFlight);
                    _flightRepository.UpdateSync(bwFlight);
                }

                await _orderRepository.Create(order);

                return new BaseResponse<Order>()
                {
                    Description = "Order created",
                    StatusCode = StatusCode.OK
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
        
        public async Task<BaseResponse<Order>> DeleteOrder(int orderId)
        {
            var baseResponse = new BaseResponse<Order>();
            try
            {
                
                var existedOrder = _orderRepository.Get(orderId);
                _orderRepository.Delete(existedOrder);
                existedOrder = _orderRepository.Get(orderId);
                if (existedOrder == null)
                {
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

        public BaseResponse<List<TicketListVM>> GetOrdersList(List<FlightListVM> flightList, string email,
            ITicketService _ticketService)
        {
            var acc = new Account()
            {
                Email = email
            };
            var account = _accountRepository.Get(acc);
            if (account == null || account.Password == null)
                return null;

            var fwTicketList = new List<FlightListVM>();
            var numberOfTickets = new List<int>();
            var orderIdList = new List<int>();
            var bwTicketList = new List<FlightListVM>();
            var ticketList = new List<TicketListVM>();
            try
            {
                var userFwFlight = (from orders in _db.Order
                    join tickets in _db.Ticket on orders.Ticket_id equals tickets.Ticket_id
                    join flights in _db.Flight on tickets.FwFlight_id equals flights.Flight_id
                    select new
                    {
                        flights.Flight_id,
                        orders.NumberOfTickets,
                        orders.User_id,
                        orders.Order_id
                    }).Where(f => f.User_id == account.Account_id).ToList();

                var userBwFlight = (from orders in _db.Order
                    join tickets in _db.Ticket on orders.Ticket_id equals tickets.Ticket_id
                    join flights in _db.Flight on tickets.BwFlight_id equals flights.Flight_id
                    select new
                    {
                        flights.Flight_id,
                        orders.NumberOfTickets,
                        orders.User_id,
                        orders.Order_id
                    }).Where(f => f.User_id == account.Account_id).ToList();


                for (int i = 0; i < userFwFlight.Count; i++)
                {
                    foreach (var item in flightList)
                    {
                        if (userFwFlight[i].Flight_id == item.Flight_id)
                        {
                            fwTicketList.Add(item);
                            numberOfTickets.Add(userFwFlight[i].NumberOfTickets);
                            orderIdList.Add(userFwFlight[i].Order_id);
                        }

                        if (userBwFlight[i].Flight_id == item.Flight_id)
                        {
                            bwTicketList.Add(item);
                        }
                    }
                }

                var a = 3;
                if (fwTicketList.Count != 0 && bwTicketList.Count != 0)
                {
                    for (int i = 0; i < fwTicketList.Count; i++)
                    {
                        var item = _ticketService.createTicket(fwTicketList[i], bwTicketList[i], numberOfTickets[i], i);
                        item.order_id = orderIdList[i];
                        ticketList.Add(item);
                        
                    }

                    

                    return new BaseResponse<List<TicketListVM>>()
                    {
                        Data = ticketList,
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