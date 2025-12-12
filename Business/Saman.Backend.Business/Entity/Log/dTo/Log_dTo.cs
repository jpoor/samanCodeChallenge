using Saman.Backend.Share.shareClasses;

namespace Saman.Backend.Business.Entity.Log
{
    public class Log_dTo : baseDTo
    {
        public Log_dTo() { }
        public Log_dTo(Log_dBo? dbo)
        : base(dbo)
        {
            if (dbo != null)
            {
                this.Id = dbo.Id;
                this.Operation = dbo.Operation;
                this.Operation_Title = LogOperation_dEo.Title(dbo.Operation);
                this.EntityName = dbo.EntityName;
                this.EntityId = dbo.EntityId;
            }
        }

        public long Id { get; set; }

        public LogOperation_dEo.LogOperation Operation { get; set; }

        public string? Operation_Title { get; set; }

        public string EntityName { get; set; } = null!;

        public string EntityId { get; set; } = null!;
    }
}
