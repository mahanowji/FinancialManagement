using Domain.Abstractions;

public interface IDocumentService
{
    Task<ServiceResult<Guid>>
        UploadAsync(CreateDocumentDto dto);

    Task<ServiceResult<List<DocumentDto>>>
        GetClientDocumentsAsync(Guid clientId);

    Task<ServiceResult>
        DeleteAsync(Guid documentId);
}