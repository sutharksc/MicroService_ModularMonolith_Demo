using SharedModule.Shared;

namespace BookingModule.Domain.Models
{
    public class Booking : BaseModel
    {
        public DateTime BookingDate { get; set; }
        public Guid UserId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerMobile { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
    }
}
