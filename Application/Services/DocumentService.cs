using Application.Common.Interfaces;
using Domain.Abstractions;
using Domain.Enums;

public class DocumentService : IDocumentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;
    private readonly IAuditLogService _auditLogService;

    public DocumentService(
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService,
        IAuditLogService auditLogService)
    {
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
        _auditLogService = auditLogService;
    }

    public async Task<ServiceResult<Guid>> UploadAsync(CreateDocumentDto dto)
    {
        try
        {

            var client = await _unitOfWork.ClientRepository.GetByIdAsync(dto.ClientId);
            if (client == null)
                return new ServiceResult<Guid>("Client not found");


            var document = new Domain.Entities.Document
            {
                Id = Guid.NewGuid(),
                ClientId = dto.ClientId,
                FileName = dto.FileName,
                FilePath = dto.FilePath,
                CategoryId = dto.CategoryId,
                CreatedAt = DateTime.UtcNow,

            };

            await _unitOfWork.DocumentRepository.AddAsync(document);
            await _unitOfWork.CommitChangesAsync();

            // 4. ثبت لاگ
            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Document '{document.FileName}' uploaded for client {client.FirstName} {client.LastName}",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult<Guid>(document.Id);
        }
        catch (Exception ex)
        {
            return new ServiceResult<Guid>($"Error uploading document: {ex.Message}");
        }
    }

    public async Task<ServiceResult<List<DocumentDto>>> GetClientDocumentsAsync(Guid clientId)
    {
        try
        {

            var client = await _unitOfWork.ClientRepository.GetByIdAsync(clientId);
            if (client == null)
                return new ServiceResult<List<DocumentDto>>("Client not found");


            var documents = await _unitOfWork.DocumentRepository.GetByClientIdAsync(clientId);


            var dtos = documents.Select(d => new DocumentDto
            {
                Id = d.Id,
                FileName = d.FileName,
                FilePath = d.FilePath,
                CreatedAt = d.CreatedAt
            }).ToList();


            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Documents viewed for client {client.FirstName} {client.LastName} ({dtos.Count} files)",
                UserId = _currentUserService.GetCurrentUserId()
            });

            return new ServiceResult<List<DocumentDto>>(dtos);
        }
        catch (Exception ex)
        {
            return new ServiceResult<List<DocumentDto>>($"Error getting documents: {ex.Message}");
        }
    }

    public async Task<ServiceResult> DeleteAsync(Guid documentId)
    {
        try
        {

            var document = await _unitOfWork.DocumentRepository.GetByIdAsync(documentId);
            if (document == null)
                return new ServiceResult("Document not found");


            var currentUserId = _currentUserService.GetCurrentUserId();
            var currentUser = await _unitOfWork.UserRepository.GetByIdAsync(currentUserId);

            if (document.ClientId != currentUserId && currentUser?.Role != UserRole.Admin)
                return new ServiceResult("You don't have permission to delete this document");


            await _unitOfWork.DocumentRepository.DeleteAsync(document);
            await _unitOfWork.CommitChangesAsync();


            await _auditLogService.CreateAsync(new CreateAuditLogDto
            {
                Action = $"Document '{document.FileName}' deleted (was for client {document.ClientId})",
                UserId = currentUserId
            });



            return new ServiceResult();
        }
        catch (Exception ex)
        {
            return new ServiceResult($"Error deleting document: {ex.Message}");
        }
    }
}