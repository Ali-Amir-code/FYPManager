using FYPManager.WinForms.Models;

namespace FYPManager.WinForms.UI.Dialogs;

public partial class EvaluationDialog : Form
{
    public EvaluationDialog(EvaluationUpsertModel? model = null)
    {
        Model = model is null
            ? new EvaluationUpsertModel()
            : new EvaluationUpsertModel
            {
                Id = model.Id,
                Name = model.Name,
                TotalMarks = model.TotalMarks,
                TotalWeightage = model.TotalWeightage
            };

        InitializeComponent();
    }

    public EvaluationUpsertModel Model { get; }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        Text = Model.Id > 0 ? "Edit Evaluation" : "Add Evaluation";
        txtName.Text = Model.Name;
        nudTotalMarks.Value = Math.Max(nudTotalMarks.Minimum, Model.TotalMarks);
        nudTotalWeightage.Value = Math.Max(nudTotalWeightage.Minimum, Model.TotalWeightage);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {
        Model.Name = txtName.Text.Trim();
        Model.TotalMarks = (int)nudTotalMarks.Value;
        Model.TotalWeightage = (int)nudTotalWeightage.Value;

        List<string> errors = new();
        if (string.IsNullOrWhiteSpace(Model.Name))
        {
            errors.Add("Evaluation name is required.");
        }

        if (Model.TotalMarks <= 0)
        {
            errors.Add("Total marks must be greater than zero.");
        }

        if (Model.TotalWeightage <= 0)
        {
            errors.Add("Total weightage must be greater than zero.");
        }

        if (errors.Count > 0)
        {
            lblValidation.Text = string.Join(Environment.NewLine, errors);
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
