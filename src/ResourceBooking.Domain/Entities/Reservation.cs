using ResourceBooking.Domain.Enums;

namespace ResourceBooking.Domain.Entities;

public class Reservation
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ResourceId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public ReservationStatus Status { get; set; }
    public User? User { get; set; }
    public Resource? Resource { get; set; }
}