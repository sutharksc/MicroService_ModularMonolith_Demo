using SharedModule.Shared;

namespace OrderModule.Domain.Models
{
    public class Order : BaseModel
    {
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotal { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public string OrderStatus { get; set; } = "Processing";

        public ICollection<OrderEvents> Events { get; set; }
    }
}
