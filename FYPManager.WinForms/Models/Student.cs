namespace FYPManager.WinForms.Models;

public sealed class Student
{
    public int Id { get; set; }
    public string RegistrationNo { get; set; } = string.Empty;
    public Person Person { get; set; } = new();
}
