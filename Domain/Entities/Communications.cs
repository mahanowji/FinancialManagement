using CmsKit.Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities
{
    public class Communications : AuditEntity
    {
        public CommunicationType Type { get; set; }
        public CommunicationDirection Direction { get; set; }
        public string Subject { get; set; }

        public string Summary { get; set; }

        public DateTime OccurredAt { get; set; }

        public int? DurationMinutes { get; set; }

        public bool IsVisibleToClient { get; set; }

        public string? ExternalProviderId { get; set; }// passed to backend form FRONT (using GmailAPI for example)

        #region Relations
        public virtual ICollection<CommunicationAttachment> Attachments { get; set; } = new List<CommunicationAttachment>();
        public Guid ClientId { get; set; }
        public virtual Clients Client { get; set; }

        public Guid UserId { get; set; }
        public virtual Users User { get; set; }
        #endregion
    }
}
