using System.Collections.Generic;

namespace Seom.Application.Model
{
    public class Offer
    {
        public Offer(Store store, Product product, decimal price)
        {
            Store = store;
            Product = product;
            Price = price;
        }

#pragma warning disable CS8618
        protected Offer() { }
#pragma warning restore CS8618
        public int Id { get; private set; }
        public Store Store { get; set; }
        public Product Product { get; set; }
        public decimal Price { get; set; }
    }
}
