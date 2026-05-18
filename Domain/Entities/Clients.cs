using CmsKit.Domain.Abstractions;
using Domain.Enums;
using System.Reflection.Metadata;

namespace Domain.Entities
{
    public class Clients : AuditEntity
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }
        public ClientStatus Status { get; set; }

        public string? Tags { get; set; }

        public bool PortalEnabled { get; set; }


        #region Relations

        //public Guid? ServicePlanId { get; set; }

        //public ServicePlan? ServicePlan { get; set; }


        //public Guid? PortalUserId { get; set; }

        //public User? PortalUser { get; set; }


        //public ICollection<Note> Notes { get; set; }

        //public ICollection<TaskItem> Tasks { get; set; }

        //public ICollection<Communication> Communications { get; set; }

        //public ICollection<Document> Documents { get; set; }

        //public ICollection<Invoice> Invoices { get; set; }

        //public ICollection<PaymentRecord> Payments { get; set; }

        //public ICollection<Consent> Consents { get; set; }


        #endregion
    }
}
