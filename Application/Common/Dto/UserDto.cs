using Domain.Enums;

public class UserDto
{
    public Guid Id { get; set; }

    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime CreatedAt { get; set; }

    public string Email { get; set; }

    public UserRole Role { get; set; }
}