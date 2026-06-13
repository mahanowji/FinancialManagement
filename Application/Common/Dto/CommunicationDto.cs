using Domain.Entities;
using Domain.Enums;

namespace Application.Common.Dto
{
    public class CommunicationDto
    {
        public Guid Id { get; set; }
        public CommunicationType Type { get; set; }

        public string Description { get; set; } = null!;

        public DateTime OccurredAt { get; set; }

        public Guid ClientId { get; set; }

    }
}
