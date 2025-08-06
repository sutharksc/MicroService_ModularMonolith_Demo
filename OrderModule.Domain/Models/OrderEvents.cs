using SharedModule.Shared;
using System.ComponentModel.DataAnnotations.Schema;

namespace OrderModule.Domain.Models
{
    public class OrderEvents : BaseModel
    {
        public Guid OrderId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string EventType { get; set; }
        public string Name { get; set; }
        public string Venue { get; set; }
        public decimal Amount { get; set; }

        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
    }
}
