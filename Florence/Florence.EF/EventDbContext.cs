using Florence.Domain.EventTemplate.Models;
using Microsoft.EntityFrameworkCore;

namespace Florence.EF;

public sealed class EventDbContext : DbContext
{
    public EventDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("event");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventDbContext).Assembly);
    }
    
    public DbSet<EventTemplate> EventTemplates { get; set; }
}