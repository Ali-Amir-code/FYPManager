using FYPManager.WinForms.UI;

namespace FYPManager.WinForms;

static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        AppServices services = new();
        Application.Run(new MainForm(services));
    }
}
