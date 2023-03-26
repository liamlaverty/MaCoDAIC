namespace Macodaic.App.Core.Models
{
    public class ConsumerPurchaseRequest
    {
        public ConsumerPurchaseRequest(Guid consumerId, Guid vendorId, int numberOfOrangesRequested,
            decimal expectedPrice)
        {
            ConsumerId = consumerId;
            VendorId = vendorId;
            NumberOfOrangesRequested = numberOfOrangesRequested;
            ExpectedPrice = expectedPrice;
        }

        public Guid ConsumerId { get; }
        public Guid VendorId { get; }
        public int NumberOfOrangesRequested { get; }
        public decimal ExpectedPrice { get; }
    }
}
