using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saman.Backend.Business.Entity.Product;
using Saman.Backend.Share.shareClasses;
using Saman.Backend.Share.shareValidators.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Saman.Backend.Business.Entity.Category
{
    [Table("Category", Schema = "cor")]
    public class Category_dBo : baseDBo
    {
        public Category_dBo() { }
        public Category_dBo(
            string name,
            int level,
            int? parent_Id = null)
        {
            this.Name = name;
            this.Level = level;
            this.Parent_Id = parent_Id;
        }

        [Key]
        public int Id { get; set; }

        [Mandatory]
        [MaxLength(200)]
        public string Name { get; set; } = null!;

        [Mandatory]
        [MaxLength(1000)]
        public string PathByName { get; set; } = null!;

        [Mandatory]
        [MaxLength(1000)]
        public string PathById { get; set; } = null!;

        [Range(1, 4)]
        public int Level { get; set; }

        public int? Parent_Id { get; set; }
        [ForeignKey("Parent_Id")]
        public virtual Category_dBo? Parent { get; set; }

        public virtual ICollection<Category_dBo> Children { get; set; } = null!;
       
        public virtual ICollection<Product_dBo> Products { get; set; } = null!;

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public bool LevelAllowedForProduct
            => this.Level == 4;
    }

    public class CategoryConfiguration : IEntityTypeConfiguration<Category_dBo>
    {
        public void Configure(EntityTypeBuilder<Category_dBo> builder)
        {
            builder.HasIndex(u => u.Name);
            builder.HasIndex(u => u.PathByName);
            builder.HasIndex(u => u.PathById);
        }
    }
}
