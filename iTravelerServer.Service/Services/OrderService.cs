using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using iTravelerServer.DAL.Interfaces;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.Enum;
using iTravelerServer.Domain.Response;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace iTravelerServer.Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IBaseRepository<Order> _orderRepository;
        private readonly IBaseRepository<Account> _accountRepository;

        public OrderService(IBaseRepository<Order> orderRepository)
        {
            _orderRepository = orderRepository;
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
        
        public async Task<BaseResponse<Order>> AddOrders(TicketListVM ticket, Account account)
        {
            var baseResponse = new BaseResponse<Order>();
            try
            {
                var user = _accountRepository.Get(account);
                var order = new Order()
                {
                    Ticket_id = ticket.TicketElem_id,
                    User_id = user.Id,
                    NumberOfTickets = ticket.NumberOfPassengers,
                    CreationDate = DateTime.Now,
                    ExpirationDate = DateTime.Now.AddDays(7)

                };
                
                await _orderRepository.Create(order);
                
                return new BaseResponse<Order>()
                {
                    Description = "Ticket created",
                    StatusCode = StatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<Order>()
                {
                    Description = $"[AddOrders] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
        }
    }   
}