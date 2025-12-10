using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Saman.Backend.Business.Entity.Category;
using Saman.Backend.Share.shareClasses;
using Saman.Backend.Share.shareValidators.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saman.Backend.Business.Entity.Product
{
    [Table("Product", Schema = "cor")]
    public class Product_dBo : baseDBo
    {
        public Product_dBo() { }
        public Product_dBo(
            string sKU, 
            string name, 
            string description, 
            decimal price, 
            int sequence,
            int category_Id)
        {
            SKU = sKU;
            Name = name;
            Description = description;
            Price = price;
            Sequence = sequence;
            Category_Id = category_Id;
        }

        [Key]
        public int Id { get; set; }

        [Mandatory]
        [MaxLength(100)]
        public string SKU { get; set; } = null!;

        [Mandatory]
        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [Mandatory]
        public string Description { get; set; } = null!;

        [NotNegative]
        public decimal Price { get; set; }

        [NotNegative]
        public int OnHand { get; set; }
      
        public int Sequence { get; set; }

        [Mandatory]
        public int Category_Id { get; set; }
        [ForeignKey("Category_Id")]
        public virtual Category_dBo Category { get; set; } = null!;
    }

    public class ProductConfiguration : IEntityTypeConfiguration<Product_dBo>
    {
        public void Configure(EntityTypeBuilder<Product_dBo> builder)
        {
            builder.HasIndex(u => u.SKU).IsUnique();
            builder.HasIndex(u => u.Name);
            builder.Property(u => u.Price).HasPrecision(18, 2);
        }
    }
}
