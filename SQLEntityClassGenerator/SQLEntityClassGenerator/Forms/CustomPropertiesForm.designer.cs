namespace SQLEntityClassGenerator.Forms
{
    partial class CustomPropertiesForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CustomPropertiesForm));
            this.btnOK = new System.Windows.Forms.Button();
            this.clbCustomProperties = new System.Windows.Forms.CheckedListBox();
            this.ddlProperties = new System.Windows.Forms.ComboBox();
            this.txtPropertyName = new System.Windows.Forms.TextBox();
            this.ddlEnumeration = new System.Windows.Forms.ComboBox();
            this.lblProperty = new System.Windows.Forms.Label();
            this.lblEnumeration = new System.Windows.Forms.Label();
            this.lblPropertyName = new System.Windows.Forms.Label();
            this.btnAddSelection = new System.Windows.Forms.Button();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(516, 338);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(134, 28);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "Commit Selections";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // clbCustomProperties
            // 
            this.clbCustomProperties.FormattingEnabled = true;
            this.clbCustomProperties.Location = new System.Drawing.Point(15, 16);
            this.clbCustomProperties.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.clbCustomProperties.Name = "clbCustomProperties";
            this.clbCustomProperties.Size = new System.Drawing.Size(651, 196);
            this.clbCustomProperties.TabIndex = 1;
            this.clbCustomProperties.ThreeDCheckBoxes = true;
            // 
            // ddlProperties
            // 
            this.ddlProperties.FormattingEnabled = true;
            this.ddlProperties.Location = new System.Drawing.Point(173, 234);
            this.ddlProperties.Name = "ddlProperties";
            this.ddlProperties.Size = new System.Drawing.Size(262, 24);
            this.ddlProperties.TabIndex = 2;
            this.ddlProperties.SelectedIndexChanged += new System.EventHandler(this.ddlProperties_SelectedIndexChanged);
            // 
            // txtPropertyName
            // 
            this.txtPropertyName.Location = new System.Drawing.Point(173, 300);
            this.txtPropertyName.Name = "txtPropertyName";
            this.txtPropertyName.Size = new System.Drawing.Size(262, 21);
            this.txtPropertyName.TabIndex = 4;
            // 
            // ddlEnumeration
            // 
            this.ddlEnumeration.FormattingEnabled = true;
            this.ddlEnumeration.Location = new System.Drawing.Point(173, 266);
            this.ddlEnumeration.Name = "ddlEnumeration";
            this.ddlEnumeration.Size = new System.Drawing.Size(262, 24);
            this.ddlEnumeration.TabIndex = 3;
            // 
            // lblProperty
            // 
            this.lblProperty.AutoSize = true;
            this.lblProperty.Location = new System.Drawing.Point(15, 238);
            this.lblProperty.Name = "lblProperty";
            this.lblProperty.Size = new System.Drawing.Size(53, 16);
            this.lblProperty.TabIndex = 5;
            this.lblProperty.Text = "Property";
            // 
            // lblEnumeration
            // 
            this.lblEnumeration.AutoSize = true;
            this.lblEnumeration.Location = new System.Drawing.Point(15, 270);
            this.lblEnumeration.Name = "lblEnumeration";
            this.lblEnumeration.Size = new System.Drawing.Size(76, 16);
            this.lblEnumeration.TabIndex = 6;
            this.lblEnumeration.Text = "Enumeration";
            // 
            // lblPropertyName
            // 
            this.lblPropertyName.AutoSize = true;
            this.lblPropertyName.Location = new System.Drawing.Point(15, 302);
            this.lblPropertyName.Name = "lblPropertyName";
            this.lblPropertyName.Size = new System.Drawing.Size(89, 16);
            this.lblPropertyName.TabIndex = 7;
            this.lblPropertyName.Text = "Property Name";
            // 
            // btnAddSelection
            // 
            this.btnAddSelection.Location = new System.Drawing.Point(15, 341);
            this.btnAddSelection.Name = "btnAddSelection";
            this.btnAddSelection.Size = new System.Drawing.Size(224, 23);
            this.btnAddSelection.TabIndex = 5;
            this.btnAddSelection.Text = "Add Property Override";
            this.btnAddSelection.UseVisualStyleBackColor = true;
            this.btnAddSelection.Click += new System.EventHandler(this.btnAddSelection_Click);
            // 
            // errorProvider
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // CustomPropertiesForm
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(681, 384);
            this.Controls.Add(this.btnAddSelection);
            this.Controls.Add(this.lblPropertyName);
            this.Controls.Add(this.lblEnumeration);
            this.Controls.Add(this.lblProperty);
            this.Controls.Add(this.ddlEnumeration);
            this.Controls.Add(this.txtPropertyName);
            this.Controls.Add(this.ddlProperties);
            this.Controls.Add(this.clbCustomProperties);
            this.Controls.Add(this.btnOK);
            this.Font = new System.Drawing.Font("Century Gothic", 8.25F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximumSize = new System.Drawing.Size(697, 422);
            this.MinimumSize = new System.Drawing.Size(697, 422);
            this.Name = "CustomPropertiesForm";
            this.Text = "Custom Property Override";
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.CheckedListBox clbCustomProperties;
        private System.Windows.Forms.ComboBox ddlProperties;
        private System.Windows.Forms.TextBox txtPropertyName;
        private System.Windows.Forms.ComboBox ddlEnumeration;
        private System.Windows.Forms.Label lblProperty;
        private System.Windows.Forms.Label lblEnumeration;
        private System.Windows.Forms.Label lblPropertyName;
        private System.Windows.Forms.Button btnAddSelection;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}