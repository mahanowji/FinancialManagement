public class CreateConsentDto
{
    public Guid ClientId { get; set; }

    public string Description { get; set; } = null!;

    public bool Accepted { get; set; }
}