using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using Path = System.IO.Path;

namespace LibraryTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        #region Properties

        public string ClassLibraryFilePath
        {
            get
            {
                return TxtSelectedDll.Text;
            }
            set
            {
                TxtSelectedDll.Text = value;
            }
        }

        public string TranslatedClassLibraryFilePath { get; set; }

        public string ConfigurationFilePath
        {
            get
            {
                return TxtSelectedConfigFile.Text;
            }
            set
            {
                TxtSelectedConfigFile.Text = value;
            }
        }

        public List<string> AssembliesCopied
        {
            get
            {
                var currentApp = Application.Current as App;

                if (currentApp != null)
                {
                    return currentApp.AssembliesCopied;
                }
                return null;
            }
            set
            {
                List<string> assembliesCopied = value;

                var currentApp = Application.Current as App;

                if (currentApp != null)
                {
                    currentApp.AssembliesCopied = assembliesCopied;
                }
            }
        }

        public Assembly ClassLibraryToAnalyze { get; set; }

        #endregion

        #region PublicMethods

        public bool AnalyzeClassLibrary(out string errorMessage)
        {
            errorMessage = string.Empty;

            FileInfo executingAssemblyInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);

            string executingAssemblyDirectory = executingAssemblyInfo.DirectoryName;

            try
            {
                Assembly dll = Assembly.ReflectionOnlyLoadFrom(ClassLibraryFilePath);

                TranslatedClassLibraryFilePath = Path.Combine(executingAssemblyInfo.DirectoryName,
                                                             string.Format("{0}.dll", dll.GetName().Name));

                File.Copy(ClassLibraryFilePath, TranslatedClassLibraryFilePath, true);

                AssembliesCopied.Add(TranslatedClassLibraryFilePath);

                AssemblyName[] referencedAssemblyNames = dll.GetReferencedAssemblies();

                foreach (AssemblyName assemblyName in referencedAssemblyNames)
                {
                    try
                    {
                        //This will only work for assemblies in the GAC
                        Assembly.Load(assemblyName.FullName);
                    }
                    catch (Exception)
                    {
                        try
                        {
                            //Try in the bin of the ClassLibraryFilePath
                            FileInfo classLibraryFileInfo = new FileInfo(ClassLibraryFilePath);

                            if (!string.IsNullOrWhiteSpace(classLibraryFileInfo.DirectoryName))
                            {
                                FileInfo dependentAssemblyPath = new FileInfo(Path.Combine(classLibraryFileInfo.DirectoryName, string.Format("{0}.dll", assemblyName.Name)));

                                string newDependencyFileName = Path.Combine(executingAssemblyDirectory, string.Format("{0}.dll", assemblyName.Name));

                                File.Copy(dependentAssemblyPath.FullName, newDependencyFileName, true);

                                Assembly.LoadFile(newDependencyFileName);

                                AssembliesCopied.Add(newDependencyFileName);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }


                Assembly[] beforeLoadAssemblies = AppDomain.CurrentDomain.GetAssemblies();

                ClassLibraryToAnalyze = Assembly.LoadFile(TranslatedClassLibraryFilePath);

                AppDomain.CurrentDomain.AssemblyResolve += ResolveEventHandler;

                GroupBoxAssembly.Header = string.Format("Viewing Types In: {0}", ClassLibraryToAnalyze.FullName);

                Assembly[] afterLoadAssemblies = AppDomain.CurrentDomain.GetAssemblies();
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }

            return string.IsNullOrWhiteSpace(errorMessage);
        }

        private static Assembly ResolveEventHandler(object sender, ResolveEventArgs args)
        {
            FileInfo executingAssemblyInfo = new FileInfo(Assembly.GetExecutingAssembly().Location);

            string executingAssemblyDirectory = executingAssemblyInfo.DirectoryName;

            foreach (string file in Directory.GetFiles(executingAssemblyDirectory, "*.dll"))
            {
                FileInfo info = new FileInfo(file);

                string fileWithoutExtension = info.Name.Replace(info.Extension, string.Empty);

                string[] assemblyPieces = args.Name.Split(',');

                string assemblySimpleName = assemblyPieces[0];

                if (fileWithoutExtension.Contains(assemblySimpleName))
                {
                    Assembly assembly = Assembly.LoadFile(file);

                    return assembly;
                }
            }

            return null;
        }

        public void ResetWindow()
        {
            ClassLibraryFilePath = string.Empty;

            ConfigurationFilePath = string.Empty;

            TreeViewClassLibraryMethods.Items.Clear();
        }

        public MainWindow()
        {
            InitializeComponent();

            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            AssembliesCopied = new List<string>();

            GroupBoxAssembly.Header = "Please select an assembly to analyze";
        }

        public bool ValidateControl(out string errorMessage)
        {
            StringBuilder builder = new StringBuilder();

            if (string.IsNullOrWhiteSpace(ClassLibraryFilePath))
            {
                builder.AppendLine("*No class library selected to analyze");
            }

            errorMessage = builder.ToString();

            return string.IsNullOrWhiteSpace(errorMessage);
        }

        #endregion

        #region Events

        private void BtnBrowseDll_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog { DefaultExt = ".dll", Filter = "Class Libraries (.dll)|*.dll" };

            bool? result = dialog.ShowDialog();

            if (result.GetValueOrDefault())
            {
                string fileName = dialog.FileName;

                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    TxtSelectedDll.Text = fileName;
                }
            }
        }

        private void BtnBrowseConfigFile_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog { DefaultExt = ".config", Filter = "Configuration Files (.config)|*.config" };

            bool? result = dialog.ShowDialog();

            if (result.GetValueOrDefault())
            {
                string fileName = dialog.FileName;

                if (!string.IsNullOrWhiteSpace(fileName))
                {
                    TxtSelectedConfigFile.Text = fileName;
                }
            }
        }

        private void MenuItemExit_OnClick(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown(0);
        }

        private void MenuItemNew_OnClick(object sender, RoutedEventArgs e)
        {
            ResetWindow();
        }

        private void BtnAnalyze_OnClick(object sender, RoutedEventArgs e)
        {
            string errorMessage;

            if (ValidateControl(out errorMessage))
            {
                if (!AnalyzeClassLibrary(out errorMessage))
                {
                    MessageBox.Show(errorMessage, "Something went wrong!");
                }
                else
                {
                    TreeViewClassLibraryMethods.ItemsSource = ClassLibraryToAnalyze.GetTypes();
                }
            }
            else
            {
                MessageBox.Show(errorMessage, "Please correct the following errors");
            }
        }

        #endregion


    }
}
