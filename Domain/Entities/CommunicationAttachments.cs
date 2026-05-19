using CmsKit.Domain.Abstractions;
using System.Reflection.Metadata;

namespace Domain.Entities
{
    public class CommunicationAttachment : AuditEntity
    {
        public Guid CommunicationId { get; set; }
        public virtual Communications Communication { get; set; }

        public Guid DocumentId { get; set; }
        public virtual Document Document { get; set; }
    }

}
