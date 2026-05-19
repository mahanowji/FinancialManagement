using CmsKit.Domain.Abstractions;

namespace Domain.Entities
{
    public class Documents : AuditEntity
    {
        public string DisplayName { get; set; }
        public string OriginalFileName { get; set; }
        public string FileKey { get; set; }
        public string MimeType { get; set; }
        public long FileSize { get; set; }
        public string Version { get; set; } = "1.0";
        public string? Description { get; set; }

        public bool IsVisibleToClient { get; set; }
        public DateTime? LastAccessedAt { get; set; }

        public string? MetadataJson { get; set; }

        #region Relations

        public Guid ClientId { get; set; }
        public Clients Client { get; set; }

        public Guid UploadedByUserId { get; set; }
        public Users UploadedByUser { get; set; }

        public Guid? CategoryId { get; set; }
        public DocumentCategories Category { get; set; }

        #endregion
    }
}
