namespace BoostYourBIMTerrificTools.SubcategoryMerge
{
    partial class FormSubcatMerge
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
            this.lstCategory = new System.Windows.Forms.ListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.lstSubcat = new System.Windows.Forms.ListBox();
            this.txtNewCatName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lstCategory
            // 
            this.lstCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstCategory.FormattingEnabled = true;
            this.lstCategory.ItemHeight = 25;
            this.lstCategory.Location = new System.Drawing.Point(12, 12);
            this.lstCategory.Name = "lstCategory";
            this.lstCategory.Size = new System.Drawing.Size(354, 554);
            this.lstCategory.Sorted = true;
            this.lstCategory.TabIndex = 9;
            this.lstCategory.SelectedIndexChanged += new System.EventHandler(this.lstCategory_SelectedIndexChanged);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(934, 581);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 44);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(772, 581);
            this.btnOk.Margin = new System.Windows.Forms.Padding(6);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(150, 44);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // lstSubcat
            // 
            this.lstSubcat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstSubcat.FormattingEnabled = true;
            this.lstSubcat.ItemHeight = 25;
            this.lstSubcat.Location = new System.Drawing.Point(372, 12);
            this.lstSubcat.Name = "lstSubcat";
            this.lstSubcat.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple;
            this.lstSubcat.Size = new System.Drawing.Size(373, 554);
            this.lstSubcat.Sorted = true;
            this.lstSubcat.TabIndex = 9;
            // 
            // txtNewCatName
            // 
            this.txtNewCatName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtNewCatName.Location = new System.Drawing.Point(754, 51);
            this.txtNewCatName.Name = "txtNewCatName";
            this.txtNewCatName.Size = new System.Drawing.Size(333, 31);
            this.txtNewCatName.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(751, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(243, 25);
            this.label1.TabIndex = 11;
            this.label1.Text = "New Subcategory Name";
            // 
            // FormSubcatMerge
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1099, 640);
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtNewCatName);
            this.Controls.Add(this.lstSubcat);
            this.Controls.Add(this.lstCategory);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Name = "FormSubcatMerge";
            this.Text = "FormSubcatMerge";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstCategory;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.ListBox lstSubcat;
        private System.Windows.Forms.TextBox txtNewCatName;
        private System.Windows.Forms.Label label1;
    }
}