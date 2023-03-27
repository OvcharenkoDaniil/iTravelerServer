
using Automarket.DAL.Repositories;

using iTravelerServer.DAL.Interfaces;
using iTravelerServer.DAL.Repositories;
using iTravelerServer.Domain.Entities;
using iTravelerServer.Domain.ViewModels.FlightVM;
using iTravelerServer.Service.Interfaces;
using iTravelerServer.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Automarket
{
    public static class Initializer
    {
        public static void InitializeRepositories(this IServiceCollection services)
        {
             services.AddScoped<IBaseRepository<Flight>, FlightRepository>();
             services.AddScoped<IBaseRepository<Account>, AccountRepository>();
             services.AddScoped<IBaseRepository<Plane>, PlaneRepository>();
             services.AddScoped<IBaseRepository<Airport>, AirportRepository>();
             services.AddScoped<IBaseRepository<Order>, OrderRepository>();
             services.AddScoped<IBaseRepository<Ticket>, TicketRepository>();
             services.AddScoped<IBaseRepository<TicketDetail>, TicketDetailRepository>();
             services.AddScoped<IBaseRepository<Transfer>, TransferRepository>();
             //services.AddScoped<IBaseRepository<TicketListVM>, TicketListRepository>();
             
        }

        public static void InitializeServices(this IServiceCollection services)
        {
            services.AddScoped<IFlightService, FlightService>();
            services.AddScoped<ITransferService, TransferService>();
            services.AddScoped<IAirportService, AirportService>();
            services.AddScoped<IPlaneService, PlaneService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IAccountService, AccountService>();
            
            //services.AddTransient<IAccountService, AccountService>();
        }
    }
}