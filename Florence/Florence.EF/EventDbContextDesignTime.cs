using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Florence.EF;

public sealed class EventDbContextDesignTime : IDesignTimeDbContextFactory<EventDbContext>
{
    public EventDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder()
            .UseSqlServer(Constants.ConnectionString);
        
        return new EventDbContext(optionsBuilder.Options);
    }
}