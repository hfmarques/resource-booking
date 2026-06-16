using ResourceBooking.Domain.Enums;

namespace ResourceBooking.Domain.Entities;

public class Resource
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public ResourceType Type { get; set; }
    public bool IsAvailable { get; set; }
    public List<Reservation> Reservations = [];
}