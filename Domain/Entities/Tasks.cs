using CmsKit.Domain.Abstractions;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Tasks : AuditEntity
    {

        public string Title { get; set; }

        public string? Description { get; set; }

        public EnumTaskStatus Status { get; set; }

        public EnumTaskPriority Priority { get; set; }

        public DateTime? DueDate { get; set; }

        public DateTime? CompletedAt { get; set; }

        public bool IsReminder { get; set; }



        #region Relations

        public Guid ClientId { get; set; }
        public Clients Client { get; set; }

        public Guid CreatedByOperatorId { get; set; }
        public Users CreatedByUser { get; set; }

        public Guid? AssignedToUserId { get; set; }
        public Users AssignedToUser { get; set; }

        #endregion
    }

}
