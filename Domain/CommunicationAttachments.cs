using Domain.Entities;

namespace Domain
{
    public class CommunicationAttachments
    {
        public Guid CommunicationId { get; set; }
        public virtual Communications Communication { get; set; }

        //public Guid DocumentId { get; set; }
        //public virtual Document Document { get; set; }
    }
}
