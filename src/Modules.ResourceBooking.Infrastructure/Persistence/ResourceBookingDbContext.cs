using Microsoft.EntityFrameworkCore;
using Modules.ResourceBooking.Domain.Entities;

namespace Modules.ResourceBooking.Infrastructure.Persistence;

public class ResourceBookingDbContext(DbContextOptions<ResourceBookingDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResourceBookingDbContext).Assembly);
    }
}
