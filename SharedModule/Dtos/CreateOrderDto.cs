namespace SharedModule.Dtos
{
    public class OrderItemDto
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
    }

    public class CreateOrderRequestDto
    {
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public double TotalAmount { get; set; }
        public string PaymentStatus { get; set; } = "Pending";
        public string OrderStatus { get; set; } = "Processing";
        public List<OrderItemDto> Items { get; set; } = new();
    }
}
