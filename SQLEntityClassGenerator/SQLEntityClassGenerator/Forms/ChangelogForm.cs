using System;
using System.Windows.Forms;

namespace SQLEntityClassGenerator.Forms
{
    partial class ChangelogForm : Form
    {
        public ChangelogForm()
        {
            InitializeComponent();
            SetChangeLog();
            txtChangelog.Focus();
        }

        private void SetChangeLog()
        {
            txtChangelog.Text = Properties.Resources.Changelog;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
