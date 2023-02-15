using System.Collections.Generic;

namespace Seom.Application.Model
{
    public class OrderItem
    {
        public OrderItem(Order order)
        {
            Order = order;
        }
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected OrderItem() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public int Id { get; set; }
        public Order Order { get; set; }
        public List<OfferOrderItem> Offers { get; } = new();
    }
}
