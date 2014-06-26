using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Security.Principal;
using System.Text;
using System.Windows.Forms;
using ADODB;
using M3.Entities.ExtensionMethods;
using MSDASC;
using SQLEntityClassGenerator.Classes;
using SQLEntityClassGenerator.Properties;

namespace SQLEntityClassGenerator.Forms
{
    public partial class MainForm : Form
    {
        #region Properties
        private Structures.EnumApplicationID SelectedApplication
        {
            get
            {
                Structures.EnumApplicationID application = Structures.EnumApplicationID.GPSApplication;
                IEnumerable<Control> checkedApplications = pnlApplication.Controls.Cast<Control>().Where(control => control is RadioButton && ((RadioButton)control).Checked);
                if (checkedApplications.Any())
                {
                    RadioButton rdoButton = checkedApplications.FirstOrDefault() as RadioButton;
                    if (rdoButton != null)
                    {
                        application = (Structures.EnumApplicationID)Convert.ToInt32(rdoButton.Tag);
                    }
                }
                else if (!txtNamespace.Text.IsNullOrWhiteSpace())
                {
                    string nameSpace = txtNamespace.Text.TrimSafely();
                    nameSpace = nameSpace.Replace("M3", string.Empty);
                    nameSpace = nameSpace.Replace("Entities", string.Empty);
                    nameSpace = nameSpace.Replace("Data", string.Empty);
                    nameSpace = nameSpace.Replace("Business", string.Empty);
                    nameSpace = nameSpace.Replace("Tests", string.Empty);
                    nameSpace = nameSpace.Replace("NonPersistent", string.Empty);
                    nameSpace = nameSpace.Replace(".", string.Empty);

                    switch (nameSpace)
                    {
                        default:
                            application = Structures.EnumApplicationID.GPSApplication;
                            break;
                    }
                }
                return application;
            }
        }

        private string SelectedTable
        {
            get { return cmbTables.Text.TrimSafely(); }
        }

        private bool SelectedTableIsView
        {
            get { return IsTableAView(cmbTables.Text); }
        }

        private string CustomName
        {
            get { return txtCustomName.Text.IsNullOrWhiteSpace() ? null : txtCustomName.Text.TrimSafely().FirstCharUpper(); }
        }

        private bool UsesCustomName
        {
            get { return !CustomName.IsNullOrWhiteSpace(); }
        }

        private static List<string> Enumerations { get; set; }

        #region Preferences Properties
        private Structures.Preferences FormPreferences
        {
            get
            {
                return new Structures.Preferences
                    {
                        DefaultSaveLocationPref = DefaultSaveLocation,
                        DisableMessageBoxesPref = DisableMessageBoxes,
                        AutoIncludeFilesPref = AutoIncludeFiles,
                        LibraryLocationPref = LibraryLocation,
                        ModifyProjectFilePref = ModifyProjectFile,
                        DisableToStringPromptPref = DisableToStringPrompt,
                        DisableCustomPropertiesPromptPref = DisableCustomPropertiesPrompt
                    };
            }
        }

        private static bool DisableMessageBoxes
        {
            get { return Settings.Default.DisableMessageBoxes; }
            set
            {
                Settings.Default.DisableMessageBoxes = value;
                Settings.Default.Save();
            }
        }

        private static string DefaultSaveLocation
        {
            get { return Settings.Default.DefaultSaveLocation; }
            set
            {
                Settings.Default.DefaultSaveLocation = value;
                Settings.Default.Save();
            }
        }

        private bool AutoIncludeFiles
        {
            get { return Settings.Default.AutoIncludeFiles; }
            set
            {
                Settings.Default.AutoIncludeFiles = value;
                Settings.Default.Save();

                txtSaveNotEnabled.Visible = value;
            }
        }

        private static string LibraryLocation
        {
            get { return Settings.Default.LibraryLocation; }
            set
            {
                Settings.Default.LibraryLocation = value;
                Settings.Default.Save();
            }
        }

        private static bool ModifyProjectFile
        {
            get { return Settings.Default.ModifyProjectFile; }
            set
            {
                Settings.Default.ModifyProjectFile = value;
                Settings.Default.Save();
            }
        }

        private static bool DisableToStringPrompt
        {
            get { return Settings.Default.DisableToStringPrompt; }
            set
            {
                Settings.Default.DisableToStringPrompt = value;
                Settings.Default.Save();
            }
        }

        private static bool DisableCustomPropertiesPrompt
        {
            get { return Settings.Default.DisableCustomPropertiesPrompt; }
            set
            {
                Settings.Default.DisableCustomPropertiesPrompt = value;
                Settings.Default.Save();
            }
        }
        #endregion
        #endregion

        public MainForm()
        {
            Clipboard.Clear();

            InitializeComponent();

            txtCustomName.Visible = false;
            txtCustomName.Text = null;

            Enumerations = new List<string>();
            RefreshEnumList();
        }

        #region Generate Methods
        private void GeneratingAll()
        {
            if (ValidateForm(Structures.EnumLayer.All))
            {
                Clipboard.Clear();

                Structures.CustomProperties.ClearCustomProperties();

                if (!DisableCustomPropertiesPrompt)
                {
                    if (!Enumerations.SafeAny())
                    {
                        //If no enumerations in our list, try refreeshing one more time
                        RefreshEnumList();
                    }

                    using (CustomPropertiesForm customPropertiesForm = new CustomPropertiesForm())
                    {
                        customPropertiesForm.SetProperties(txtConnStr.Text.TrimSafely(), SelectedTable);
                        customPropertiesForm.SetEnumerations(Enumerations);
                        customPropertiesForm.ShowDialog(this);
                    }
                }

                GeneratingStoredProcedures();

                DetermineNamespace(Structures.EnumLayer.Entities);
                GeneratingSearch();
                GeneratingEntity();

                DetermineNamespace(Structures.EnumLayer.Data);
                GeneratingData();

                DetermineNamespace(Structures.EnumLayer.Business);
                GeneratingManager();

                DetermineNamespace(Structures.EnumLayer.Tests);
                GeneratingTests();

                ShowMessageBox(this,
                               Resources.AllLayersHaveBeenGenerated,
                               Resources.HugeSuccess,
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);

                txtNamespace.Clear();
            }
        }

        private void GeneratingStoredProcedures()
        {
            if (ValidateForm(Structures.EnumLayer.StoredProcedure))
            {
                Clipboard.Clear();

                string tableName = SelectedTable;
                string connStr = txtConnStr.Text.TrimSafely();

                if (rdoSaveToFile.Checked)
                {
                    string fileName = string.Format("{0}SP{1}", GetCustomNameIfSet(tableName), Resources.SQLExt);

                    if (DefaultSaveLocation.IsNullOrWhiteSpace())
                    {
                        saveFileDialog1.FileName = fileName;
                        saveFileDialog1.Filter = Resources.SQLFiles;

                        if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                        {
                            fileName = saveFileDialog1.FileName;
                        }
                    }
                    else
                    {
                        fileName = Path.Combine(DefaultSaveLocation, fileName);
                    }

                    if (!fileName.IsNullOrWhiteSpace())
                    {
                        GenerateStoredProcedures(new Structures.SQLEntityGeneratorOptions
                            {
                                FileName = fileName,
                                ConnectionString = connStr,
                                TableName = tableName
                            });
                    }
                }
                else
                {
                    GenerateStoredProcedures(new Structures.SQLEntityGeneratorOptions
                        {
                            FileName = string.Format("{0}{1}", GetCustomNameIfSet(tableName), Resources.SQLExt),
                            ConnectionString = connStr,
                            TableName = tableName
                        });
                }
            }
        }

        private void GeneratingSearch()
        {
            Clipboard.Clear();

            string tableName = SelectedTable;
            bool isView = SelectedTableIsView;
            string connStr = txtConnStr.Text.TrimSafely();
            string nameSpace = string.Format("{0}{1}", txtNamespace.Text.TrimSafely(), isView ? Resources.NonPersistent : string.Empty);
            string fileName = string.Format("Search{0}{1}", GetCustomNameIfSet(FixName(tableName, isView)), Resources.CSharpExt);

            if (rdoSaveToFile.Checked)
            {
                if (DefaultSaveLocation.IsNullOrWhiteSpace())
                {
                    saveFileDialog1.FileName = fileName;
                    saveFileDialog1.Filter = Resources.CSharpFiles;

                    if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                    {
                        fileName = saveFileDialog1.FileName;
                    }
                }
                else
                {
                    fileName = Path.Combine(DefaultSaveLocation, fileName);
                }

                if (!fileName.IsNullOrWhiteSpace())
                {
                    GenerateSearchEntity(new Structures.SQLEntityGeneratorOptions
                        {
                            FileName = fileName,
                            ConnectionString = connStr,
                            TableName = tableName,
                            NameSpace = nameSpace
                        });
                }
            }
            else
            {
                GenerateSearchEntity(new Structures.SQLEntityGeneratorOptions
                    {
                        FileName = fileName,
                        ConnectionString = connStr,
                        TableName = tableName,
                        NameSpace = nameSpace
                    });
            }
        }

