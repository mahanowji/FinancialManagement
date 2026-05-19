using CmsKit.Domain.Abstractions;

namespace Domain.Entities
{
    public class DocumentCategories : AuditEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }
        public string? Icon { get; set; }
        public bool IsSystemDefined { get; set; } = false;

        #region Relations
        public Users? Operator { get; set; }
        public Guid? OperatorId { get; set; }
        public virtual ICollection<Documents> Documents { get; set; } = new HashSet<Documents>();

        #endregion
    }
}
