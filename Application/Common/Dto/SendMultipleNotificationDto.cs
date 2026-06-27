// Application/Common/Dto/SendMultipleNotificationDto.cs
namespace Application.Common.Dto
{
    public class SendMultipleNotificationDto
    {
        public List<Guid> ClientIds { get; set; } = new();
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}