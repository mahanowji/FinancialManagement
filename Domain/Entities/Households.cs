using CmsKit.Domain.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class HouseHolds : AuditEntity
    {
        public string Name { get; set; }

        public string? Description { get; set; }

        public string? PrimaryEmail { get; set; }

        public string? PrimaryPhone { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? PostalCode { get; set; }

        public string? Country { get; set; }

        #region Relations
        public ICollection<Clients> Clients { get; set; } = new List<Clients>();
        public Guid OperatorId { get; set; }
        public Users Operator { get; set; }

        #endregion
    }
}
