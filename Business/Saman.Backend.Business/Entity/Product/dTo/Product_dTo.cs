using Saman.Backend.Share.shareClasses;

namespace Saman.Backend.Business.Entity.Product
{
    public class Product_dTo : baseDTo
    {
        public Product_dTo() { }
        public Product_dTo(Product_dBo? dbo)
        : base(dbo)
        {
            if (dbo != null)
            {
                this.Id = dbo.Id;
                this.SKU = dbo.SKU;
                this.Name = dbo.Name;
                this.Price = dbo.Price;
                this.OnHand = dbo.OnHand;
            }
        }

        public int Id { get; set; }

        public string SKU { get; set; } = null!;

        public string Name { get; set; } = null!;

        public decimal Price { get; set; }

        public int OnHand { get; set; }
    }
}
