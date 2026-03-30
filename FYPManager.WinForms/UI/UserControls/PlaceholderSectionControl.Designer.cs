namespace FYPManager.WinForms.UI.UserControls;

partial class PlaceholderSectionControl
{
    private System.ComponentModel.IContainer components = null;
    private Panel cardPanel;
    private Label lblTitle;
    private Label lblDescription;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        cardPanel = new Panel();
        lblDescription = new Label();
        lblTitle = new Label();
        cardPanel.SuspendLayout();
        SuspendLayout();
        cardPanel.BackColor = AppTheme.CardBackColor;
        cardPanel.BorderStyle = BorderStyle.FixedSingle;
        cardPanel.Controls.Add(lblDescription);
        cardPanel.Controls.Add(lblTitle);
        cardPanel.Location = new Point(40, 40);
        cardPanel.Name = "cardPanel";
        cardPanel.Padding = new Padding(24);
        cardPanel.Size = new Size(640, 180);
        lblDescription.Dock = DockStyle.Top;
        lblDescription.Font = new Font("Segoe UI", 11F);
        lblDescription.ForeColor = AppTheme.TextMuted;
        lblDescription.Location = new Point(24, 62);
        lblDescription.Name = "lblDescription";
        lblDescription.Size = new Size(590, 70);
        lblDescription.Text = "Description";
        lblTitle.Dock = DockStyle.Top;
        lblTitle.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
        lblTitle.ForeColor = AppTheme.TextPrimary;
        lblTitle.Location = new Point(24, 24);
        lblTitle.Name = "lblTitle";
        lblTitle.Size = new Size(590, 38);
        lblTitle.Text = "Title";
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = AppTheme.ContentBackColor;
        Controls.Add(cardPanel);
        Name = "PlaceholderSectionControl";
        Size = new Size(960, 560);
        cardPanel.ResumeLayout(false);
        ResumeLayout(false);
    }
}
