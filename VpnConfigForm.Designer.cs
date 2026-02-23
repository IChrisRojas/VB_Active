namespace StayAwake
{
    partial class VpnConfigForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.txtProfile = new System.Windows.Forms.TextBox();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();

            var lblProfile = new System.Windows.Forms.Label { Text = "VPN Profile Name:", Left = 10, Top = 15, Width = 150 };
            this.txtProfile.Left = 10; this.txtProfile.Top = 35; this.txtProfile.Width = 260;

            var lblUser = new System.Windows.Forms.Label { Text = "Username:", Left = 10, Top = 65, Width = 150 };
            this.txtUsername.Left = 10; this.txtUsername.Top = 85; this.txtUsername.Width = 260;

            var lblPass = new System.Windows.Forms.Label { Text = "Password:", Left = 10, Top = 115, Width = 150 };
            this.txtPassword.Left = 10; this.txtPassword.Top = 135; this.txtPassword.Width = 260;
            this.txtPassword.PasswordChar = '*';

            this.btnSave.Text = "Save"; this.btnSave.Left = 110; this.btnSave.Top = 175;
            this.btnSave.Click += new System.EventHandler(this.OnSave);

            this.btnCancel.Text = "Cancel"; this.btnCancel.Left = 195; this.btnCancel.Top = 175;
            this.btnCancel.Click += new System.EventHandler(this.OnCancel);

            this.ClientSize = new System.Drawing.Size(284, 211);
            this.Controls.Add(lblProfile); this.Controls.Add(this.txtProfile);
            this.Controls.Add(lblUser); this.Controls.Add(this.txtUsername);
            this.Controls.Add(lblPass); this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.btnSave); this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VPN Configuration";
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
