using Saman.Backend.Share.shareClasses;

namespace Saman.Backend.Business.Entity.Category
{
    public class Category_dTo : baseDTo
    {
        public Category_dTo() { }
        public Category_dTo(Category_dBo? dbo)
        : base(dbo)
        {
            if (dbo != null)
            {
                this.Id = dbo.Id;
                this.Name = dbo.Name;
                this.PathByName = dbo.PathByName;
                this.PathById = dbo.PathById;
                this.Level = dbo.Level;
            }
        }

        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string? PathByName { get; set; } 

        public string? PathById { get; set; } 

        public int Level { get; set; }
    }
}
