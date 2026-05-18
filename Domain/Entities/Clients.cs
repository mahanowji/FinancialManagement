using Domain.Enums;
using System.Reflection.Metadata;

namespace Domain.Entities
{
    public class Clients
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string? Email { get; set; }

        public string? Phone { get; set; }
        public EnumClientStatus Status { get; set; }
        public string? Tags { get; set; }

        public bool PortalEnabled { get; set; }

        #region Relations
        public Guid? HouseholdId { get; set; }
        public HouseHolds? Household { get; set; }
        public Users? PortalUser { get; set; }
        public Guid? PortalUserId { get; set; }

        public Guid OperatorId { get; set; }

        public Users Operator { get; set; }

        public Guid? ServicePlanId { get; set; }
        public ServicePlans? ServicePlan { get; set; }


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
