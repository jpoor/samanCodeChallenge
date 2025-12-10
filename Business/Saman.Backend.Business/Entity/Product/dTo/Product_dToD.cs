using Saman.Backend.Business.Entity.Category;

namespace Saman.Backend.Business.Entity.Product
{
    public class Product_dToD : Product_dTo
    {
        public Product_dToD() { }
        public Product_dToD(Product_dBo? dbo) : base(dbo)
        {
            if (dbo != null)
            {
                this.Description = dbo.Description;
                this.Category = new Category_dTo(dbo.Category);
            }
        }

        public string Description { get; set; } = null!;

        public Category_dTo Category { get; set; } = null!;
    }
}
