namespace FYPManager.WinForms.Models;

public sealed class StudentUpsertModel
{
    public int Id { get; set; }
    public string RegistrationNo { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public string? Contact { get; set; }
    public string Email { get; set; } = string.Empty;
    public DateTime? DateOfBirth { get; set; }
    public int? GenderId { get; set; }
}
