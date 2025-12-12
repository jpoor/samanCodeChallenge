using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saman.Backend.Business.Entity.Authentication;
using Saman.Backend.Share.shareClasses;
using Saman.Backend.Share.shareValidators.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace Saman.Backend.Business.Entity.Log
{
    [Table("Log", Schema = "cor")]
    public class Log_dBo : baseDBo
    {
        public Log_dBo() { }
        public Log_dBo(
            string user_Id,
            LogOperation_dEo.LogOperation operation,
            object entity)
        {
            User_Id = user_Id;
            Operation = operation;

            EntityName = entity.GetType().Name;

            var idProp = entity.GetType().GetProperty("Id");

            if (idProp == null)
            {
                EntityId = "-";
            }

            else
            {
                var idValue = idProp.GetValue(entity);
                EntityId = idValue?.ToString() ?? "-";
            }

            EntityLog = JsonSerializer.Serialize(
                entity,
                new JsonSerializerOptions
                {
                    WriteIndented = false,
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles
                });
        }


        [Key]
        public long Id { get; set; }

        public LogOperation_dEo.LogOperation Operation { get; set; }

        [Mandatory]
        [MaxLength(100)]
        public string EntityName { get; set; } = null!;

        [Mandatory]
        [MaxLength(10)]
        public string EntityId { get; set; } = null!;

        [Mandatory]
        public string EntityLog { get; set; } = null!;

        [Mandatory]
        public string User_Id { get; set; } = null!;
        [ForeignKey("User_Id")]
        public virtual User_dBo? User { get; set; }
    }

    public class LogConfiguration : IEntityTypeConfiguration<Log_dBo>
    {
        public void Configure(EntityTypeBuilder<Log_dBo> builder)
        {
            builder.HasIndex(u => u.EntityName);
            builder.HasIndex(u => u.EntityId);
        }
    }
}
