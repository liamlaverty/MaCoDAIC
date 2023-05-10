namespace Macodaic.App.Core.Models
{
    public class VendorOffer
    {
        public VendorOffer(Guid vendorId, int numberOrangesOffered, decimal pricePerOrange)
        {
            VendorId = vendorId;
            NumberOrangesOffered = numberOrangesOffered;
            PricePerOrange = pricePerOrange;
        }

        public Guid VendorId { get; }
        public int NumberOrangesOffered { get; }
        public decimal PricePerOrange { get; }
    }
}
