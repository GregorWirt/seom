namespace Seom.Application.Model
{
    public class OfferOrderItem
    {
        public OfferOrderItem(OrderItem orderItem, Offer offer, int? sweetness = null)
        {
            OrderItem = orderItem;
            Offer = offer;
            Sweetness = sweetness;
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected OfferOrderItem() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public int Id { get; set; }
        public OrderItem OrderItem {get;set;}
        public Offer Offer { get;set;}
        public int? Sweetness { get; set; }
    }
}
