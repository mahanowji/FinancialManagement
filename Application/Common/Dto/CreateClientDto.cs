public class CreateClientDto
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public Guid AdvisorId { get; set; }
    public Guid HouseholdId { get; set; }
}