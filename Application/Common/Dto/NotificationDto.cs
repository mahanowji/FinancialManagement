// Application/Common/Dto/NotificationDto.cs
namespace Application.Common.Dto
{
    public class NotificationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime SentAt { get; set; }
        public string ClientName { get; set; } = null!;
    }
}