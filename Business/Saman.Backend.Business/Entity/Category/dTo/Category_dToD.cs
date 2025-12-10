namespace Saman.Backend.Business.Entity.Category
{
    public class Category_dToD : Category_dTo
    {
        public Category_dToD() { }
        public Category_dToD(Category_dBo? dbo) : base(dbo)
        {
            if (dbo != null)
            {
                this.Parent = new Category_dTo(dbo.Parent);
            }
        }

        public Category_dTo? Parent { get; set; }
    }
}
