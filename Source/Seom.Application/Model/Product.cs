using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Seom.Application.Model
{
    public class Product
    {
        public Product(string name, ProductCategory category, string? annotation = null)
        {
            Name = name;
            Category = category;
            Annotation = annotation;
        }
#pragma warning disable CS8618
        protected Product() { }
#pragma warning restore CS8618
        public int Id { get; private set; }
        public Guid Guid { get; set; }
        public string Name { get; set; }
        public string? Annotation { get; set; }
        public ProductCategory Category { get; set; }
        public string Type { get; } = default!; // Discriminator
        public List<Offer> Offers { get; } = new();
        public decimal Price { get; set; }
        public int Quantity { get; set; }

        public int BacketID { get; set; }
    }
}
