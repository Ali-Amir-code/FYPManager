using FYPManager.WinForms.Models;

namespace FYPManager.WinForms.UI.Dialogs;

public partial class GroupDialog : Form
{
    public GroupDialog(GroupUpsertModel? model = null)
    {
        Model = model is null
            ? new GroupUpsertModel { CreatedOn = DateOnly.FromDateTime(DateTime.Today) }
            : new GroupUpsertModel
            {
                Id = model.Id,
                CreatedOn = model.CreatedOn
            };

        InitializeComponent();
    }

    public GroupUpsertModel Model { get; }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Text = Model.Id > 0 ? "Edit Group" : "Create Group";
        dtpCreatedOn.Value = Model.CreatedOn.ToDateTime(TimeOnly.MinValue);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        Model.CreatedOn = DateOnly.FromDateTime(dtpCreatedOn.Value.Date);
        DialogResult = DialogResult.OK;
        Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
        Close();
    }
}
