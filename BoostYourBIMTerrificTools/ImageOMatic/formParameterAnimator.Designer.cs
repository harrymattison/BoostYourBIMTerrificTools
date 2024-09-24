namespace BoostYourBIM
{
    partial class formParameterAnimator
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
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lstDisplayStyle = new System.Windows.Forms.ListBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cboFileType = new System.Windows.Forms.ComboBox();
            this.cboFitDir = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPixels = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.chkOpenFolder = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnFolder = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabParameter = new System.Windows.Forms.TabPage();
            this.grpValues = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtInc = new System.Windows.Forms.TextBox();
            this.txtEnd = new System.Windows.Forms.TextBox();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.lstParam = new System.Windows.Forms.ListBox();
            this.tabPhases = new System.Windows.Forms.TabPage();
            this.lstPhases = new System.Windows.Forms.ListBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboExtents = new System.Windows.Forms.ComboBox();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabParameter.SuspendLayout();
            this.grpValues.SuspendLayout();
            this.tabPhases.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lstDisplayStyle);
            this.groupBox4.Location = new System.Drawing.Point(7, 303);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(260, 100);
            this.groupBox4.TabIndex = 4;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Display Style";
            // 
            // lstDisplayStyle
            // 
            this.lstDisplayStyle.FormattingEnabled = true;
            this.lstDisplayStyle.Location = new System.Drawing.Point(6, 16);
            this.lstDisplayStyle.Name = "lstDisplayStyle";
            this.lstDisplayStyle.Size = new System.Drawing.Size(240, 69);
            this.lstDisplayStyle.Sorted = true;
            this.lstDisplayStyle.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(190, 664);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 13;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(109, 664);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 12;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cboFileType);
            this.groupBox5.Controls.Add(this.cboFitDir);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.txtPixels);
            this.groupBox5.Location = new System.Drawing.Point(7, 411);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(260, 72);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Images";
            // 
            // cboFileType
            // 
            this.cboFileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFileType.FormattingEnabled = true;
            this.cboFileType.Location = new System.Drawing.Point(6, 43);
            this.cboFileType.Name = "cboFileType";
            this.cboFileType.Size = new System.Drawing.Size(150, 21);
            this.cboFileType.TabIndex = 8;
            // 
            // cboFitDir
            // 
            this.cboFitDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFitDir.FormattingEnabled = true;
            this.cboFitDir.Location = new System.Drawing.Point(50, 16);
            this.cboFitDir.Name = "cboFitDir";
            this.cboFitDir.Size = new System.Drawing.Size(106, 21);
            this.cboFitDir.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(159, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "pixels";
            // 
            // txtPixels
            // 
            this.txtPixels.Location = new System.Drawing.Point(6, 17);
            this.txtPixels.MaxLength = 5;
            this.txtPixels.Name = "txtPixels";
            this.txtPixels.Size = new System.Drawing.Size(38, 20);
            this.txtPixels.TabIndex = 6;
            this.txtPixels.Text = "512";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.chkOpenFolder);
            this.groupBox6.Controls.Add(this.label8);
            this.groupBox6.Controls.Add(this.btnFolder);
            this.groupBox6.Location = new System.Drawing.Point(7, 543);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(260, 101);
            this.groupBox6.TabIndex = 9;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Output Folder";
            // 
            // chkOpenFolder
            // 
            this.chkOpenFolder.AutoSize = true;
            this.chkOpenFolder.Checked = true;
            this.chkOpenFolder.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOpenFolder.Location = new System.Drawing.Point(7, 78);
            this.chkOpenFolder.Name = "chkOpenFolder";
            this.chkOpenFolder.Size = new System.Drawing.Size(164, 17);
            this.chkOpenFolder.TabIndex = 11;
            this.chkOpenFolder.Text = "&Open Folder After Completion";
            this.chkOpenFolder.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(8, 45);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(239, 32);
            this.label8.TabIndex = 1;
            this.label8.Text = "A sub-folder \"Image-O-Matic\" will be automatically created in the specified folde" +
    "r";
            // 
            // btnFolder
            // 
            this.btnFolder.Location = new System.Drawing.Point(6, 19);
            this.btnFolder.Name = "btnFolder";
            this.btnFolder.Size = new System.Drawing.Size(241, 23);
            this.btnFolder.TabIndex = 10;
            this.btnFolder.UseVisualStyleBackColor = true;
            this.btnFolder.Click += new System.EventHandler(this.btnFolder_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(4, 674);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(83, 13);
            this.linkLabel1.TabIndex = 11;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Send Feedback";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabParameter);
            this.tabControl1.Controls.Add(this.tabPhases);
            this.tabControl1.Location = new System.Drawing.Point(5, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(260, 291);
            this.tabControl1.TabIndex = 15;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabParameter
            // 
            this.tabParameter.Controls.Add(this.grpValues);
            this.tabParameter.Controls.Add(this.lstParam);
            this.tabParameter.Location = new System.Drawing.Point(4, 22);
            this.tabParameter.Name = "tabParameter";
            this.tabParameter.Padding = new System.Windows.Forms.Padding(3);
            this.tabParameter.Size = new System.Drawing.Size(252, 265);
            this.tabParameter.TabIndex = 0;
            this.tabParameter.Text = "Parameter";
            this.tabParameter.UseVisualStyleBackColor = true;
            // 
            // grpValues
            // 
            this.grpValues.Controls.Add(this.label4);
            this.grpValues.Controls.Add(this.label3);
            this.grpValues.Controls.Add(this.label2);
            this.grpValues.Controls.Add(this.txtInc);
            this.grpValues.Controls.Add(this.txtEnd);
            this.grpValues.Controls.Add(this.txtStart);
            this.grpValues.Location = new System.Drawing.Point(6, 196);
            this.grpValues.Name = "grpValues";
            this.grpValues.Size = new System.Drawing.Size(240, 63);
            this.grpValues.TabIndex = 3;
            this.grpValues.TabStop = false;
            this.grpValues.Text = "Values";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(122, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Increment";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(64, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "End";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Start";
            // 
            // txtInc
            // 
            this.txtInc.Location = new System.Drawing.Point(125, 34);
            this.txtInc.Name = "txtInc";
            this.txtInc.Size = new System.Drawing.Size(38, 20);
            this.txtInc.TabIndex = 4;
            this.txtInc.Text = "1";
            // 
            // txtEnd
            // 
            this.txtEnd.Location = new System.Drawing.Point(67, 34);
            this.txtEnd.Name = "txtEnd";
            this.txtEnd.Size = new System.Drawing.Size(38, 20);
            this.txtEnd.TabIndex = 3;
            // 
            // txtStart
            // 
            this.txtStart.Location = new System.Drawing.Point(11, 34);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(38, 20);
            this.txtStart.TabIndex = 2;
            // 
            // lstParam
            // 
            this.lstParam.FormattingEnabled = true;
            this.lstParam.Location = new System.Drawing.Point(6, 17);
            this.lstParam.Name = "lstParam";
            this.lstParam.Size = new System.Drawing.Size(240, 173);
            this.lstParam.Sorted = true;
            this.lstParam.TabIndex = 1;
            this.lstParam.SelectedIndexChanged += new System.EventHandler(this.lstParam_SelectedIndexChanged);
            // 
            // tabPhases
            // 
            this.tabPhases.Controls.Add(this.lstPhases);
            this.tabPhases.Location = new System.Drawing.Point(4, 22);
            this.tabPhases.Name = "tabPhases";
            this.tabPhases.Padding = new System.Windows.Forms.Padding(3);
            this.tabPhases.Size = new System.Drawing.Size(252, 265);
            this.tabPhases.TabIndex = 1;
            this.tabPhases.Text = "Phases";
            this.tabPhases.UseVisualStyleBackColor = true;
            // 
            // lstPhases
            // 
            this.lstPhases.FormattingEnabled = true;
            this.lstPhases.Location = new System.Drawing.Point(6, 13);
            this.lstPhases.Name = "lstPhases";
            this.lstPhases.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstPhases.Size = new System.Drawing.Size(240, 238);
            this.lstPhases.Sorted = true;
            this.lstPhases.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cboExtents);
            this.groupBox1.Location = new System.Drawing.Point(7, 489);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 48);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Image Extents";
            // 
            // cboExtents
            // 
            this.cboExtents.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboExtents.FormattingEnabled = true;
            this.cboExtents.Location = new System.Drawing.Point(6, 19);
            this.cboExtents.Name = "cboExtents";
            this.cboExtents.Size = new System.Drawing.Size(186, 21);
            this.cboExtents.TabIndex = 9;
            // 
            // formParameterAnimator
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(275, 699);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.groupBox4);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "formParameterAnimator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Image-O-Matic";
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabParameter.ResumeLayout(false);
            this.grpValues.ResumeLayout(false);
            this.grpValues.PerformLayout();
            this.tabPhases.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox lstDisplayStyle;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPixels;
        private System.Windows.Forms.ComboBox cboFitDir;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnFolder;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.CheckBox chkOpenFolder;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabParameter;
        private System.Windows.Forms.TabPage tabPhases;
        private System.Windows.Forms.ListBox lstPhases;
        private System.Windows.Forms.GroupBox grpValues;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtInc;
        private System.Windows.Forms.TextBox txtEnd;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.ListBox lstParam;
        private System.Windows.Forms.ComboBox cboFileType;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cboExtents;
    }
}