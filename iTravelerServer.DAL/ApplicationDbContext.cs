using iTravelerServer.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace iTravelerServer.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            //Database.EnsureCreated();
        }
        public DbSet<Airport> Airport { get; set; }
        public DbSet<Flight> Flight { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Plane> Plane { get; set; }
        public DbSet<Baggage> Baggage { get; set; }
        public DbSet<OrderDetails>OrderDetails { get; set; }
        public DbSet<FlightDetails>FlightDetails { get; set; }
        public DbSet<Transfer> Transfer { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Place> Place { get; set; }
        public DbSet<Facility> Facility { get; set; }
        
    }
}