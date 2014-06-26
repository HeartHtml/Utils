namespace MultiScript.Forms
{
    partial class FoldersToRunForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FoldersToRunForm));
            this.cblSelectedFolders = new System.Windows.Forms.CheckedListBox();
            this.btnRemoveSelectedFolders = new System.Windows.Forms.Button();
            this.btnCloseForm = new System.Windows.Forms.Button();
            this.btnRemoveAllFolders = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cblSelectedFolders
            // 
            this.cblSelectedFolders.CheckOnClick = true;
            this.cblSelectedFolders.FormattingEnabled = true;
            this.cblSelectedFolders.IntegralHeight = false;
            this.cblSelectedFolders.Location = new System.Drawing.Point(12, 15);
            this.cblSelectedFolders.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cblSelectedFolders.Name = "cblSelectedFolders";
            this.cblSelectedFolders.Size = new System.Drawing.Size(1160, 340);
            this.cblSelectedFolders.TabIndex = 0;
            this.cblSelectedFolders.ThreeDCheckBoxes = true;
            // 
            // btnRemoveSelectedFolders
            // 
            this.btnRemoveSelectedFolders.Location = new System.Drawing.Point(532, 386);
            this.btnRemoveSelectedFolders.Name = "btnRemoveSelectedFolders";
            this.btnRemoveSelectedFolders.Size = new System.Drawing.Size(182, 23);
            this.btnRemoveSelectedFolders.TabIndex = 1;
            this.btnRemoveSelectedFolders.Text = "Remove Selected Folders";
            this.btnRemoveSelectedFolders.UseVisualStyleBackColor = true;
            this.btnRemoveSelectedFolders.Click += new System.EventHandler(this.btnRemoveSelectedFolders_Click);
            // 
            // btnCloseForm
            // 
            this.btnCloseForm.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCloseForm.Location = new System.Drawing.Point(763, 386);
            this.btnCloseForm.Name = "btnCloseForm";
            this.btnCloseForm.Size = new System.Drawing.Size(75, 23);
            this.btnCloseForm.TabIndex = 2;
            this.btnCloseForm.Text = "Close";
            this.btnCloseForm.UseVisualStyleBackColor = true;
            this.btnCloseForm.Click += new System.EventHandler(this.btnCloseForm_Click);
            // 
            // btnRemoveAllFolders
            // 
            this.btnRemoveAllFolders.Location = new System.Drawing.Point(347, 386);
            this.btnRemoveAllFolders.Name = "btnRemoveAllFolders";
            this.btnRemoveAllFolders.Size = new System.Drawing.Size(136, 23);
            this.btnRemoveAllFolders.TabIndex = 3;
            this.btnRemoveAllFolders.Text = "Remove All Folders";
            this.btnRemoveAllFolders.UseVisualStyleBackColor = true;
            this.btnRemoveAllFolders.Click += new System.EventHandler(this.btnRemoveAllFolders_Click);
            // 
            // FoldersToRunForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCloseForm;
            this.ClientSize = new System.Drawing.Size(1184, 462);
            this.Controls.Add(this.btnRemoveAllFolders);
            this.Controls.Add(this.btnCloseForm);
            this.Controls.Add(this.btnRemoveSelectedFolders);
            this.Controls.Add(this.cblSelectedFolders);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1200, 500);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1200, 500);
            this.Name = "FoldersToRunForm";
            this.Text = "Folders To Run MultiScript On";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckedListBox cblSelectedFolders;
        private System.Windows.Forms.Button btnRemoveSelectedFolders;
        private System.Windows.Forms.Button btnCloseForm;
        private System.Windows.Forms.Button btnRemoveAllFolders;
    }
}