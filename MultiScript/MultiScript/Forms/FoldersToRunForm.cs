using System;
using System.Windows.Forms;
using MultiScriptLib;

namespace MultiScript.Forms
{
    public partial class FoldersToRunForm : Form
    {
        public FoldersToRunForm()
        {
            InitializeComponent();
            LoadSelectedFolders();
        }

        private void LoadSelectedFolders()
        {
            cblSelectedFolders.Items.Clear();

            foreach (FolderToRun selectedFolder in FoldersToRun.SelectedFolders)
            {
                cblSelectedFolders.Items.Add(selectedFolder);
            }
        }

        private void btnRemoveAllFolders_Click(object sender, EventArgs e)
        {
            FoldersToRun.ClearFolders();
            LoadSelectedFolders();
        }

        private void btnRemoveSelectedFolders_Click(object sender, EventArgs e)
        {
            foreach (FolderToRun selectedFolder in cblSelectedFolders.CheckedItems)
            {
                FoldersToRun.RemoveFolder(selectedFolder);
            }

            LoadSelectedFolders();
        }

        private void btnCloseForm_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
