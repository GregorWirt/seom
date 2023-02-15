using Microsoft.EntityFrameworkCore;

namespace Seom.Application.Model
{
    [Index(nameof(Name), IsUnique = true)]
    public class ProductCategory
    {
        public ProductCategory(string name, bool sweetnessAvailable, bool toppingCategory, string ? annotation = null)
        {
            Name = name;
            SweetnessAvailable = sweetnessAvailable;
            ToppingCategory = toppingCategory;
            Annotation = annotation;
        }

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        protected ProductCategory() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public int Id { get; set; }
        public string Name { get; set; }
        public bool SweetnessAvailable { get; set; }
        public bool ToppingCategory { get; set; }
        public string? Annotation { get; set; }
    }
}
