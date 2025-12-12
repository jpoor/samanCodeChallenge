using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Saman.Backend.Share.shareClasses;
using Saman.Backend.Share.shareValidators.Attributes;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace Saman.Backend.Business.Entity.Authentication
{
    [Table("AspNetUsers", Schema = "dbo")]
    public class User_dBo : IdentityUser
    {
        public User_dBo() : base()
        {
            this.IdentificationCode = shareConvertor.strGUID();
            this.ReferralCode = GenerateReferralCode();
            this.CreationDate = DateTime.UtcNow;
        }

        public User_dBo(
            string username,
            string firstName,
            string? lastName = null,
            string? creatorId = null,
            string? referrerId = null)
        : this()
        {
            this.UserName = username;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.CreatorId = creatorId ?? "0";
            this.Referrer_Id = referrerId;
        }

        [Mandatory]
        public string IdentificationCode { get; set; } = null!;

        [Mandatory]
        [StringLength(6)]
        public string ReferralCode { get; set; } = null!;

        [Mandatory]
        public string FirstName { get; set; } = null!;

        public string? LastName { get; set; }

        public string? ProfileImageFile { get; set; }

        public string CreatorId { get; set; } = null!;

        public DateTime CreationDate { get; set; }

        public string? Referrer_Id { get; set; }
        [ForeignKey("Referrer_Id")]
        public virtual User_dBo? Referrer { get; set; }
     
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string Displayname =>
            string.Concat(
                this.FirstName, " ",
                this.LastName);


        private string GenerateReferralCode()
        {
            const int length = 6;
            char[] _allowedChars = "2345679ACDEFHKMNPRSTUVWXYZ".ToCharArray();
            var data = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(data);

            var sb = new StringBuilder(length);
            foreach (var b in data)
                sb.Append(_allowedChars[b % _allowedChars.Length]);

            return sb.ToString();
        }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User_dBo>
    {
        public void Configure(EntityTypeBuilder<User_dBo> builder)
        {
            builder.HasIndex(u => u.NormalizedUserName).IsUnique();
            builder.HasIndex(u => u.IdentificationCode).IsUnique();
            builder.HasIndex(u => u.ReferralCode).IsUnique();
            builder.HasIndex(u => u.NormalizedEmail).HasFilter("NormalizedEmail IS NOT NULL").IsUnique();
            builder.HasIndex(u => u.PhoneNumber).HasFilter("PhoneNumber IS NOT NULL").IsUnique();

            builder.Property(p => p.CreationDate).HasDefaultValueSql("GETDATE()");

            builder.HasData(
                new User_dBo
                {
                    Id = "usridsuperadmin",
                    IdentificationCode = "OWePCGHUqKgKrAjWSw",
                    UserName = "superadmin",
                    NormalizedUserName = "SUPERADMIN",
                    FirstName = "Super Admin",
                    PasswordHash = "AQAAAAIAAYagAAAAEI8nEg+XoAuBQ0kxNJy3h1sIVpzNQxn7i/TaC5NvpxoN89dJP79nZcQzHPfVZLF+pw==", // 123456
                    SecurityStamp = "N4DNDTNCCHQALRBOVJOYXG3ACWHESEKK",
                    ConcurrencyStamp = "3a23c325-271e-4387-ba65-63d8497e57b4",
                    LockoutEnabled = true,
                    CreatorId = "0"
                });
        }
    }

    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            builder.HasData(
                new IdentityRole { Id = "r01superadmin", Name = "SuperAdmin", NormalizedName = "SUPERADMIN" },
                new IdentityRole { Id = "r02admin", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "r03support", Name = "Support", NormalizedName = "SUPPORT" });
        }
    }

    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string> { RoleId = "r01superadmin", UserId = "usridsuperadmin" });
        }
    }
}
