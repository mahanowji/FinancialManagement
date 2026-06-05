using Domain.Entities;
using Domain.Enums;

namespace Application.Common.Dto
{
    public class CommunicationDto
    {
        public CommunicationType Type { get; set; }

        public string Description { get; set; } = null!;

        public DateTime OccurredAt { get; set; }

        public Guid ClientId { get; set; }

        public Client Client { get; set; } = null!;
    }
}
