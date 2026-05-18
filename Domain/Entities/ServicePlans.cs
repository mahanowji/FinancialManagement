using CmsKit.Domain.Abstractions;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public Guid UserId { get; set; }
        public Users User { get; set; }
        #endregion
    }
}
