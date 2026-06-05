public class CreateTaskDto
{
    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime DueDate { get; set; }

    public Guid ClientId { get; set; }
}