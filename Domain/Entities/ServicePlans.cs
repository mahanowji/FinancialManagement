using CmsKit.Domain.Abstractions;
using Domain.Enums;


namespace Domain.Entities
{
    public class ServicePlans : AuditEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public EnumBillingCycles BillingCycle { get; set; }

        public bool IsActive { get; set; }

        #region Relations
        public ICollection<Clients> Clients { get; set; } = new List<Clients>();
        public Guid OperatorId { get; set; }
        public Users Operator { get; set; }
        #endregion
    }
}
