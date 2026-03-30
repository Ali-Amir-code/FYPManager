namespace FYPManager.WinForms.UI.UserControls;

public partial class PlaceholderSectionControl : UserControl
{
    public PlaceholderSectionControl(string title, string description)
    {
        InitializeComponent();
        lblTitle.Text = title;
        lblDescription.Text = description;
    }
}
