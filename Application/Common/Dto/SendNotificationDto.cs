// Application/Common/Dto/SendNotificationDto.cs
namespace Application.Common.Dto
{
    public class SendNotificationDto
    {
        public Guid ClientId { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
    }
}