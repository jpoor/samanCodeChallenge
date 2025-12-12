using Saman.Backend.Share.shareClasses;

namespace Saman.Backend.Business.Entity.Authentication
{
    public class Token_dTo : baseDTo
    {
        public Token_dTo() { }
        public Token_dTo(string token, DateTime? expireDate = null)
        {
            Token = token;
            ExpireDate = expireDate;
        }

        public string? Token { get; set; }

        public DateTime? ExpireDate { get; set; }
    }
}
