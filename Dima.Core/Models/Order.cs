using Dima.Core.Enums;

namespace Dima.Core.Models
{
    public class Order
    {
        public long Id { get; set; }
        public string Number { get; set; } = Guid.NewGuid().ToString("N")[..8];
        public string? ExternalReference { get; set; }
        public EPaymentGateway Gateway { get; set; } = EPaymentGateway.Stripe;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public EOrderStatus Status { get; set; } = EOrderStatus.WaitingPayment;
        public long ProductId { get; set; }
        public Product Product { get; set; } = null!;   
        public long? VouncherId { get; set; }
        public Vouncher? Vouncher { get; set; }
        public string UserId { get; set; } = string.Empty;
        public decimal Total => Product.Price - (Vouncher?.Amount ?? 0);   
    }
}
