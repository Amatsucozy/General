using System.Text.Json;
using Florence.Domain.EventTemplate.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Florence.EF.EntityConfig;

public sealed class EventTemplateConfig : IEntityTypeConfiguration<EventTemplate>
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new();

    private static readonly ValueComparer ParametersValueComparer = new ValueComparer<string[]>(
        (first, second) => (first ?? Array.Empty<string>()).SequenceEqual(second ?? Array.Empty<string>()),
        array => array.GetHashCode(),
        array => array.Select(arrayElement => arrayElement).ToArray()
    );

    public void Configure(EntityTypeBuilder<EventTemplate> builder)
    {
        builder.ToTable(nameof(EventTemplate));

        builder.HasKey(
            nameof(EventTemplate.Id),
            nameof(EventTemplate.TenantId)
        );

        builder.Property(et => et.Parameters)
            .HasConversion(
                parameters => JsonSerializer.Serialize(parameters, JsonSerializerOptions),
                serialized => JsonSerializer.Deserialize<string[]>(serialized, JsonSerializerOptions) ??
                              Array.Empty<string>(),
                ParametersValueComparer
            );
    }
}