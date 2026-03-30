namespace FYPManager.WinForms.Models;

public sealed class Advisor
{
    public int Id { get; set; }
    public int Designation { get; set; }
    public decimal? Salary { get; set; }
    public Person Person { get; set; } = new();
}
