using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dto
{
    public class UpdateClientDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public ClientStatus status { get; set; }

        public Guid AdvisorId { get; set; }
        public Guid HouseholdId { get; set; }
    }
}
