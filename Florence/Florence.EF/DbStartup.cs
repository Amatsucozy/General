using Florence.Domain.EventTemplate.Enums;
using Florence.Domain.EventTemplate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Florence.EF;

public static class DbStartup
{
    public static void Run(IServiceScopeFactory serviceScopeFactory)
    {
        using var serviceScope = serviceScopeFactory.CreateScope();

        MigrateDb(serviceScope);
        InsertDefaultData(serviceScope);
    }
    
    private static void MigrateDb(IServiceScope serviceScope)
    {
        var eventDbContext = serviceScope.ServiceProvider.GetRequiredService<EventDbContext>();

        if (!eventDbContext.Database.GetPendingMigrations().Any())
        {
            return;
        }
        
        eventDbContext.Database.Migrate();
    }
    
    private static void InsertDefaultData(IServiceScope serviceScope)
    {
        var eventDbContext = serviceScope.ServiceProvider.GetRequiredService<EventDbContext>();
        var tenantId = serviceScope.ServiceProvider.GetRequiredService<IConfiguration>().GetTenantId("Florence");

        var currentTemplateIds = eventDbContext.EventTemplates
            .Where(et => et.TenantId.Equals(tenantId))
            .Select(et => et.Id)
            .ToHashSet();

        var definedTemplateIds = Enum.GetValues<EventTemplateId>();

        if (definedTemplateIds.Length == currentTemplateIds.Count)
        {
            return;
        }

        var newTemplateIds = definedTemplateIds.Where(id => !currentTemplateIds.Contains(id));

        eventDbContext.EventTemplates.AddRange(GetDefaultEventTemplate(newTemplateIds, tenantId));
        eventDbContext.SaveChanges();
    }

    private static Guid GetTenantId(this IConfiguration configuration, string name)
    {
        return Guid.TryParse(configuration.GetSection("Tenant")[name], out var tenantId) ? tenantId : Guid.Empty;
    }

    private static IEnumerable<EventTemplate> GetDefaultEventTemplate(
        IEnumerable<EventTemplateId> definedTemplateIds,
        Guid tenantId
    )
    {
        return definedTemplateIds.Select(templateId => new EventTemplate(templateId, tenantId));
    }
}