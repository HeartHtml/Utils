using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using M3.Entities.ExtensionMethods;
using SQLEntityClassGenerator.Classes;
using SQLEntityClassGenerator.Properties;

namespace SQLEntityClassGenerator.Forms
{
    public partial class CustomPropertiesForm : Form
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

        public List<Structures.Property> Properties { get; set; }
        public List<string> Enumerations { get; set; }

        public CustomPropertiesForm()
        {
            InitializeComponent();

            if (Properties == null)
            {
                Properties = new List<Structures.Property>();
            }

            if (Enumerations == null)
            {
                Enumerations = new List<string>();
            }
        }

        #region Events
        private void ddlProperties_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            txtPropertyName.Text = ddlProperties.Text;
        }

        private void btnAddSelection_Click(object sender, System.EventArgs e)
        {
            if (FormIsValid())
            {
                bool isEnum = !ddlEnumeration.Text.IsNullOrWhiteSpace();

                Structures.CustomProperty customProperty 
                    = new Structures.CustomProperty
                        {
                            PropertyName = ddlProperties.Text.TrimSafely(),
                            EnumerationType = isEnum ? ddlEnumeration.Text.TrimSafely() : string.Empty,
                            CustomName = txtPropertyName.Text.TrimSafely(),
                            IsEnumeration = isEnum
                        };

                clbCustomProperties.Items.Add(customProperty, CheckState.Checked);

                ClearForm();
            }
        }

        private void btnOK_Click(object sender, System.EventArgs e)
        {
            Structures.CustomProperties.AddCustomProperties(clbCustomProperties.CheckedItems.Cast<Structures.CustomProperty>());
            DialogResult = DialogResult.OK;
            Close();
        }
        #endregion

        #region Helper Methods
        private void ClearForm()
        {
            ddlProperties.SelectedItem = null;
            ddlEnumeration.SelectedItem = null;
            txtPropertyName.Text = string.Empty;
        }

        public void SetProperties(string connectionString, string tableName)
        {
            Properties = MainForm.GetProperties(connectionString, tableName);
            
            ddlProperties.Items.Clear();
            ddlProperties.Items.AddRange(Properties.Cast<object>().ToArray());
            ddlProperties.DisplayMember = "Name";
        }

        public void SetEnumerations(IEnumerable<string> enumerations)
        {
            if (enumerations.SafeAny())
            {
                enumerations = enumerations.OrderBy(e => e);
            }

            Enumerations.AddRange(enumerations);
            
            ddlEnumeration.Items.Clear();
            ddlEnumeration.Items.AddRange(Enumerations.Cast<object>().ToArray());
        }

        private bool FormIsValid()
        {
            bool isValid = true;
            errorProvider.Clear();

            if (ddlProperties.Text.IsNullOrWhiteSpace())
            {
                isValid = false;
                errorProvider.SetError(ddlProperties, Resources.RequiredFieldError);
            }

            if (txtPropertyName.Text.IsNullOrWhiteSpace())
            {
                isValid = false;
                errorProvider.SetError(txtPropertyName, Resources.RequiredFieldError);
            }

            return isValid;
        }
        #endregion
    }
}