        private void GeneratingEntity()
        {
            Clipboard.Clear();

            string tableName = SelectedTable;
            bool isView = SelectedTableIsView;
            string connStr = txtConnStr.Text.TrimSafely();
            string nameSpace = string.Format("{0}{1}", txtNamespace.Text.TrimSafely(), isView ? Resources.NonPersistent : string.Empty);
            string fileName = string.Format("{0}{1}", GetCustomNameIfSet(FixName(tableName, isView)), Resources.CSharpExt);

            List<Structures.Property> selectedColumns = new List<Structures.Property>();

            if (!DisableToStringPrompt)
            {
                using (ColumnsForm form = new ColumnsForm())
                {
                    form.SetInitialColumns(connStr, tableName);
                    if (form.ShowDialog(this) == DialogResult.OK)
                    {
                        selectedColumns.AddRange(form.SelectedColumns);
                    }
                }
            }

            if (rdoSaveToFile.Checked)
            {
                if (DefaultSaveLocation.IsNullOrWhiteSpace())
                {
                    saveFileDialog1.FileName = fileName;
                    saveFileDialog1.Filter = Resources.CSharpFiles;

                    if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                    {
                        fileName = saveFileDialog1.FileName;
                    }
                }
                else
                {
                    fileName = Path.Combine(DefaultSaveLocation, fileName);
                }

                if (!fileName.IsNullOrWhiteSpace())
                {
                    GenerateClass(new Structures.SQLEntityGeneratorOptions
                        {
                            FileName = fileName,
                            ConnectionString = connStr,
                            TableName = tableName,
                            NameSpace = nameSpace,
                            SelectedColumns = selectedColumns
                        });
                }
            }
            else
            {
                GenerateClass(new Structures.SQLEntityGeneratorOptions
                    {
                        FileName = fileName,
                        ConnectionString = connStr,
                        TableName = tableName,
                        NameSpace = nameSpace,
                        SelectedColumns = selectedColumns
                    });
            }
        }

        private void GeneratingData()
        {
            Clipboard.Clear();

            string tableName = SelectedTable;
            bool isView = SelectedTableIsView;
            string connStr = txtConnStr.Text.TrimSafely();
            string nameSpace = string.Format("{0}{1}", txtNamespace.Text.TrimSafely(), isView ? Resources.NonPersistent : string.Empty);
            string fileName = string.Format("{0}Dao{1}", GetCustomNameIfSet(FixName(tableName, isView)), Resources.CSharpExt);

            if (rdoSaveToFile.Checked)
            {
                if (DefaultSaveLocation.IsNullOrWhiteSpace())
                {
                    saveFileDialog1.FileName = fileName;
                    saveFileDialog1.Filter = Resources.CSharpFiles;

                    if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                    {
                        fileName = saveFileDialog1.FileName;
                    }
                }
                else
                {
                    fileName = Path.Combine(DefaultSaveLocation, fileName);
                }

                if (!fileName.IsNullOrWhiteSpace())
                {
                    GenerateDAO(new Structures.SQLEntityGeneratorOptions
                        {
                            FileName = fileName,
                            ConnectionString = connStr,
                            TableName = tableName,
                            NameSpace = nameSpace
                        });
                }
            }
            else
            {
                GenerateDAO(new Structures.SQLEntityGeneratorOptions
                    {
                        FileName = fileName,
                        ConnectionString = connStr,
                        TableName = tableName,
                        NameSpace = nameSpace
                    });
            }
        }

        private void GeneratingManager()
        {
            Clipboard.Clear();

            string tableName = SelectedTable;
            bool isView = SelectedTableIsView;
            string connStr = txtConnStr.Text.TrimSafely();
            string nameSpace = string.Format("{0}{1}", txtNamespace.Text.TrimSafely(), isView ? Resources.NonPersistent : string.Empty);
            string fileName = string.Format("{0}Manager{1}", GetCustomNameIfSet(FixName(tableName, isView)), Resources.CSharpExt);

            if (rdoSaveToFile.Checked)
            {
                if (DefaultSaveLocation.IsNullOrWhiteSpace())
                {
                    saveFileDialog1.FileName = fileName;
                    saveFileDialog1.Filter = Resources.CSharpFiles;

                    if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                    {
                        fileName = saveFileDialog1.FileName;
                    }
                }
                else
                {
                    fileName = Path.Combine(DefaultSaveLocation, fileName);
                }

                if (!fileName.IsNullOrWhiteSpace())
                {
                    GenerateManager(new Structures.SQLEntityGeneratorOptions
                        {
                            FileName = fileName,
                            ConnectionString = connStr,
                            TableName = tableName,
                            NameSpace = nameSpace
                        });
                }
            }
            else
            {
                GenerateManager(new Structures.SQLEntityGeneratorOptions
                    {
                        FileName = fileName,
                        ConnectionString = connStr,
                        TableName = tableName,
                        NameSpace = nameSpace
                    });
            }
        }

        private void GeneratingTests()
        {
            Clipboard.Clear();

            string tableName = SelectedTable;
            bool isView = SelectedTableIsView;
            string connStr = txtConnStr.Text.TrimSafely();
            string nameSpace = txtNamespace.Text.TrimSafely();
            string fileName = string.Format("{0}ManagerTest{1}", GetCustomNameIfSet(FixName(tableName, isView)), Resources.CSharpExt);

            if (rdoSaveToFile.Checked)
            {
                if (DefaultSaveLocation.IsNullOrWhiteSpace())
                {
                    saveFileDialog1.FileName = fileName;
                    saveFileDialog1.Filter = Resources.CSharpFiles;

                    if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                    {
                        fileName = saveFileDialog1.FileName;
                    }
                }
                else
                {
                    fileName = Path.Combine(DefaultSaveLocation, fileName);
                }

                if (!fileName.IsNullOrWhiteSpace())
                {
                    GenerateTests(new Structures.SQLEntityGeneratorOptions
                        {
                            FileName = fileName,
                            ConnectionString = connStr,
                            TableName = tableName,
                            NameSpace = nameSpace
                        });
                }
            }
            else
            {
                GenerateTests(new Structures.SQLEntityGeneratorOptions
                    {
                        FileName = fileName,
                        ConnectionString = connStr,
                        TableName = tableName,
                        NameSpace = nameSpace
                    });
            }
        }

