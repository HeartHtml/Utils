using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using M3.Entities.ExtensionMethods;
using SQLEntityClassGenerator.Classes;

namespace SQLEntityClassGenerator.Forms
{
    partial class PreferencesForm : Form
    {
        #region Properties
        public Structures.Preferences SQLGeneratorPreferences
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
        private void btnDefaultSaveLocation_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtDefaultSaveLocation.Text.TrimSafely();

            if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                txtDefaultSaveLocation.Text = folderBrowserDialog.SelectedPath.TrimSafely();
            }
        }

        private void btnLibraryLocation_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.SelectedPath = txtLibraryLocation.Text.TrimSafely();

            if (folderBrowserDialog.ShowDialog(this) == DialogResult.OK)
            {
                txtLibraryLocation.Text = folderBrowserDialog.SelectedPath.TrimSafely();
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
        public void LoadFormFromPreferences(Structures.Preferences preferences)
        {
            chkDisableMessageBoxes.Checked = preferences.DisableMessageBoxesPref;
            chkAutoIncludeFiles.Checked = preferences.AutoIncludeFilesPref;
            chkModifyProjectFile.Checked = preferences.ModifyProjectFilePref;
            chkDisableToStringPrompt.Checked = preferences.DisableToStringPromptPref;
            chkDisableCustomProperties.Checked = preferences.DisableCustomPropertiesPromptPref;

            txtDefaultSaveLocation.Text = preferences.DefaultSaveLocationPref;
            txtLibraryLocation.Text = preferences.LibraryLocationPref;
        }

        private Structures.Preferences GatherPreferencesFromForm()
        {
            return new Structures.Preferences
                       {
                           DisableMessageBoxesPref = chkDisableMessageBoxes.Checked,
                           DefaultSaveLocationPref = txtDefaultSaveLocation.Text,
                           AutoIncludeFilesPref = chkAutoIncludeFiles.Checked,
                           LibraryLocationPref = txtLibraryLocation.Text,
                           ModifyProjectFilePref = chkModifyProjectFile.Checked,
                           DisableToStringPromptPref = chkDisableToStringPrompt.Checked,
                           DisableCustomPropertiesPromptPref = chkDisableCustomProperties.Checked
                       };
        }
        #endregion

        #region Validating
        private void txtDefaultSaveLocation_Validating(object sender, CancelEventArgs e)
        {
            if (!txtDefaultSaveLocation.Text.IsNullOrWhiteSpace())
            {
                if (Directory.Exists(txtDefaultSaveLocation.Text))
                {
                    errorProvider.SetError(txtDefaultSaveLocation, string.Empty);
                }
                else
                {
                    txtDefaultSaveLocation.Select(0, txtDefaultSaveLocation.Text.Length);
                    errorProvider.SetError(txtDefaultSaveLocation, "Directory is invalid!");
                    e.Cancel = true;
                }
            }
        }

        private void txtLibraryLocation_Validating(object sender, CancelEventArgs e)
        {
            if (!txtLibraryLocation.Text.IsNullOrWhiteSpace())
            {
                if (Directory.Exists(txtLibraryLocation.Text))
                {
                    bool solutionFound = Directory.EnumerateFiles(txtLibraryLocation.Text).Any(fileName => fileName.Contains("M3Libraries.sln"));

                    if (solutionFound)
                    {
                        errorProvider.SetError(txtLibraryLocation, string.Empty);
                    }
                    else
                    {
                        errorProvider.SetError(txtLibraryLocation, "Library Directory must contain M3Libraries.sln!");
                        e.Cancel = true;
                    }
                }
                else
                {
                    txtLibraryLocation.Select(0, txtLibraryLocation.Text.Length);
                    errorProvider.SetError(txtLibraryLocation, "Directory is invalid!");
                    e.Cancel = true;
                }
            }
        }

        private void chkAutoIncludeFiles_Validating(object sender, CancelEventArgs e)
        {
            if (chkAutoIncludeFiles.Checked && txtLibraryLocation.Text.IsNullOrWhiteSpace())
            {
                errorProvider.SetError(txtLibraryLocation, "Directory is required, if auto include files is checked!");
                e.Cancel = true;
            }
            else
            {
                if (Directory.Exists(txtLibraryLocation.Text))
                {
                    bool solutionFound = Directory.EnumerateFiles(txtLibraryLocation.Text).Any(fileName => fileName.Contains("M3Libraries.sln"));

                    if (solutionFound)
                    {
                        errorProvider.SetError(txtLibraryLocation, string.Empty);
                    }
                    else
                    {
                        errorProvider.SetError(txtLibraryLocation, "Library Directory must contain M3Libraries.sln!");
                        e.Cancel = true;
                    }
                }
            }
        }
        #endregion
    }
}
