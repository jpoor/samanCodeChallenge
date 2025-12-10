namespace Saman.Backend.Share.shareClasses
{
    public class baseDTo
    {
        public baseDTo() { }
        public baseDTo(baseDBo? dbo)
        {
            if (dbo != null)
            {
                this.CreatorId = dbo.CreatorId;
                this.CreationDate = dbo.CreationDate;
                this.UpdaterId = dbo.UpdaterId;
                this.UpdateDate = dbo.UpdateDate;
            }
        }


        public virtual string CreatorId { get; set; } = null!;

        public virtual DateTime CreationDate { get; set; }

        public virtual string? UpdaterId { get; set; }

        public virtual DateTime? UpdateDate { get; set; }
    }
}