        private void GenerateStoredProcedures(Structures.SQLEntityGeneratorOptions options)
        {
            SqlConnection connection = new SqlConnection(options.ConnectionString);
            string database = connection.Database;

            bool isView = IsTableAView(options.TableName);

            string template = ReadResource(Structures.EnumLayer.StoredProcedure, isView);

            string prefix = DetermineSPPrefix();

            template = template.Replace("[PREFIX]", prefix);

            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            if (windowsIdentity != null)
            {
                string author = windowsIdentity.Name.Replace(Resources.Domain, string.Empty);

                if (author.IsNullOrWhiteSpace())
                {
                    author = Resources.DefaultAuthor;
                }

                options.FileName = options.FileName.FirstCharUpper();
                //options.TableName = options.TableName.FirstCharUpper();

                template = template.Replace("[AUTHOR]", author);
            }

            template = template.Replace("[TABLENAME]", options.TableName);
            template = options.TableName.Length > 26
                           ? template.Replace("[TABLENAMETRANS]", options.TableName.Substring(0, 26))
                           : template.Replace("[TABLENAMETRANS]", options.TableName);
            template = template.Replace("[DATABASE]", database);
            template = template.Replace("[CREATEDATE]", DateTime.Today.ToString("MM/dd/yyyy"));

            List<Structures.Property> properties = GetProperties(options.ConnectionString, options.TableName);

            if (properties != null)
            {
                #region Properties String Building
                StringBuilder propertiesSelectBuilder = new StringBuilder();
                StringBuilder propertiesSelectParamBuilder = new StringBuilder();
                StringBuilder propertiesSelectNullableBuilder = new StringBuilder();
                StringBuilder propertiesInsertBuilder = new StringBuilder();
                StringBuilder propertiesUpdateBuilder = new StringBuilder();
                StringBuilder propertiesDeleteBuilder = new StringBuilder();
                StringBuilder propertiesColumnNamesBuilder = new StringBuilder();
                StringBuilder propertiesColumnNamesWithoutIdentityBuilder = new StringBuilder();
                StringBuilder propertiesParametersBuilder = new StringBuilder();
                StringBuilder propertiesWhereClauseBuilder = new StringBuilder();
                StringBuilder propertiesUpdateStatementBuilder = new StringBuilder();
                StringBuilder propertiesDyanmicParamsBuilder = new StringBuilder();

                #region Loop Through Properties
                foreach (Structures.Property property in properties)
                {
                    string propertyName = property.Name.FirstCharUpper().TrimSafely();
                    string sqlDataType = ConvertToSQLDataType(property).TrimSafely();

                    if (properties.IndexOf(property) != 0)
                    {
                        propertiesSelectBuilder.Append("            ");
                        propertiesSelectParamBuilder.Append("            ");
                    }

                    if (!property.IsIdentity && !property.IsComputed)
                    {
                        propertiesInsertBuilder.AppendLine(string.Format("    @{0} {1},", propertyName, sqlDataType));
                    }

                    if (!property.IsComputed)
                    {
                        propertiesUpdateBuilder.AppendLine(string.Format("    @{0} {1},", propertyName, sqlDataType));
                    }

                    if (property.IsPrimaryKey)
                    {
                        propertiesDeleteBuilder.AppendLine(string.Format("    @{0} {1},", propertyName, sqlDataType));
                        propertiesWhereClauseBuilder.AppendLine(string.Format("{0} = @{0} AND", property.Name));
                    }

                    if (!property.IsIdentity)
                    {
                        propertiesColumnNamesWithoutIdentityBuilder.AppendLine(string.Format("            {0},", property.Name));
                        propertiesParametersBuilder.AppendLine(string.Format("            @{0},", property.Name));
                        propertiesUpdateStatementBuilder.AppendLine(string.Format("            {0} = @{0},", property.Name));
                    }

                    propertiesSelectBuilder.AppendLine(string.Format("@{0},", propertyName));

                    propertiesSelectParamBuilder.AppendLine(string.Format("@{0} {1},", propertyName, sqlDataType));

                    propertiesSelectNullableBuilder.AppendLine(string.Format("    @{0} {1} = NULL,", propertyName, sqlDataType));

                    propertiesColumnNamesBuilder.AppendLine(string.Format("            {0},", property.Name.TrimSafely()));

                    #region Dynamic Parameters
                    propertiesDyanmicParamsBuilder.AppendLine(string.Format("        IF @{0} IS NOT NULL", property.Name));
                    propertiesDyanmicParamsBuilder.AppendLine("        BEGIN");

                    if (property.IsDateTime)
                    {
                        propertiesDyanmicParamsBuilder.AppendLine(string.Format("            SET @SQL = @SQL + ' AND {0} {1} {2} '",
                                                                  property.Name,
                                                                  property.Comparer,
                                                                  string.Format(Resources.DateComparisonStringFormat, property.Name)));
                    }
                    else
                    {
                        propertiesDyanmicParamsBuilder.AppendLine(string.Format("            SET @SQL = @SQL + ' AND {0} {1} @{0} '",
                                                                  property.Name,
                                                                  property.Comparer));
                    }

                    propertiesDyanmicParamsBuilder.AppendLine("        END");
                    propertiesDyanmicParamsBuilder.AppendLine(string.Empty);
                    #endregion
                }
                #endregion

                template = template.Replace("[SQLInsertParamList]", propertiesInsertBuilder.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[SQLDeleteParamList]", propertiesDeleteBuilder.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[SQLUpdateParamList]", propertiesUpdateBuilder.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[SQLSelectList]", propertiesSelectBuilder.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[SQLSelectNullParamList]", propertiesSelectNullableBuilder.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[COLUMNNAMES]", propertiesColumnNamesBuilder.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[SQLSelectParamList]", propertiesSelectParamBuilder.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[COLUMNNAMESWOIDENT]", propertiesColumnNamesWithoutIdentityBuilder.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[PARAMETERS]", propertiesParametersBuilder.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[DELETECONDITION]", propertiesWhereClauseBuilder.ToString().RemoveLastInstanceOfComma().RemoveLastInstanceOfWord("AND"));
                template = template.Replace("[SQLUpdateWHERE]", propertiesWhereClauseBuilder.ToString().RemoveLastInstanceOfComma().RemoveLastInstanceOfWord("AND"));
                template = template.Replace("[SQLUpdateStatement]", propertiesUpdateStatementBuilder.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[DYNAMICPARAMETERS]", propertiesDyanmicParamsBuilder.ToString().TrimEnd());
                #endregion
            }

            bool saved = SaveContent(Structures.EnumLayer.StoredProcedure, options.FileName, template);

            if (saved)
            {
                ShowMessageBox(this, Resources.StoredProceduresCreated, Resources.Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GenerateSearchEntity(Structures.SQLEntityGeneratorOptions options)
        {
            options.FileName = options.FileName.FirstCharUpper();
            options.TableName = options.TableName.FirstCharUpper();
            options.NameSpace = options.NameSpace.FirstCharUpper();

            bool isView = IsTableAView(options.TableName);

            if (isView)
            {
                options.NameSpace = CorrectNamespaceForView(options.NameSpace);
            }

            string template = ReadResource(Structures.EnumLayer.Search, isView);
            string fixedTableName = string.Format("Search{0}", GetCustomNameIfSet(FixName(options.TableName, isView)));

            template = template.Replace("[FILENAME]", Path.GetFileName(options.FileName));
            template = template.Replace("[TABLENAME]", fixedTableName);
            template = template.Replace("[YEAR]", DateTime.Now.Year.ToString());
            template = template.Replace("[NAMESPACE]", options.NameSpace);

            List<Structures.Property> properties = GetProperties(options.ConnectionString, options.TableName);

            if (properties != null)
            {
                StringBuilder propertiesBuilder = new StringBuilder();
                StringBuilder constructorBuilder = new StringBuilder();

                constructorBuilder.AppendLine("        /// <summary>");
                constructorBuilder.AppendLine(string.Format("        /// Initializes a new instance of the {0} class.", fixedTableName));
                constructorBuilder.AppendLine("        /// </summary>");
                constructorBuilder.AppendLine(string.Format("        public {0}()", fixedTableName));
                constructorBuilder.AppendLine("        {");

                foreach (Structures.Property property in properties)
                {
                    propertiesBuilder.AppendLine("        /// <summary>");
                    propertiesBuilder.AppendLine(string.Format("        /// Gets or sets {0}.", property.CustomName.FirstCharUpper()));
                    propertiesBuilder.AppendLine("        /// </summary>");

                    propertiesBuilder.AppendLine(string.Format("        public {0} {1} {{ get; set; }}",
                                                               GetDotNetDataTypeForceNullable(property),
                                                               property.CustomName.FirstCharUpper()));

                    propertiesBuilder.AppendLine();

                    constructorBuilder.AppendLine(string.Format("            {0} = null;", property.CustomName.FirstCharUpper()));
                }

                constructorBuilder.AppendLine("        }");

                template = template.Replace("[PROPERTIES]", propertiesBuilder.ToString().RemoveLastInstanceOfWord(Environment.NewLine));
                template = template.Replace("[CONSTRUCTOR]", constructorBuilder.ToString().RemoveLastInstanceOfWord(Environment.NewLine));
            }

            bool saved = SaveContent(Structures.EnumLayer.Search, options.FileName, template);

            if (saved)
            {
                ShowMessageBox(this, Resources.ClassCreated, Resources.Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GenerateClass(Structures.SQLEntityGeneratorOptions options)
        {
            options.FileName = options.FileName.FirstCharUpper();
            options.TableName = options.TableName.FirstCharUpper();
            options.NameSpace = options.NameSpace.FirstCharUpper();

            bool isView = IsTableAView(options.TableName);

            if (isView)
            {
                options.NameSpace = CorrectNamespaceForView(options.NameSpace);
            }

            string template = ReadResource(Structures.EnumLayer.Entities, isView);
            string fixedTableName = GetCustomNameIfSet(FixName(options.TableName, isView));

            template = template.Replace("[FILENAME]", Path.GetFileName(options.FileName));
            template = template.Replace("[TABLENAME]", fixedTableName);
            template = template.Replace("[YEAR]", DateTime.Now.Year.ToString());
            template = template.Replace("[NAMESPACE]", options.NameSpace);

            List<Structures.Property> properties = GetProperties(options.ConnectionString, options.TableName);

            if (properties != null)
            {
                #region Properties String Builder
                StringBuilder propertiesBuilder = new StringBuilder();
                StringBuilder constructorBuilder = new StringBuilder();
                StringBuilder toStringBuilder = new StringBuilder();

                constructorBuilder.AppendLine("        /// <summary>");
                constructorBuilder.AppendLine(string.Format("        /// Initializes a new instance of the {0} class.", fixedTableName));
                constructorBuilder.AppendLine("        /// </summary>");
                constructorBuilder.AppendLine(string.Format("        public {0}()", fixedTableName));
                constructorBuilder.AppendLine("        {");

                #region Loop Through Properties
                foreach (Structures.Property property in properties)
                {
                    if (!isView)
                    {
                        propertiesBuilder.AppendLine(string.Format("        private {0} {1};", GetDotNetDataType(property), property.CustomName.FirstCharLower()));
                        propertiesBuilder.AppendLine();
                    }

                    propertiesBuilder.AppendLine("        /// <summary>");
                    propertiesBuilder.AppendLine(string.Format("        /// Gets or sets {0}.", property.CustomName.FirstCharUpper()));
                    propertiesBuilder.AppendLine("        /// </summary>");

                    if (!isView)
                    {
                        propertiesBuilder.AppendLine(string.Format("        [SqlName(\"{0}\")]", property.Name));
                    }

                    if (isView)
                    {
                        propertiesBuilder.AppendLine(string.Format("        public {0} {1} {{ get; set; }}", GetDotNetDataType(property), property.CustomName.FirstCharUpper()));
                    }
                    else
                    {
                        propertiesBuilder.AppendLine(string.Format("        public {0} {1}", GetDotNetDataType(property), property.CustomName.FirstCharUpper()));
                        propertiesBuilder.AppendLine("        {   ");
                        propertiesBuilder.AppendLine("            get ");
                        propertiesBuilder.AppendLine("            {");
                        propertiesBuilder.AppendLine(string.Format("                return {0};", property.CustomName.FirstCharLower()));
                        propertiesBuilder.AppendLine("            }");
                        propertiesBuilder.AppendLine("            set");
                        propertiesBuilder.AppendLine("            {");
                        propertiesBuilder.AppendLine(string.Format("                if (value != {0})", property.CustomName.FirstCharLower()));
                        propertiesBuilder.AppendLine("                {");
                        propertiesBuilder.AppendLine(string.Format("                    {0} = value;", property.CustomName.FirstCharLower()));
                        propertiesBuilder.AppendLine("                    IsItemModified = true;");
                        propertiesBuilder.AppendLine("                }");
                        propertiesBuilder.AppendLine("            }");
                        propertiesBuilder.AppendLine("        }");
                    }

                    constructorBuilder.AppendLine(string.Format("            {0} = default({1});", property.CustomName.FirstCharUpper(), GetDotNetDataType(property)));

                    propertiesBuilder.AppendLine();
                }
                #endregion

                if (!isView)
                {
                    constructorBuilder.AppendLine("            IsItemModified = false;");
                }

                constructorBuilder.AppendLine("        }");

                if (options.SelectedColumns.Any())
                {
                    string selectedColumnString = string.Empty;
                    int currentIndex = 0;
                    foreach (Structures.Property p in properties.Where(p => options.SelectedColumns.Select(c => c.Name).Contains(p.Name)))
                    {
                        selectedColumnString += string.Format("{0}: {1}{2}{3}, ", p.CustomName, "{", currentIndex, "}");
                        currentIndex++;
                    }

                    selectedColumnString = selectedColumnString.TrimSafely().RemoveLastInstanceOfComma().TrimSafely();

                    string selectedColumnWithOutParamsString = properties.Where(p => options.SelectedColumns.Select(c => c.Name).Contains(p.Name))
                                                                         .Aggregate(string.Empty,
                                                                                    (current, property) =>
                                                                                    string.Format("{0}{1}", current, string.Format("{0}, ", property.CustomName)));

                    selectedColumnWithOutParamsString = selectedColumnWithOutParamsString.TrimSafely().RemoveLastInstanceOfComma().TrimSafely();

                    toStringBuilder = new StringBuilder(string.Format("return string.Format(\"{0};\", {1});", selectedColumnString, selectedColumnWithOutParamsString));
                }
                else
                {
                    toStringBuilder.Append("// TODO Please add correct ToString()");
                    toStringBuilder.Append(Environment.NewLine);
                    toStringBuilder.Append("			return string.Empty;");
                }

                template = template.Replace("[PROPERTIES]", propertiesBuilder.ToString().RemoveLastInstanceOfWord(Environment.NewLine));
                template = template.Replace("[CONSTRUCTOR]", constructorBuilder.ToString().RemoveLastInstanceOfWord(Environment.NewLine));
                template = template.Replace("[TOSTRING]", toStringBuilder.ToString());
                #endregion
            }

            bool saved = SaveContent(Structures.EnumLayer.Entities, options.FileName, template);

            if (saved)
            {
                ShowMessageBox(this, Resources.ClassCreated, Resources.Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GenerateDAO(Structures.SQLEntityGeneratorOptions options)
        {
            options.FileName = options.FileName.FirstCharUpper();
            options.NameSpace = options.NameSpace.FirstCharUpper();

            bool isView = IsTableAView(options.TableName);

            if (isView)
            {
                options.NameSpace = CorrectNamespaceForView(options.NameSpace);
            }

            string entityNamespace = options.NameSpace.Replace("Data", "Entities");
            string template = ReadResource(Structures.EnumLayer.Data, isView);
            string fixedTableName = GetCustomNameIfSet(FixName(options.TableName, isView));
            string prefix = DetermineSPPrefix();
            string connectionVar = DetermineConnectionString(options.ConnectionString);

            template = template.Replace("[FILENAME]", Path.GetFileName(options.FileName));
            template = template.Replace("[TABLENAME]", fixedTableName);
            template = template.Replace("[REALTABLENAME]", options.TableName);
            template = template.Replace("[YEAR]", DateTime.Now.Year.ToString());
            template = template.Replace("[NAMESPACE]", options.NameSpace);
            template = template.Replace("[ENTITYNAMESPACE]", entityNamespace);
            template = template.Replace("[PREFIX]", prefix);
            template = template.Replace("[CONNECTIONVAR]", connectionVar);

            List<Structures.Property> properties = GetProperties(options.ConnectionString, options.TableName);

            StringBuilder primaryKeysParameters = new StringBuilder();
            StringBuilder primaryKeysDocumentation = new StringBuilder();
            StringBuilder primaryKeysSQLParams = new StringBuilder();
            StringBuilder linqParameters = new StringBuilder();
            StringBuilder nonIdentitySQLParams = new StringBuilder();
            StringBuilder allSQLParams = new StringBuilder();
            StringBuilder nonComputedSQLParams = new StringBuilder();

            if (properties != null)
            {
                #region Properties String Builder
                Structures.Property primaryKey = properties.FirstOrDefault(p => p.IsPrimaryKey);
                string mainPrimaryKey = primaryKey.HasCustomName ? primaryKey.CustomName : primaryKey.Name;

                #region Loop Through Properties
                foreach (Structures.Property property in properties)
                {
                    string propName = property.Name;
                    string customName = property.HasCustomName ? property.CustomName : propName;
                    string propType = property.Type;
                    string propTypeWithNullable = string.Format("{0}{1}",
                                                                propType,
                                                                property.Nullable && !propType.Equals("string")
                                                                    ? "?"
                                                                    : string.Empty);
                    string enumModifier = property.IsEnum
                                              ? property.Nullable
                                                    ? "(int?)"
                                                    : "(int)"
                                              : string.Empty;
                    string enumModifierNullable = property.IsEnum ? "(int?)" : string.Empty;
                    string trimSafely = propTypeWithNullable.Equals("string") ? ".TrimSafely()" : string.Empty;

                    if (property.IsPrimaryKey)
                    {
                        primaryKeysParameters.AppendFormat("{0} {1}, ", propType, customName.FirstCharLower());

                        primaryKeysDocumentation.AppendLine(string.Format(Resources.ParamDocString, customName.FirstCharLower()));

                        primaryKeysSQLParams.AppendLine(string.Format(Resources.PrimaryKeyParamString,
                                                                      propName,
                                                                      customName.FirstCharLower(),
                                                                      enumModifier));
                    }

                    if (!property.IsIdentity && !property.IsComputed)
                    {
                        nonIdentitySQLParams.AppendLine(string.Format(Resources.ParamString,
                                                                      propName.FirstCharUpper(),
                                                                      customName.FirstCharUpper(),
                                                                      enumModifier));
                    }

                    allSQLParams.AppendLine(string.Format(Resources.ParamString,
                                                          propName.FirstCharUpper(),
                                                          customName.FirstCharUpper(),
                                                          enumModifierNullable));

                    if (!property.IsComputed)
                    {
                        nonComputedSQLParams.AppendLine(string.Format(Resources.ParamString,
                                                                      propName.FirstCharUpper(),
                                                                      customName.FirstCharUpper(),
                                                                      enumModifier));
                    }

                    linqParameters.AppendLine(string.Format(Resources.LinqString,
                                                            customName.FirstCharUpper(),
                                                            propName.FirstCharUpper(),
                                                            propTypeWithNullable,
                                                            trimSafely));
                }
                #endregion

                template = template.Replace("[PRIMARYKEYSDOC]", primaryKeysDocumentation.ToString().RemoveLastInstanceOfWord(Environment.NewLine));
                template = template.Replace("[PRIMARYKEYS]", primaryKeysParameters.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[PRIMARYKEY]", mainPrimaryKey.FirstCharUpper());
                template = template.Replace("[SQLParamListDelete]", primaryKeysSQLParams.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[SqlParamListInsert]", nonIdentitySQLParams.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[SqlParamListSearch]", allSQLParams.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[SqlParamListUpdate]", nonComputedSQLParams.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[LinqParameters]", linqParameters.ToString().RemoveLastInstanceOfComma());
                #endregion
            }

            bool saved = SaveContent(Structures.EnumLayer.Data, options.FileName, template);

            if (saved)
            {
                ShowMessageBox(this, Resources.DAOCreated, Resources.Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GenerateManager(Structures.SQLEntityGeneratorOptions options)
        {
            options.FileName = options.FileName.FirstCharUpper();
            options.TableName = options.TableName.FirstCharUpper();
            options.NameSpace = options.NameSpace.FirstCharUpper();

            bool isView = IsTableAView(options.TableName);

            if (isView)
            {
                options.NameSpace = CorrectNamespaceForView(options.NameSpace);
            }

            string entityNamespace = options.NameSpace.Replace("Business", "Entities");
            string dataNamespace = options.NameSpace.Replace("Business", "Data");
            string template = ReadResource(Structures.EnumLayer.Business, isView);
            string fixedTableName = GetCustomNameIfSet(FixName(options.TableName, isView));

            template = template.Replace("[FILENAME]", Path.GetFileName(options.FileName));
            template = template.Replace("[TABLENAME]", fixedTableName);
            template = template.Replace("[TABLENAMELOWERFIRST]", fixedTableName.FirstCharLower());
            template = template.Replace("[YEAR]", DateTime.Now.Year.ToString(CultureInfo.InvariantCulture));
            template = template.Replace("[NAMESPACE]", options.NameSpace);
            template = template.Replace("[ENTITYNAMESPACE]", entityNamespace);
            template = template.Replace("[DATANAMESPACE]", dataNamespace);

            List<Structures.Property> properties = GetProperties(options.ConnectionString, options.TableName);

            StringBuilder propertiesPrimaryKeysComments = new StringBuilder();
            StringBuilder propertiesPrimaryKeys = new StringBuilder();
            StringBuilder propertiesPrimaryKeyParams = new StringBuilder();
            StringBuilder propertiesPrimaryKeySearch = new StringBuilder();
            StringBuilder propertiesValidation = new StringBuilder();

            if (properties != null)
            {
                foreach (Structures.Property property in properties)
                {
                    string customName = property.HasCustomName ? property.CustomName : property.Name;

                    if (property.IsPrimaryKey)
                    {
                        string space = string.Empty;
                        if (propertiesPrimaryKeysComments.Length > 0)
                        {
                            space = "        ";
                        }

                        propertiesPrimaryKeysComments.AppendLine(string.Format(Resources.PrimaryKeysDocumentation,
                                                                               space,
                                                                               customName.FirstCharLower(),
                                                                               options.TableName));

                        propertiesPrimaryKeySearch.AppendLine(string.Format("{0} = {1},",
                                                                            customName.FirstCharUpper(),
                                                                            customName.FirstCharLower()));

                        propertiesPrimaryKeys.AppendFormat("{0}, ", customName.FirstCharLower());

                        propertiesPrimaryKeyParams.AppendFormat("{0} {1}, ",
                                                                property.Type,
                                                                customName.FirstCharLower());
                    }
                    else
                    {
                        if (property.Type.Equals("DateTime") && !property.Nullable)
                        {
                            string validatorCheck = string.Format("			if (!item.{0}.IsValidWithSqlDateStandards())",
                                                                  customName.FirstCharUpper());

                            string validatorMessage
                                = customName.SafeEquals("CreatedDate") || customName.SafeEquals("CreatedDTTM")
                                      ? string.Format("				item.{0} = DateTime.Now;", customName.FirstCharUpper())
                                      : string.Format("				errorMessage += \"{0} must be valid.\";", customName.FirstCharUpper());

                            propertiesValidation.AppendLine(validatorCheck);
                            propertiesValidation.AppendLine("			{");
                            propertiesValidation.AppendLine(validatorMessage);
                            propertiesValidation.AppendLine("			}");
                            propertiesValidation.AppendLine();
                        }
                    }
                }

                template = template.Replace("[PRIMARYKEYPARAMCOMMENTS]", propertiesPrimaryKeysComments.ToString().RemoveLastInstanceOfWord(Environment.NewLine));
                template = template.Replace("[PRIMARYKEYS]", propertiesPrimaryKeys.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[PRIMARYKEYSEARCH]", propertiesPrimaryKeySearch.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[PRIMARYKEYPARAMS]", propertiesPrimaryKeyParams.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[VALIDATION]", propertiesValidation.ToString().RemoveLastInstanceOfWord(Environment.NewLine));
            }

            bool saved = SaveContent(Structures.EnumLayer.Business, options.FileName, template);

            if (saved)
            {
                ShowMessageBox(this, Resources.ManagerCreated, Resources.Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void GenerateTests(Structures.SQLEntityGeneratorOptions options)
        {
            options.FileName = options.FileName.FirstCharUpper();
            options.TableName = options.TableName.FirstCharUpper();
            options.NameSpace = options.NameSpace.FirstCharUpper();

            bool isView = IsTableAView(options.TableName);

            //Remove .NonPersistent from NameSpace for Tests
            options.NameSpace = options.NameSpace.Replace(Resources.NonPersistent, string.Empty);

            string entityNamespace = string.Format("{0}{1}", options.NameSpace.Replace("Tests", "Entities"), isView ? Resources.NonPersistent : string.Empty);
            string managerNamespace = string.Format("{0}{1}", options.NameSpace.Replace("Tests", "Business"), isView ? Resources.NonPersistent : string.Empty);
            string template = ReadResource(Structures.EnumLayer.Tests, isView);
            string fixedTableName = GetCustomNameIfSet(FixName(options.TableName, isView));

            template = template.Replace("[TABLENAME]", fixedTableName);
            template = template.Replace("[NAMESPACE]", options.NameSpace);
            template = template.Replace("[FILENAME]", Path.GetFileName(options.FileName));
            template = template.Replace("[YEAR]", DateTime.Now.Year.ToString());
            template = template.Replace("[ENTITYNAMESPACE]", entityNamespace);
            template = template.Replace("[MANAGERNAMESPACE]", managerNamespace);

            List<Structures.Property> properties = GetProperties(options.ConnectionString, options.TableName);

            StringBuilder propertiesPrimaryKeysInit = new StringBuilder();
            StringBuilder propertiesPrimaryKeys = new StringBuilder();

            if (properties != null)
            {
                foreach (Structures.Property property in properties.Where(p => p.IsPrimaryKey))
                {
                    propertiesPrimaryKeys.AppendFormat("{0}, ", property.Name.FirstCharLower());

                    string initProp = string.Format("const {0} {1} = {2}; ", property.Type, property.Name.FirstCharLower(), property.DefaultValue);

                    propertiesPrimaryKeysInit.AppendLine(initProp);
                }

                template = template.Replace("[PRIMARYKEYS]", propertiesPrimaryKeys.ToString().RemoveLastInstanceOfComma());
                template = template.Replace("[PRIMARYKEYSINIT]", propertiesPrimaryKeysInit.ToString());
            }

            bool saved = SaveContent(Structures.EnumLayer.Tests, options.FileName, template);

            if (saved)
            {
                ShowMessageBox(this, Resources.TestsCreated, Resources.Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        #region Events
        #region Events
        private static void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = ViewSchemaCheck(sender as BackgroundWorker, e.Argument);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            int progressPercentage = e.ProgressPercentage;
            Text = string.Format("{0} - View Schema Progress: {1}%", Resources.SQLEntityClassGeneratorTitle, progressPercentage);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result.ToString().IsNullOrWhiteSpace())
            {
                ShowMessageBox(this,
                               Resources.NoErrorsOnViewSchemaCheck,
                               Resources.NoErrorsOnViewSchemaCheck,
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information,
                               true);
            }
            else
            {
                ShowMessageBox(this,
                               string.Format(Resources.ViewSchemaCheckErrorsFound),
                               Resources.ErrorFound,
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error,
                               true);

                string fileName = string.Format("ViewSchemaErrors-{0}.txt", DateTime.Now.ToString("MMddyyyy-HHmmss"));

                if (DefaultSaveLocation.IsNullOrWhiteSpace())
                {
                    saveFileDialog1.FileName = fileName;
                    saveFileDialog1.Filter = Resources.TXTFiles;

                    if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
                    {
                        fileName = saveFileDialog1.FileName;
                    }
                }
                else
                {
                    fileName = Path.Combine(DefaultSaveLocation, fileName);
                }

                if (!fileName.IsNullOrWhiteSpace())
                {
                    SaveContent(null, fileName, e.Result.ToString());
                }
            }

            viewSchemaCheckToolStripMenuItem.Enabled = true;
            viewSchemaCheckToolStripMenuItem.ToolTipText = Resources.CheckViewSchemas;

            Text = Resources.SQLEntityClassGeneratorTitle;
        }

        private void txtConnStr_TextChanged(object sender, EventArgs e)
        {
            RefreshTables();
        }

        private void rdoCustom_CheckedChanged(object sender, EventArgs e)
        {
            txtCustomPrefix.Visible = rdoCustom.Checked;
        }

        private void chkUsePrefix_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkUsePrefix.Checked;

            if (!isChecked)
            {
                txtUsePrefix.Text = string.Empty;
            }

            txtUsePrefix.Visible = isChecked;
        }

        private void chkUseCustomName_CheckedChanged(object sender, EventArgs e)
        {
            bool isChecked = chkUseCustomName.Checked;

            if (!isChecked)
            {
                txtCustomName.Text = string.Empty;
            }

            txtCustomName.Visible = isChecked;
        }
        #endregion

        #region Button Events
        private void btnConnection_Click(object sender, EventArgs e)
        {
            ShowConnectionDialog();
        }

        private void btnRefreshTables_Click(object sender, EventArgs e)
        {
            RefreshTables();
        }

        private void btnGenerateSP_Click(object sender, EventArgs e)
        {
            GeneratingStoredProcedures();
        }

        private void btnEZGenerate_Click(object sender, EventArgs e)
        {
            GeneratingAll();
        }
        #endregion

        #region Toolbar Events
        #region Misc Toolbar
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void changelogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ChangelogForm form = new ChangelogForm())
            {
                form.ShowDialog(this);
            }
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PreferencesForm form = new PreferencesForm())
            {
                form.LoadFormFromPreferences(FormPreferences);
                if (form.ShowDialog(this) == DialogResult.OK)
                {
                    //Save preferences here
                    Structures.Preferences prefs = form.SQLGeneratorPreferences;
                    DisableMessageBoxes = prefs.DisableMessageBoxesPref;
                    DefaultSaveLocation = prefs.DefaultSaveLocationPref;
                    AutoIncludeFiles = prefs.AutoIncludeFilesPref;
                    LibraryLocation = prefs.LibraryLocationPref;
                    ModifyProjectFile = prefs.ModifyProjectFilePref;
                    DisableToStringPrompt = prefs.DisableToStringPromptPref;
                    DisableCustomPropertiesPrompt = prefs.DisableCustomPropertiesPromptPref;
                }
            }
        }

        private void viewSchemaCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string connectionString = txtConnStr.Text.TrimSafely();

            errorProvider1.Clear();

            if (connectionString.IsNullOrWhiteSpace())
            {
                errorProvider1.SetError(txtConnStr, Resources.ConnectionRequiredForViewSchemaCheck);

                ShowMessageBox(this,
                               Resources.ConnectionRequiredForViewSchemaCheck,
                               Resources.Error,
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Exclamation,
                               true);
                return;
            }

            viewSchemaCheckToolStripMenuItem.Enabled = false;
            viewSchemaCheckToolStripMenuItem.ToolTipText = Resources.CheckingViewSchemas;

            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            backgroundWorker1.ProgressChanged += backgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += backgroundWorker1_RunWorkerCompleted;
            backgroundWorker1.RunWorkerAsync(connectionString);
        }
        #endregion

        #region Connection Strings
        private void accountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtConnStr.Text = string.Format(Resources.ConnectionString, Resources.AccountsServerDev, Resources.AccountsDatabase);
        }

        #endregion

        #region Namespaces
        private void M3EntitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtNamespace.Text = Resources.EntitiesNamespace;
        }

        private void M3DataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtNamespace.Text = Resources.DataNamespace;
        }

        private void M3BusinessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtNamespace.Text = Resources.BusinessNamespace;
        }

        private void M3TestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtNamespace.Text = Resources.TestsNamespace;
        }

        private void GPSApplicationEntitiesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtNamespace.Text = string.Format(Resources.NamespaceString, Resources.EntitiesNamespace, Resources.GPSApplication);
        }

        private void GPSApplicationDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtNamespace.Text = string.Format(Resources.NamespaceString, Resources.DataNamespace, Resources.GPSApplication);
        }

        private void GPSApplicationBusinessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtNamespace.Text = string.Format(Resources.NamespaceString, Resources.BusinessNamespace, Resources.GPSApplication);
        }

        private void GPSApplicationTestsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtNamespace.Text = string.Format(Resources.NamespaceString, Resources.TestsNamespace, Resources.GPSApplication);
        }

        #endregion
        #endregion
        #endregion

        #region Helper Methods
        private void ShowConnectionDialog()
        {
            object connection = new Connection();
            object connectionObj = connection;

            var linkClass = new DataLinksClass();

            if (linkClass.PromptEdit(ref connectionObj))
            {
                var connString = (Connection)connection;
                DbConnectionStringBuilder builder
                    = new DbConnectionStringBuilder
                        {
                            ConnectionString = connString.ConnectionString
                        };
                builder.Remove("Provider");
                txtConnStr.Text = builder.ConnectionString;
            }
        }

        private void RefreshTables()
        {
            bool isValid = true;
            errorProvider1.Clear();

            if (txtConnStr.Text.IsNullOrWhiteSpace())
            {
                isValid = false;
                errorProvider1.SetError(lblConnStr, Resources.RequiredFieldError);
            }

            if (isValid)
            {
                string connectionString = txtConnStr.Text.TrimSafely();
                if (!connectionString.IsNullOrWhiteSpace())
                {
                    LoadTables(connectionString);
                }
            }
        }

        private string DetermineConnectionString(string connStr)
        {
            string connectionVar = "GPSAppsConnectionString";

            try
            {
                using (SqlConnection connection = new SqlConnection(connStr))
                {
                    switch (connection.Database.ToUpper())
                    {
                        case "ACCOUNTS":
                            connectionVar = "GPSAppsConnectionString";
                            break;
                    }
                }
            }
            catch (Exception)
            {
                ShowMessageBox(this,
                               Resources.ErrorDeterminingConnectionString,
                               Resources.Error,
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error,
                               true);
                connectionVar = "GPSAppsConnectionString";
            }

            return connectionVar;
        }

        public static List<Structures.Property> GetProperties(string connStr, string tableName)
        {
            List<Structures.Property> properties = new List<Structures.Property>();

            DataTable table
                = new DataTable
                    {
                        Locale = CultureInfo.CurrentCulture
                    };

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                string commandText = Resources.GetPropertiesQuery;

                using (SqlCommand command = new SqlCommand(string.Format(commandText, tableName), connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(table);
                    }

                    connection.Close();
                }
            }

            if (table.Rows != null)
            {
                // ReSharper disable LoopCanBePartlyConvertedToQuery
                foreach (DataRow row in table.Rows)
                {
                    Structures.Property property
                        = new Structures.Property
                            {
                                Name = (row["COLUMN_NAME"] as string).TrimSafely(),
                                Type = ConvertSQLType(row["DATA_TYPE"] as string).TrimSafely(),
                                Nullable = ConvertSQLNullable(row["IS_NULLABLE"] as string),
                                NumericPrecision = row["NUMERIC_PRECISION"] as byte?,
                                CharacterLength = row["CHARACTER_MAXIMUM_LENGTH"] as int?,
                                NumericScale = row["NUMERIC_SCALE"] as int?,
                                SqlDataType = (row["DATA_TYPE"] as string).TrimSafely(),
                                IsPrimaryKey = row["IsPrimaryKey"].ToString().Equals("1"),
                                IsIdentity = bool.Parse(row["IsIdentity"].ToString()),
                                IsComputed = bool.Parse(row["IsComputed"].ToString())
                            };

                    property.DefaultValue = GetDefaultValueByType(property.Type);

                    property.CustomName = property.Name;

                    string comparer;
                    switch (property.Type)
                    {
                        case "string":
                            comparer = "LIKE";
                            break;

                        case "DateTime":
                            comparer = "BETWEEN";
                            break;

                        default:
                            comparer = "=";
                            break;
                    }

                    property.Comparer = comparer;

                    DetermineCustomProperties(ref property);

                    properties.Add(property);
                }
                // ReSharper restore LoopCanBePartlyConvertedToQuery
            }

            return properties;
        }

        private static void DetermineCustomProperties(ref Structures.Property property)
        {
            if (Structures.CustomProperties.SelectedCustomProperties.SafeAny())
            {
                Structures.CustomProperty customProperty = Structures.CustomProperties.GetCustomProperty(property);
                if (customProperty != null)
                {
                    property.CustomName = customProperty.CustomName;
                    property.HasCustomName = true;
                    if (customProperty.IsEnumeration)
                    {
                        property.Type = customProperty.EnumerationType;
                        property.IsEnum = true;
                    }
                }
            }
        }

        private bool SaveContent(Structures.EnumLayer? layer, string fileName, string content)
        {
            if (AutoIncludeFiles && layer.HasValue && layer.Value != Structures.EnumLayer.StoredProcedure)
            {
                return SaveInLibrary(layer.Value, fileName, content);
            }

            if (rdoSaveToFile.Checked)
            {
                try
                {
                    using (FileStream stream = File.Open(fileName, FileMode.Create))
                    {
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            writer.Write(content);
                            writer.Flush();
                            return true;
                        }
                    }
                }
                catch (Exception)
                {
                    ShowMessageBox(this,
                                   Resources.ErrorSavingFile,
                                   Resources.GlitchInTheMatrix,
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                    return false;
                }
            }

            if (rdoSaveToClipboard.Checked)
            {
                try
                {
                    string currentClipboard = Clipboard.GetText();
                    Clipboard.SetText(string.Format("{0}{1}{2}", currentClipboard, Environment.NewLine, content));
                    return true;
                }
                catch (Exception)
                {
                    ShowMessageBox(this,
                                   Resources.ErrorCopyingContent,
                                   Resources.GlitchInTheMatrix,
                                   MessageBoxButtons.OK,
                                   MessageBoxIcon.Error);
                    return false;
                }
            }

            return false;
        }

        private bool SaveInLibrary(Structures.EnumLayer layer, string filePath, string content)
        {
            try
            {
                filePath = DetermineLibraryFilePath(layer, filePath);

                using (FileStream stream = File.Open(filePath, FileMode.Create))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.Write(content);
                        writer.Flush();
                    }
                }

                if (ModifyProjectFile)
                {
                    IncludeInProject(layer, filePath);
                }
            }
            catch (Exception)
            {
                ShowMessageBox(this,
                               Resources.ErrorSavingFile,
                               Resources.GlitchInTheMatrix,
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void IncludeInProject(Structures.EnumLayer layer, string filePath)
        {
            if (!LibraryLocation.IsNullOrWhiteSpace())
            {
                string layerString = GetLayerStringFromEnum(layer);
                string layerPath = Path.Combine(LibraryLocation, layerString);
                if (Directory.Exists(layerPath))
                {
                    string projectPath = string.Format(@"{0}\{1}.csproj", layerPath, layerString);
                    if (File.Exists(projectPath))
                    {
                        File.SetAttributes(projectPath, FileAttributes.Normal);

                        List<string> projectText = File.ReadAllLines(projectPath).ToList();

                        int index = 0;

                        foreach (string projectLine in projectText)
                        {
                            if (projectLine.Contains("Compile"))
                            {
                                string xmlFilePath = DetermineXMLInclude(filePath);
                                string compileString = string.Format("<Compile Include=\"{0}\" />", xmlFilePath);
                                projectText.Insert(index + 1, compileString);
                                break;
                            }

                            index++;
                        }

                        File.Delete(projectPath);
                        File.WriteAllLines(projectPath, projectText);
                    }
                }
            }
        }

        private string DetermineXMLInclude(string filePath)
        {
            string xmlIncludePath = filePath;

            if (!xmlIncludePath.IsNullOrWhiteSpace())
            {
                xmlIncludePath = string.Format(@"{0}\", GetApplicationStringFromEnum(SelectedApplication));

                if (SelectedTableIsView)
                {
                    xmlIncludePath += @"NonPersistent\";
                }

                xmlIncludePath += GetFileNameFromPath(filePath);
            }

            return xmlIncludePath;
        }

        private string DetermineLibraryFilePath(Structures.EnumLayer layer, string path)
        {
            string libraryFileName = path;

            if (!LibraryLocation.IsNullOrWhiteSpace())
            {
                string fileName = GetFileNameFromPath(path);
                if (!fileName.IsNullOrWhiteSpace())
                {
                    libraryFileName = Path.Combine(LibraryLocation, GetLayerStringFromEnum(layer));
                    libraryFileName = Path.Combine(libraryFileName, GetApplicationStringFromEnum(SelectedApplication));

                    if (SelectedTableIsView)
                    {
                        libraryFileName = Path.Combine(libraryFileName, "NonPersistent");
                    }

                    libraryFileName = Path.Combine(libraryFileName, fileName);
                }
            }

            return libraryFileName;
        }

        private static string GetLayerStringFromEnum(Structures.EnumLayer layer)
        {
            string layerString = string.Empty;

            switch (layer)
            {
                case Structures.EnumLayer.Entities:
                case Structures.EnumLayer.Search:
                    layerString = "M3.Entities";
                    break;

                case Structures.EnumLayer.Data:
                    layerString = "M3.Data";
                    break;

                case Structures.EnumLayer.Business:
                    layerString = "M3.Business";
                    break;

                case Structures.EnumLayer.Tests:
                    layerString = "M3.Tests";
                    break;
            }

            return layerString;
        }

        private string GetApplicationStringFromEnum(Structures.EnumApplicationID application)
        {
            string applicationString = string.Empty;

            switch (application)
            {
                case Structures.EnumApplicationID.GPSApplication:
                    applicationString = "GPSApplication";
                    break;
                case Structures.EnumApplicationID.Custom:
                    applicationString = txtCustomPrefix.Text.TrimSafely().Replace(".", string.Empty);
                    break;
            }

            return applicationString;
        }

        private static string GetFileNameFromPath(string path)
        {
            string fileName = path;

            if (!path.IsNullOrWhiteSpace() && path.Contains(@"\"))
            {
                string[] pathSplit = path.Split('\\');
                if (pathSplit.Length > 0)
                {
                    fileName = pathSplit.ElementAt(pathSplit.Length - 1);
                }
            }

            return fileName;
        }

        private void LoadTables(string connectionString)
        {
            List<Structures.Table> tables = new List<Structures.Table>();

            DataTable table = new DataTable { Locale = CultureInfo.CurrentCulture };

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand(Resources.GetTablesQuery, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(table);
                    }

                    connection.Close();
                }
            }

            if (table.Rows != null && table.Rows.Count > 0)
            {
                tables.AddRange(table.Rows.Cast<DataRow>().Select(t => new Structures.Table
                    {
                        Name = t["TABLE_NAME"].ToString().TrimSafely(),
                        IsView = t["TABLE_TYPE"].ToString().TrimSafely().Contains("VIEW")
                    }));
            }

            cmbTables.DataSource = tables;
            cmbTables.DisplayMember = Structures.Table.DisplayMember;
            cmbTables.ValueMember = Structures.Table.ValueMember;
            cmbTables.Enabled = tables.Count > 0;
            cmbTables.SelectedIndex = -1;
        }

        private static string GetDotNetDataType(Structures.Property property)
        {
            return string.Format("{0}{1}",
                                 property.Type,
                                 property.IsPrimaryKey ||
                                        property.Nullable &&
                                        !property.Type.Equals("string", StringComparison.InvariantCultureIgnoreCase) ? "?" : string.Empty).TrimSafely();
        }

        private static string GetDotNetDataTypeForceNullable(Structures.Property property)
        {
            return string.Format("{0}{1}",
                                 property.Type,
                                 !property.Type.Equals("string", StringComparison.InvariantCultureIgnoreCase) ? "?" : string.Empty).TrimSafely();
        }

        private void DetermineNamespace(Structures.EnumLayer layer)
        {
            string nameSpace = "M3";

            switch (layer)
            {
                case Structures.EnumLayer.Entities:
                    nameSpace += ".Entities";
                    break;

                case Structures.EnumLayer.Data:
                    nameSpace += ".Data";
                    break;

                case Structures.EnumLayer.Business:
                    nameSpace += ".Business";
                    break;

                case Structures.EnumLayer.Tests:
                    nameSpace += ".Tests";
                    break;
            }

            foreach (RadioButton rdoButton in pnlApplication.Controls.Cast<Control>()
                                                                     .Where(control => control is RadioButton && ((RadioButton)control).Checked))
            {
                switch ((Structures.EnumApplicationID)Convert.ToInt32(rdoButton.Tag))
                {
                    case Structures.EnumApplicationID.GPSApplication:
                        nameSpace += ".GPSApplication";
                        break;
                    case Structures.EnumApplicationID.Custom:
                        nameSpace += string.Format(".{0}", txtCustomPrefix.Text.TrimSafely().Replace(".", string.Empty));
                        break;
                }
            }

            txtNamespace.Text = nameSpace;
        }

        private bool ValidateForm(Structures.EnumLayer layer)
        {
            bool isValid = false;

            switch (layer)
            {
                case Structures.EnumLayer.All:
                    isValid = ValidateForm(new Structures.SQLEntityGeneratorValidation
                        {
                            RequiresNameSpace = false
                        });
                    break;

                case Structures.EnumLayer.StoredProcedure:
                    isValid = ValidateForm(new Structures.SQLEntityGeneratorValidation
                        {
                            RequiresCustomPrefix = false,
                            RequiresNameSpace = false
                        });
                    break;

                case Structures.EnumLayer.Search:
                case Structures.EnumLayer.Entities:
                case Structures.EnumLayer.Data:
                case Structures.EnumLayer.Business:
                case Structures.EnumLayer.Tests:
                    isValid = ValidateForm(new Structures.SQLEntityGeneratorValidation());
                    break;
            }

            return isValid;
        }

        private bool ValidateForm(Structures.SQLEntityGeneratorValidation validation)
        {
            bool isValid = true;

            errorProvider1.Clear();

            if (validation.RequiresConnectionString && txtConnStr.Text.IsNullOrWhiteSpace())
            {
                errorProvider1.SetError(lblConnStr, Resources.RequiredFieldError);
                isValid = false;
            }

            if (validation.RequiresTableName)
            {
                if (SelectedTable.IsNullOrWhiteSpace())
                {
                    errorProvider1.SetError(lblTables, Resources.RequiredFieldError);
                    isValid = false;
                }
                else if (!txtConnStr.Text.IsNullOrWhiteSpace())
                {
                    string tableValidCommand = string.Format(Resources.IsTableValid, SelectedTable);

                    DataTable table = new DataTable { Locale = CultureInfo.CurrentCulture };

                    using (SqlConnection connection = new SqlConnection(txtConnStr.Text))
                    {
                        using (SqlCommand command = new SqlCommand(tableValidCommand, connection))
                        {
                            command.CommandType = CommandType.Text;
                            connection.Open();

                            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                            {
                                adapter.Fill(table);
                            }

                            connection.Close();
                        }
                    }

                    if (!table.Rows.Cast<DataRow>().Any())
                    {
                        errorProvider1.SetError(lblTables, Resources.TableMustBeValid);
                        isValid = false;
                    }
                }
            }

            if (validation.RequiresNameSpace && txtNamespace.Text.IsNullOrWhiteSpace())
            {
                errorProvider1.SetError(lblNameSpace, Resources.RequiredFieldError);
                isValid = false;
            }

            if (validation.RequiresCustomPrefix && rdoCustom.Checked && txtCustomPrefix.Text.IsNullOrWhiteSpace())
            {
                errorProvider1.SetError(txtCustomPrefix, Resources.RequiredFieldError);
                isValid = false;
            }

            return isValid;
        }

        private static string ConvertToSQLDataType(Structures.Property value)
        {
            string returnValue = string.Format(" {0}", value.SqlDataType);

            if (value.NumericScale.HasValue && value.NumericScale.Value == 0 ||
                value.Type.Contains("Date") ||
                value.SqlDataType.Contains("bit") ||
                value.SqlDataType.Contains("money") ||
                value.SqlDataType.Contains("text"))
            {
                return string.Format(" {0} ", value.SqlDataType);
            }

            if (value.SqlDataType.Contains("float"))
            {
                return string.Format(" {0} ", value.SqlDataType);
            }

            return value.CharacterLength.HasValue
                       ? (value.CharacterLength == -1
                            ? string.Format(" {0}(MAX)", value.SqlDataType)
                            : string.Format(" {0}({1})", value.SqlDataType, value.CharacterLength))
                       : (value.NumericPrecision.HasValue
                            ? string.Format(" {0}({1}, {2})", value.SqlDataType, value.NumericPrecision, value.NumericScale)
                            : returnValue);
        }

        private static bool ConvertSQLNullable(string type)
        {
            return type == "YES";
        }

        private static string ConvertSQLType(string type)
        {
            string clrType = string.Empty;

            if (!type.IsNullOrWhiteSpace())
            {
                switch (type.ToLower())
                {
                    case "bigint":
                        clrType = "Int64";
                        break;

                    case "binary":
                    case "varbinary":
                        clrType = "byte[]";
                        break;

                    case "bit":
                        clrType = "bool";
                        break;

                    case "char":
                    case "nchar":
                    case "text":
                    case "varchar":
                    case "nvarchar":
                    case "ntext":
                        clrType = "string";
                        break;

                    case "date":
                    case "datetime":
                    case "datetime2":
                    case "time":
                        clrType = "DateTime";
                        break;

                    case "tinyint":
                    case "int":
                    case "smallint":
                        clrType = "int";
                        break;

                    case "money":
                    case "numeric":
                    case "float":
                    case "decimal":
                        clrType = "decimal";
                        break;

                    case "uniqueidentifier":
                        clrType = "Guid";
                        break;

                    default:
                        clrType = "object";
                        break;
                }
            }

            return clrType;
        }

        private static object GetDefaultValueByType(string type)
        {
            object retValue;

            switch (type)
            {
                case "Int64":
                    retValue = default(Int64);
                    break;

                case "byte[]":
                    retValue = default(byte[]);
                    break;

                case "bool":
                    retValue = default(bool);
                    break;

                case "string":
                    retValue = default(string);
                    break;

                case "DateTime":
                    retValue = default(DateTime);
                    break;

                case "int":
                    retValue = default(int);
                    break;

                case "decimal":
                    retValue = default(decimal);
                    break;

                case "Guid":
                    retValue = default(Guid);
                    break;

                default:
                    retValue = default(object);
                    break;
            }

            return retValue;
        }

        private string DetermineSPPrefix()
        {
            return txtUsePrefix.Text;
        }

        private string ReadResource(Structures.EnumLayer layer, bool isView)
        {
            string resource = string.Empty;

            switch (layer)
            {
                case Structures.EnumLayer.StoredProcedure:
                    resource = ReadEmbeddedResource(isView ? Resources.ViewSPTemplate : Resources.SPTemplate);
                    break;

                case Structures.EnumLayer.Search:
                    resource = ReadEmbeddedResource(isView ? Resources.ViewSearchTemplate : Resources.SearchTemplate);
                    break;

                case Structures.EnumLayer.Entities:
                    resource = ReadEmbeddedResource(isView ? Resources.ViewClassTemplate : Resources.ClassTemplate);
                    break;

                case Structures.EnumLayer.Data:
                    resource = ReadEmbeddedResource(isView ? Resources.ViewDAOTemplate : Resources.DAOTemplate);
                    break;

                case Structures.EnumLayer.Business:
                    resource = ReadEmbeddedResource(isView ? Resources.ViewManagerTemplate : Resources.ManagerTemplate);
                    break;

                case Structures.EnumLayer.Tests:
                    resource = ReadEmbeddedResource(isView ? Resources.ViewTestsTemplate : Resources.TestsTemplate);
                    break;
            }

            return resource;
        }

        private string ReadEmbeddedResource(string templateName)
        {
            string embeddedResource = string.Empty;

            try
            {
                Assembly assembly = Assembly.GetExecutingAssembly();

                if (!string.IsNullOrEmpty(templateName))
                {
                    var stream = assembly.GetManifestResourceStream(templateName);
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            embeddedResource = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (Exception)
            {
                ShowMessageBox(this, Resources.ErrorReadingEmbeddedResource, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error, true);
            }

            return embeddedResource;
        }

        private static void ShowMessageBox(IWin32Window owner, string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon, bool overrideDisableMsgBox = false)
        {
            if (overrideDisableMsgBox || !DisableMessageBoxes)
            {
                MessageBox.Show(owner, message, title, buttons, icon);
            }
        }

        private bool IsTableAView(string tableName)
        {
            string viewCheckCommand = string.Format(Resources.IsTableAViewQuery, tableName);

            DataTable table = new DataTable { Locale = CultureInfo.CurrentCulture };

            using (SqlConnection connection = new SqlConnection(txtConnStr.Text))
            {
                using (SqlCommand command = new SqlCommand(viewCheckCommand, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(table);
                    }

                    connection.Close();
                }
            }

            return table.Rows.Cast<DataRow>().Any();
        }

        private static string CorrectNamespaceForView(string nameSpace)
        {
            if (!nameSpace.IsNullOrWhiteSpace())
            {
                //Ensure that we have only 1 instance of ".NonPersistent"
                nameSpace = nameSpace.Replace(Resources.NonPersistent, string.Empty);
                nameSpace += Resources.NonPersistent;
            }

            return nameSpace;
        }

        private static string FixName(string tableName, bool isView)
        {
            if (!tableName.IsNullOrWhiteSpace())
            {
                if (isView)
                {
                    tableName = FixNameForView(tableName);
                }
                else if (tableName.StartsWith("LKP"))
                {
                    tableName = FixNameForLKP(tableName);
                }
            }

            return tableName;
        }

        private static string FixNameForLKP(string tableName)
        {
            if (!tableName.IsNullOrWhiteSpace() && tableName.StartsWith("LKP"))
            {
                tableName = tableName.Replace("LKP", string.Empty);
            }

            return tableName;
        }

        private static string FixNameForView(string viewName)
        {
            if (!viewName.IsNullOrWhiteSpace())
            {
                int indexOfUnderscore = viewName.IndexOf('_');
                if (indexOfUnderscore > -1)
                {
                    viewName = viewName.Remove(0, viewName.IndexOf('_') + 1);
                    viewName += "View";
                }
            }

            return viewName;
        }

        private static string ViewSchemaCheck(BackgroundWorker backgroundWorker, object connectionString)
        {
            DataSet dataSet
                = new DataSet
                    {
                        Locale = CultureInfo.CurrentCulture,
                        EnforceConstraints = false
                    };

            DataTable table
                = new DataTable
                    {
                        Locale = CultureInfo.CurrentCulture
                    };

            using (SqlConnection connection = new SqlConnection(connectionString.ToString()))
            {
                using (SqlCommand command = new SqlCommand(Resources.ViewSchemaQuery, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(table);
                        dataSet.Tables.Add(table);
                    }

                    connection.Close();
                }
            }

            StringBuilder errorMessage = new StringBuilder();

            if (dataSet.HasAtLeastOneRow())
            {
                IEnumerable<DataRow> dataRows = dataSet.GetRowsFromDataSet();

                int count = dataRows.Count();
                int counter = 1;

                foreach (string viewName in dataRows.Select(dr => dr.GetValue<string>("Table_Name")))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString.ToString()))
                    {
                        string commandText = string.Format(Resources.ViewSchemaCheckQuery, viewName);
                        SqlCommand command = new SqlCommand(commandText, connection) { CommandType = CommandType.Text };
                        connection.Open();

                        try
                        {
                            command.ExecuteReader();
                        }
                        catch (Exception ex)
                        {
                            errorMessage.AppendLine(string.Format("View: {0}{2}Error(s):{2}{1}{2}",
                                                                  viewName,
                                                                  ex.Message,
                                                                  Environment.NewLine));
                        }

                        connection.Close();
                    }

                    int percentageDone = counter * 100 / count;

                    backgroundWorker.ReportProgress(percentageDone);
                    counter++;
                }
            }

            return errorMessage.ToString();
        }

        private string GetCustomNameIfSet(string alternateName)
        {
            return UsesCustomName ? CustomName : alternateName;
        }

        private static void RefreshEnumList()
        {
            if (!LibraryLocation.IsNullOrWhiteSpace())
            {
                string enumerationsFilePath = Path.Combine(LibraryLocation, Resources.EnumerationsFileLocation);
                if (File.Exists(enumerationsFilePath))
                {
                    FileInfo enumerationsFile = new FileInfo(enumerationsFilePath);
                    string enumerationsContent = enumerationsFile.OpenText().ReadToEnd();

                    string[] enumerationLines = enumerationsContent.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                    List<string> enumerations
                        = (enumerationLines.Where(enumerationLine => enumerationLine.Contains("public enum"))
                                           .Select(enumerationLine =>
                                                   enumerationLine.RemoveStrings("public enum").TrimSafely())).ToList();

                    Enumerations = enumerations;
                }
            }
        }
        #endregion

        

    }
}