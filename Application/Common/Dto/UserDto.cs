using Domain.Enums;

public class UserDto
{
    public Guid Id { get; set; }

    public string FullName { get; set; }

    public string Email { get; set; }

    public UserRole Role { get; set; }
}