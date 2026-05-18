using CmsKit.Domain.Abstractions;
using Domain.Enums;

namespace Domain.Entities
{
    public class User : AuditEntity
    {
        public string Username { get; set; }    
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string PasswordHash { get; set; }
        public string? Email { get; set; }
        public Guid SecurityStamp { get; set; }

        public EnumRoles Roles { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }


        #region 
        #endregion
    }
}
