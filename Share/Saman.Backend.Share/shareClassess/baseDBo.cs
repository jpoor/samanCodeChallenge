using System.ComponentModel.DataAnnotations;

namespace Saman.Backend.Share.shareClasses
{
    public abstract class baseDBo
    {       
        [Required]
        [StringLength(50)]
        public virtual string CreatorId { get; set; } = null!;

        [Required]
        public virtual DateTime CreationDate { get; set; } = DateTime.UtcNow;

        [StringLength(50)]
        public virtual string? UpdaterId { get; set; }

        public virtual DateTime? UpdateDate { get; set; }

        [Required]
        public virtual bool Deactive { get; set; }
    }
}
