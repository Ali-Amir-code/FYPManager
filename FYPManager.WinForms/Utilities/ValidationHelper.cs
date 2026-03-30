using System.Text.RegularExpressions;

namespace FYPManager.WinForms.Utilities;

public static class ValidationHelper
{
    private static readonly Regex EmailPattern = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex ContactPattern = new(
        @"^[0-9+\-\s]{7,20}$",
        RegexOptions.Compiled);

    private static readonly Regex RegistrationPattern = new(
        @"^[A-Za-z0-9\-]+$",
        RegexOptions.Compiled);

    public static bool HasValue(string? value) => !string.IsNullOrWhiteSpace(value);

    public static bool IsValidEmail(string email) =>
        HasValue(email) && EmailPattern.IsMatch(email.Trim());

    public static bool IsValidContact(string? contact) =>
        !HasValue(contact) || ContactPattern.IsMatch(contact!.Trim());

    public static bool IsValidRegistrationNumber(string registrationNumber) =>
        HasValue(registrationNumber) && RegistrationPattern.IsMatch(registrationNumber.Trim());
}
