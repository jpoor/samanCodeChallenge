using Saman.Backend.Business.Entity.Authentication;

namespace Saman.Backend.Business.Entity.Log
{
    public class Log_dToD : Log_dTo
    {
        public Log_dToD() { }
        public Log_dToD(Log_dBo? dbo) : base(dbo)
        {
            if (dbo != null)
            {
                this.EntityLog = dbo.EntityLog;
                this.User = new User_dTo(dbo.User);
            }
        }

        public string EntityLog { get; set; } = null!;

        public User_dTo? User { get; set; }
    }
}
