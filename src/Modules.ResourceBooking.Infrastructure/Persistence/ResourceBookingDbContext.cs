using Microsoft.EntityFrameworkCore;
using Modules.ResourceBooking.Domain.Entities;
using Modules.ResourceBooking.Infrastructure.Persistence.Configurations;

namespace Modules.ResourceBooking.Infrastructure.Persistence;

public class ResourceBookingDbContext : DbContext
{
    public ResourceBookingDbContext(DbContextOptions<ResourceBookingDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Resource> Resources => Set<Resource>();
    public DbSet<Reservation> Reservations => Set<Reservation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ResourceBookingDbContext).Assembly);
    }
}
