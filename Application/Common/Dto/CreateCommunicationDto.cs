using Domain.Enums;

public class CreateCommunicationDto
{
    public Guid ClientId { get; set; }

    public CommunicationType Type { get; set; }

    public string Description { get; set; }
}