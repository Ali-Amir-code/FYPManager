using FYPManager.WinForms.Models;
using FYPManager.WinForms.Utilities;

namespace FYPManager.WinForms.UI.Dialogs;

public partial class ProjectDialog : Form
{
    public ProjectDialog(ProjectUpsertModel? model = null)
    {
        Model = model is null
            ? new ProjectUpsertModel()
            : new ProjectUpsertModel
            {
                Id = model.Id,
                Title = model.Title,
                Description = model.Description
            };

        InitializeComponent();
    }

    public ProjectUpsertModel Model { get; }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Text = Model.Id > 0 ? "Edit Project" : "Add Project";
        txtTitle.Text = Model.Title;
        txtDescription.Text = Model.Description;
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        Model.Title = txtTitle.Text.Trim();
        Model.Description = string.IsNullOrWhiteSpace(txtDescription.Text) ? null : txtDescription.Text.Trim();

        List<string> errors = new();
        if (!ValidationHelper.HasValue(Model.Title))
        {
            errors.Add("Title is required.");
        }
        else if (Model.Title.Length > 50)
        {
            errors.Add("Title cannot be longer than 50 characters.");
        }

        if (errors.Count > 0)
        {
            lblValidation.Text = UiMessageHelper.JoinLines(errors);
            return;
        }

        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
