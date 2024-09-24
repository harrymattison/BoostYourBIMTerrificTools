
namespace BoostYourBIMTerrificTools.SelectByType
{
    partial class FormSelectByType
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
            this.lstCat = new System.Windows.Forms.ListBox();
            this.lstTypes = new System.Windows.Forms.ListBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNumber = new System.Windows.Forms.Label();
            this.btnClearSel = new System.Windows.Forms.Button();
            this.cboCatType = new System.Windows.Forms.ComboBox();
            this.chkCurrentView = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFilter = new System.Windows.Forms.TextBox();
            this.btnAll = new System.Windows.Forms.Button();
            this.btnNone = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lstCat
            // 
            this.lstCat.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lstCat.FormattingEnabled = true;
            this.lstCat.ItemHeight = 16;
            this.lstCat.Location = new System.Drawing.Point(12, 44);
            this.lstCat.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lstCat.Name = "lstCat";
            this.lstCat.Size = new System.Drawing.Size(188, 372);
            this.lstCat.TabIndex = 0;
            this.lstCat.SelectedIndexChanged += new System.EventHandler(this.lstCat_SelectedIndexChanged);
            // 
            // lstTypes
            // 
            this.lstTypes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstTypes.FormattingEnabled = true;
            this.lstTypes.ItemHeight = 16;
            this.lstTypes.Location = new System.Drawing.Point(226, 43);
            this.lstTypes.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.lstTypes.Name = "lstTypes";
            this.lstTypes.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstTypes.Size = new System.Drawing.Size(270, 340);
            this.lstTypes.TabIndex = 1;
            // 
            // btnSelect
            // 
            this.btnSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelect.Location = new System.Drawing.Point(421, 443);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 46);
            this.btnSelect.TabIndex = 2;
            this.btnSelect.Text = "Select";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(226, 443);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 17);
            this.label1.TabIndex = 3;
            this.label1.Text = "Count:";
            // 
            // lblNumber
            // 
            this.lblNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblNumber.AutoSize = true;
            this.lblNumber.Location = new System.Drawing.Point(281, 443);
            this.lblNumber.Name = "lblNumber";
            this.lblNumber.Size = new System.Drawing.Size(16, 17);
            this.lblNumber.TabIndex = 3;
            this.lblNumber.Text = "0";
            this.lblNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClearSel
            // 
            this.btnClearSel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearSel.Location = new System.Drawing.Point(12, 443);
            this.btnClearSel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClearSel.Name = "btnClearSel";
            this.btnClearSel.Size = new System.Drawing.Size(88, 46);
            this.btnClearSel.TabIndex = 4;
            this.btnClearSel.Text = "Clear Selection";
            this.btnClearSel.UseVisualStyleBackColor = true;
            this.btnClearSel.Click += new System.EventHandler(this.btnClearSel_Click);
            // 
            // cboCatType
            // 
            this.cboCatType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboCatType.FormattingEnabled = true;
            this.cboCatType.Location = new System.Drawing.Point(12, 12);
            this.cboCatType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cboCatType.Name = "cboCatType";
            this.cboCatType.Size = new System.Drawing.Size(188, 24);
            this.cboCatType.TabIndex = 5;
            this.cboCatType.SelectedIndexChanged += new System.EventHandler(this.cboCatType_SelectedIndexChanged);
            // 
            // chkCurrentView
            // 
            this.chkCurrentView.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkCurrentView.AutoSize = true;
            this.chkCurrentView.Location = new System.Drawing.Point(226, 398);
            this.chkCurrentView.Margin = new System.Windows.Forms.Padding(4);
            this.chkCurrentView.Name = "chkCurrentView";
            this.chkCurrentView.Size = new System.Drawing.Size(143, 21);
            this.chkCurrentView.TabIndex = 6;
            this.chkCurrentView.Text = "Current View Only";
            this.chkCurrentView.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(224, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(39, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Filter";
            // 
            // txtFilter
            // 
            this.txtFilter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFilter.Location = new System.Drawing.Point(269, 16);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.Size = new System.Drawing.Size(227, 22);
            this.txtFilter.TabIndex = 8;
            this.txtFilter.TextChanged += new System.EventHandler(this.txtFilter_TextChanged);
            // 
            // btnAll
            // 
            this.btnAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAll.Location = new System.Drawing.Point(376, 396);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(57, 23);
            this.btnAll.TabIndex = 9;
            this.btnAll.Text = "&All";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnNone
            // 
            this.btnNone.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNone.Location = new System.Drawing.Point(439, 396);
            this.btnNone.Name = "btnNone";
            this.btnNone.Size = new System.Drawing.Size(57, 23);
            this.btnNone.TabIndex = 9;
            this.btnNone.Text = "&None";
            this.btnNone.UseVisualStyleBackColor = true;
            this.btnNone.Click += new System.EventHandler(this.btnNone_Click);
            // 
            // FormSelectByType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 507);
            this.Controls.Add(this.btnNone);
            this.Controls.Add(this.btnAll);
            this.Controls.Add(this.txtFilter);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chkCurrentView);
            this.Controls.Add(this.cboCatType);
            this.Controls.Add(this.btnClearSel);
            this.Controls.Add(this.lblNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSelect);
            this.Controls.Add(this.lstTypes);
            this.Controls.Add(this.lstCat);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSelectByType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Select By Type";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox lstCat;
        private System.Windows.Forms.ListBox lstTypes;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNumber;
        private System.Windows.Forms.Button btnClearSel;
        private System.Windows.Forms.ComboBox cboCatType;
        private System.Windows.Forms.CheckBox chkCurrentView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFilter;
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnNone;
    }
}