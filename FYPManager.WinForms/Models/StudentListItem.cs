namespace FYPManager.WinForms.Models;

public sealed class StudentListItem
{
    public int Id { get; set; }
    public string RegistrationNo { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Contact { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public int? GenderId { get; set; }
    public string? GenderValue { get; set; }
}
