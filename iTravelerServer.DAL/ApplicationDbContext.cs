using iTravelerServer.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace iTravelerServer.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Airport> Airport { get; set; }
        public DbSet<Flight> Flight { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<Plane> Plane { get; set; }
        public DbSet<Ticket>Ticket { get; set; }
        public DbSet<TicketDetail>TicketDetail { get; set; }
        public DbSet<Transfer> Transfer { get; set; }
        public DbSet<Account> Account { get; set; }
        
    }
}