using CmsKit.Domain.Abstractions;
using CmsKit.Domain.Enums;

namespace Domain.Entities
{
    public class Users : AuditEntity
    {
        public string UserName { get; set; }
        public EnumRole Role { get; set; }
        public string PasswordHash { get; set; }
        public Guid SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }


        #region Relations

        public ICollection<HouseHolds> HouseHolds { get; set; } = new List<HouseHolds>();
        public ICollection<Clients> Clients { get; set; } = new List<Clients>();

        #endregion
    }
}
