namespace FYPManager.WinForms.Utilities;

public static class UiMessageHelper
{
    public static string JoinLines(IEnumerable<string> lines) => string.Join(Environment.NewLine, lines);
}
