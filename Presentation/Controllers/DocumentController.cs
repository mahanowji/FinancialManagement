using Application.Common.Dto;
using Application.Common.Interfaces;
using Domain.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class DocumentController : ControllerBase
    {
        private readonly IDocumentService _documentService;
        private readonly IClientService _clientService;
        private readonly ICurrentUserService _currentUserService;

        public DocumentController(
            IDocumentService documentService,
            IClientService clientService,
            ICurrentUserService currentUserService)
        {
            _documentService = documentService;
            _clientService = clientService;
            _currentUserService = currentUserService;
        }

        [HttpPost("upload")]
        [Authorize(Roles = "Admin,Advisor")]  
        public async Task<IActionResult> Upload([FromBody] CreateDocumentDto dto)
        {
            var result = await _documentService.UploadAsync(dto);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { documentId = result.Data, message = "Document uploaded successfully" });
        }

        [HttpGet("client/{clientId}")]
        [Authorize(Roles = "Admin,Advisor,Staff,Client")]
        public async Task<IActionResult> GetClientDocuments(Guid clientId)
        {
   
            if (User.IsInRole("Client"))
            {
                var currentUserId = _currentUserService.GetCurrentUserId();
                var clientResult = await _clientService.GetByUserIdAsync(currentUserId);

                if (!clientResult.IsSuccess || clientResult.Data == null)
                    return Forbid("You don't have access to these documents");

                if (clientResult.Data.Id != clientId)
                    return Forbid("You can only view your own documents");
            }

            var result = await _documentService.GetClientDocumentsAsync(clientId);

            if (!result.IsSuccess)
                return NotFound(new { error = result.Message });

            return Ok(result.Data);
        }

        [HttpDelete("{documentId}")]
        [Authorize(Roles = "Admin,Advisor")]  
        public async Task<IActionResult> Delete(Guid documentId)
        {
            var result = await _documentService.DeleteAsync(documentId);

            if (!result.IsSuccess)
                return BadRequest(new { error = result.Message });

            return Ok(new { message = "Document deleted successfully" });
        }
    }
}