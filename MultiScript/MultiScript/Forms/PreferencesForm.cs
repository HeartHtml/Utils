using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using MultiScriptLib;
using UtilsLib.ExtensionMethods;

namespace MultiScript.Forms
{
    partial class PreferencesForm : Form
    {
        #region Properties
        public Preferences MultiScriptPreferences
        {
            get
            {
                return GatherPreferencesFromForm();
            }
        }
        #endregion

        public PreferencesForm()
        {
            InitializeComponent();
        }

        #region Events
        private void btnDefaultFolderLocation_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtDefaultFolderLocation.Text.TrimSafely();

            if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                txtDefaultFolderLocation.Text = folderBrowserDialog.SelectedPath.TrimSafely();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            if (ValidateChildren(ValidationConstraints.Enabled))
            {
                DialogResult = DialogResult.OK;
                Close();
            }
            else
            {
                DialogResult = DialogResult.None;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            errorProvider.Clear();

            DialogResult = DialogResult.Cancel;
            Close();
        }
        #endregion

        #region Helper Methods

        public void LoadFormFromPreferences(Preferences preferences)
        {
            chkRemoveSuccessfulLog.Checked = preferences.RemoveSuccessfulLogs;
            txtDefaultFolderLocation.Text = preferences.DefaultFolderLocation;
        }

        private Preferences GatherPreferencesFromForm()
        {
            return new Preferences
                       {
                           RemoveSuccessfulLogs = chkRemoveSuccessfulLog.Checked,
                           DefaultFolderLocation = txtDefaultFolderLocation.Text.TrimSafely(),
                       };
        }
        #endregion

        #region Validating
        private void txtDefaultFolderLocation_Validating(object sender, CancelEventArgs e)
        {
            if (!txtDefaultFolderLocation.Text.IsNullOrWhiteSpace())
            {
                if (Directory.Exists(txtDefaultFolderLocation.Text))
                {
                    errorProvider.SetError(txtDefaultFolderLocation, string.Empty);
                }
                else
                {
                    txtDefaultFolderLocation.Select(0, txtDefaultFolderLocation.Text.Length);
                    errorProvider.SetError(txtDefaultFolderLocation, "Directory is invalid!");
                    e.Cancel = true;
                }
            }
        }
        #endregion
    }
}
