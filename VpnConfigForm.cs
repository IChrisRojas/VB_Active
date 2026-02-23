using System;
using System.Windows.Forms;

namespace StayAwake
{
    public partial class VpnConfigForm : Form
    {
        private TextBox txtProfile;
        private TextBox txtUsername;
        private TextBox txtPassword;
        private Button btnSave;
        private Button btnCancel;

        public VpnConfigForm()
        {
            InitializeComponent();
            LoadCurrentConfig();
        }

        private void LoadCurrentConfig()
        {
            var config = ConfigService.LoadConfig();
            txtProfile.Text = config.ProfileName;
            txtUsername.Text = config.Username;
            txtPassword.Text = config.GetPassword();
        }

        private void OnSave(object sender, EventArgs e)
        {
            var config = new VpnConfig
            {
                ProfileName = txtProfile.Text.Trim(),
                Username = txtUsername.Text.Trim()
            };
            config.SetPassword(txtPassword.Text);

            ConfigService.SaveConfig(config);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void OnCancel(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
