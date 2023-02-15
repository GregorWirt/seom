namespace Seom.Application.Model
{
    public class ProductWithSize : Product
    {
        public ProductWithSize(Size size, string name, ProductCategory productCategory, string? annotation = null) : base(name, productCategory, annotation)
        {
            Size = size;
        }
        protected ProductWithSize() { }
        public Size Size { get; set; }
    }
}
