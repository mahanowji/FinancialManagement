using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Common.Dto
{

    public class CreateClientUserDto
    {
        public Guid ClientId { get; set; }   
        public string Email { get; set; } = null!; 
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Password { get; set; } = null!; 
    }
}
