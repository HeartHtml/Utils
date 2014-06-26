using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SQLEntityClassGenerator.Classes;
using SQLEntityClassGenerator.Properties;

namespace SQLEntityClassGenerator.Forms
{
    partial class ColumnsForm : Form
    {
        private const int CP_NOCLOSE_BUTTON = 0x200;
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams myCp = base.CreateParams;
                myCp.ClassStyle = myCp.ClassStyle | CP_NOCLOSE_BUTTON;
                return myCp;
            }
        }

        public List<Structures.Property> SelectedColumns { get; set; }

        public ColumnsForm()
        {
            InitializeComponent();

            if (SelectedColumns == null)
            {
                SelectedColumns = new List<Structures.Property>();
            }
        }

        public void SetInitialColumns(string connectionString, string tableName)
        {
            List<Structures.Property> properties = MainForm.GetProperties(connectionString, tableName);

            foreach (Structures.Property property in properties)
            {
                clbColumns.Items.Add(property);

                if (property.IsPrimaryKey || property.IsIdentity)
                {
                    clbColumns.SetItemChecked(clbColumns.Items.Count - 1, true);
                }
            }

            clbColumns.DisplayMember = "Name";
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            SelectedColumns.AddRange(clbColumns.CheckedItems.Cast<Structures.Property>());
            DialogResult = DialogResult.OK;
            Close();
        }

        private void ColumnsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (clbColumns.CheckedItems.Count == 0)
            {
                e.Cancel = true;

                DialogResult = DialogResult.Abort;

                MessageBox.Show(this,
                                Resources.PleaseSelectAtLeastOneColumn,
                                Resources.NoColumnsSelected,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Exclamation);
            }
        }
    }
}
