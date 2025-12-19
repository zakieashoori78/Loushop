namespace Loushop.Dtos.RequestDto
{
    public class PaymentDto
    {
        public required string UserId { get; set; }
        public required decimal Amount { get; set; }
        public int OrderId { get; set; }
    }
}
