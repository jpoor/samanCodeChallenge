using Saman.Backend.Share.shareClasses;

namespace Saman.Backend.Business.Entity.Authentication
{
    public class User_dTo : baseDTo
    {
        public User_dTo() { }
        public User_dTo(User_dBo? dbo)
        {
            if (dbo != null)
            {
                this.IdentificationCode = dbo.IdentificationCode;
                this.DisplayName = dbo.Displayname;
                this.ProfileImageFile = dbo.ProfileImageFile;
            }
        }

        public string IdentificationCode { get; set; } = null!;

        public string DisplayName { get; set; } = null!;

        public string? ProfileImageFile { get; set; }
    }
}
