using Florence.Domain.EventTemplate.Enums;

namespace Florence.Domain.EventTemplate.Models;

public sealed class EventTemplate
{
    public EventTemplateId Id { get; private set; }
    
    public Guid TenantId { get; private set; }
    
    public string Description { get; private set; }
    
    public string[] Parameters { get; private set; }
    
    public EventTemplate()
    {
        Parameters = Array.Empty<string>();
    }
    
    public EventTemplate(EventTemplateId id, Guid tenantId)
    {
        Id = id;
        TenantId = tenantId;
        Description = string.Empty;
        Parameters = Array.Empty<string>();
    }
}