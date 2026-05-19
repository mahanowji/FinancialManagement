using CmsKit.Domain.Abstractions;
using System.Reflection.Metadata;

namespace Domain.Entities
{
    public class Notes : AuditEntity
    {

        public string Title { get; set; }

        public string Content { get; set; }

        public bool IsPrivate { get; set; }


        #region Relations
        public ICollection<Document> Attachments { get; set; } = new List<Document>();
        public Guid ClientId { get; set; }
        public Clients Client { get; set; }
        public Guid OperatorId { get; set; }
        public Users Operator { get; set; }

        #endregion
    }
}
