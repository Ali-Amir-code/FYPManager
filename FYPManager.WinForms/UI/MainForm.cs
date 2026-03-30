using FYPManager.WinForms.UI.UserControls;

namespace FYPManager.WinForms.UI;

public partial class MainForm : Form
{
    private readonly Dictionary<string, Button> _navButtons = new();

    public MainForm(AppServices services)
    {
        Services = services;
        InitializeComponent();
        BuildNavigationMap();
        ShowSection("Students", new StudentsControl(Services));
    }

    public AppServices Services { get; }

    private void BuildNavigationMap()
    {
        _navButtons["Students"] = btnStudents;
        _navButtons["Advisors"] = btnAdvisors;
        _navButtons["Projects"] = btnProjects;
        _navButtons["Groups"] = btnGroups;
        _navButtons["Evaluations"] = btnEvaluations;
        _navButtons["Reports"] = btnReports;
    }

    private void ShowSection(string title, Control control)
    {
        lblSectionTitle.Text = title;

        foreach ((string key, Button button) in _navButtons)
        {
            button.BackColor = key == title ? AppTheme.SidebarAccentColor : AppTheme.SidebarBackColor;
        }

        contentPanel.Controls.Clear();
        control.Dock = DockStyle.Fill;
        contentPanel.Controls.Add(control);
    }

    private void btnStudents_Click(object sender, EventArgs e) => ShowSection("Students", new StudentsControl(Services));
    private void btnAdvisors_Click(object sender, EventArgs e) => ShowSection("Advisors", new AdvisorsControl(Services));
    private void btnProjects_Click(object sender, EventArgs e) => ShowSection("Projects", new ProjectsControl(Services));
    private void btnGroups_Click(object sender, EventArgs e) => ShowSection("Groups", new GroupsControl(Services));
    private void btnEvaluations_Click(object sender, EventArgs e) => ShowSection("Evaluations", new EvaluationsControl(Services));
    private void btnReports_Click(object sender, EventArgs e) => ShowSection("Reports", new ReportsControl(Services));
}
