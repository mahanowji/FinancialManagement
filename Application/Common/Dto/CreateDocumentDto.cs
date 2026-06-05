public class CreateDocumentDto
{
    public Guid ClientId { get; set; }

    public string FileName { get; set; }

    public string FilePath { get; set; }

    public Guid CategoryId { get; set; }
}