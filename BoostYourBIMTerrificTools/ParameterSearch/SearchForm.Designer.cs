namespace StringSearch
{
  partial class SearchForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing )
    {
      if( disposing && ( components != null ) )
      {
        components.Dispose();
      }
      base.Dispose( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SearchForm));
            this.label2 = new System.Windows.Forms.Label();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkWholeWord = new System.Windows.Forms.CheckBox();
            this.chkMatchCase = new System.Windows.Forms.CheckBox();
            this.chkBuiltInParams = new System.Windows.Forms.CheckBox();
            this.chkRegex = new System.Windows.Forms.CheckBox();
            this.rbProject = new System.Windows.Forms.RadioButton();
            this.rbView = new System.Windows.Forms.RadioButton();
            this.rbSelection = new System.Windows.Forms.RadioButton();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cmbParameter = new System.Windows.Forms.ComboBox();
            this.lblParameter = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkRvtLinks = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkUserParams = new System.Windows.Forms.CheckBox();
            this.chkStandardParams = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rbElementType = new System.Windows.Forms.RadioButton();
            this.rbNonElementType = new System.Windows.Forms.RadioButton();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnLogFile = new System.Windows.Forms.Button();
            this.cmbSearchString = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Category:";
            // 
            // cmbCategory
            // 
            this.cmbCategory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(7, 125);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(650, 21);
            this.cmbCategory.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbSearchString);
            this.groupBox1.Controls.Add(this.chkWholeWord);
            this.groupBox1.Controls.Add(this.chkMatchCase);
            this.groupBox1.Location = new System.Drawing.Point(7, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(650, 93);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Find options";
            // 
            // chkWholeWord
            // 
            this.chkWholeWord.AutoSize = true;
            this.chkWholeWord.Location = new System.Drawing.Point(6, 66);
            this.chkWholeWord.Name = "chkWholeWord";
            this.chkWholeWord.Size = new System.Drawing.Size(164, 17);
            this.chkWholeWord.TabIndex = 1;
            this.chkWholeWord.Text = "Match entire parameter value";
            this.chkWholeWord.UseVisualStyleBackColor = true;
            // 
            // chkMatchCase
            // 
            this.chkMatchCase.AutoSize = true;
            this.chkMatchCase.Location = new System.Drawing.Point(6, 46);
            this.chkMatchCase.Name = "chkMatchCase";
            this.chkMatchCase.Size = new System.Drawing.Size(82, 17);
            this.chkMatchCase.TabIndex = 0;
            this.chkMatchCase.Text = "Match case";
            this.chkMatchCase.UseVisualStyleBackColor = true;
            // 
            // chkBuiltInParams
            // 
            this.chkBuiltInParams.AutoSize = true;
            this.chkBuiltInParams.Location = new System.Drawing.Point(6, 33);
            this.chkBuiltInParams.Name = "chkBuiltInParams";
            this.chkBuiltInParams.Size = new System.Drawing.Size(193, 17);
            this.chkBuiltInParams.TabIndex = 1;
            this.chkBuiltInParams.Text = "Search BuiltInParameters (API only)";
            this.chkBuiltInParams.UseVisualStyleBackColor = true;
            this.chkBuiltInParams.CheckedChanged += new System.EventHandler(this.chkBuiltInParams_CheckedChanged);
            // 
            // chkRegex
            // 
            this.chkRegex.AutoSize = true;
            this.chkRegex.Location = new System.Drawing.Point(6, 15);
            this.chkRegex.Name = "chkRegex";
            this.chkRegex.Size = new System.Drawing.Size(138, 17);
            this.chkRegex.TabIndex = 0;
            this.chkRegex.Text = "Use regular expressions";
            this.chkRegex.UseVisualStyleBackColor = true;
            this.chkRegex.CheckedChanged += new System.EventHandler(this.chkRegex_CheckedChanged);
            // 
            // rbProject
            // 
            this.rbProject.AutoSize = true;
            this.rbProject.Location = new System.Drawing.Point(10, 49);
            this.rbProject.Name = "rbProject";
            this.rbProject.Size = new System.Drawing.Size(87, 17);
            this.rbProject.TabIndex = 2;
            this.rbProject.Text = "Entire project";
            this.rbProject.UseVisualStyleBackColor = true;
            // 
            // rbView
            // 
            this.rbView.AutoSize = true;
            this.rbView.Checked = true;
            this.rbView.Location = new System.Drawing.Point(10, 32);
            this.rbView.Name = "rbView";
            this.rbView.Size = new System.Drawing.Size(84, 17);
            this.rbView.TabIndex = 1;
            this.rbView.TabStop = true;
            this.rbView.Text = "Current view";
            this.rbView.UseVisualStyleBackColor = true;
            // 
            // rbSelection
            // 
            this.rbSelection.AutoSize = true;
            this.rbSelection.Location = new System.Drawing.Point(10, 15);
            this.rbSelection.Name = "rbSelection";
            this.rbSelection.Size = new System.Drawing.Size(104, 17);
            this.rbSelection.TabIndex = 0;
            this.rbSelection.Text = "Current selection";
            this.rbSelection.UseVisualStyleBackColor = true;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Location = new System.Drawing.Point(492, 540);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 11;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(582, 540);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 12;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // cmbParameter
            // 
            this.cmbParameter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbParameter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParameter.FormattingEnabled = true;
            this.cmbParameter.Location = new System.Drawing.Point(7, 171);
            this.cmbParameter.Name = "cmbParameter";
            this.cmbParameter.Size = new System.Drawing.Size(650, 21);
            this.cmbParameter.TabIndex = 5;
            // 
            // lblParameter
            // 
            this.lblParameter.AutoSize = true;
            this.lblParameter.Location = new System.Drawing.Point(7, 154);
            this.lblParameter.Name = "lblParameter";
            this.lblParameter.Size = new System.Drawing.Size(91, 13);
            this.lblParameter.TabIndex = 4;
            this.lblParameter.Text = "Built-in parameter:";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chkRvtLinks);
            this.groupBox2.Controls.Add(this.rbProject);
            this.groupBox2.Controls.Add(this.rbSelection);
            this.groupBox2.Controls.Add(this.rbView);
            this.groupBox2.Location = new System.Drawing.Point(7, 200);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(201, 88);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Element selection";
            // 
            // chkRvtLinks
            // 
            this.chkRvtLinks.AutoSize = true;
            this.chkRvtLinks.Location = new System.Drawing.Point(10, 67);
            this.chkRvtLinks.Name = "chkRvtLinks";
            this.chkRvtLinks.Size = new System.Drawing.Size(113, 17);
            this.chkRvtLinks.TabIndex = 3;
            this.chkRvtLinks.Text = "Search RVT Links";
            this.chkRvtLinks.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.chkBuiltInParams);
            this.groupBox3.Controls.Add(this.chkRegex);
            this.groupBox3.Location = new System.Drawing.Point(7, 421);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(201, 57);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Advanced";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.chkUserParams);
            this.groupBox4.Controls.Add(this.chkStandardParams);
            this.groupBox4.Location = new System.Drawing.Point(7, 359);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(201, 56);
            this.groupBox4.TabIndex = 8;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Parameter selection";
            // 
            // chkUserParams
            // 
            this.chkUserParams.AutoSize = true;
            this.chkUserParams.Checked = true;
            this.chkUserParams.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkUserParams.Location = new System.Drawing.Point(7, 32);
            this.chkUserParams.Name = "chkUserParams";
            this.chkUserParams.Size = new System.Drawing.Size(139, 17);
            this.chkUserParams.TabIndex = 1;
            this.chkUserParams.Text = "User (shared and family)";
            this.chkUserParams.UseVisualStyleBackColor = true;
            this.chkUserParams.CheckedChanged += new System.EventHandler(this.chkUserParams_CheckedChanged);
            // 
            // chkStandardParams
            // 
            this.chkStandardParams.AutoSize = true;
            this.chkStandardParams.Checked = true;
            this.chkStandardParams.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkStandardParams.Location = new System.Drawing.Point(7, 15);
            this.chkStandardParams.Name = "chkStandardParams";
            this.chkStandardParams.Size = new System.Drawing.Size(90, 17);
            this.chkStandardParams.TabIndex = 0;
            this.chkStandardParams.Text = "Revit (built-in)";
            this.chkStandardParams.UseVisualStyleBackColor = true;
            this.chkStandardParams.CheckedChanged += new System.EventHandler(this.chkStandardParams_CheckedChanged);
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.rbElementType);
            this.groupBox5.Controls.Add(this.rbNonElementType);
            this.groupBox5.Location = new System.Drawing.Point(7, 294);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(201, 59);
            this.groupBox5.TabIndex = 13;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Instances versus Types and Symbols";
            // 
            // rbElementType
            // 
            this.rbElementType.AutoSize = true;
            this.rbElementType.Location = new System.Drawing.Point(9, 34);
            this.rbElementType.Name = "rbElementType";
            this.rbElementType.Size = new System.Drawing.Size(117, 17);
            this.rbElementType.TabIndex = 1;
            this.rbElementType.Text = "Types and Symbols";
            this.rbElementType.UseVisualStyleBackColor = true;
            this.rbElementType.CheckedChanged += new System.EventHandler(this.rbElementType_CheckedChanged);
            // 
            // rbNonElementType
            // 
            this.rbNonElementType.AutoSize = true;
            this.rbNonElementType.Checked = true;
            this.rbNonElementType.Location = new System.Drawing.Point(9, 16);
            this.rbNonElementType.Name = "rbNonElementType";
            this.rbNonElementType.Size = new System.Drawing.Size(71, 17);
            this.rbNonElementType.TabIndex = 0;
            this.rbNonElementType.TabStop = true;
            this.rbNonElementType.Text = "Instances";
            this.rbNonElementType.UseVisualStyleBackColor = true;
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Location = new System.Drawing.Point(6, 105);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(235, 13);
            this.linkLabel2.TabIndex = 7;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Take an online course in Revit API programming";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(6, 64);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(173, 13);
            this.linkLabel1.TabIndex = 7;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Support Boost Your BIM at Patreon";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.Location = new System.Drawing.Point(3, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(389, 48);
            this.label4.TabIndex = 0;
            this.label4.Text = "Your generous support makes it possible for Boost Your BIM to provide free tools " +
    "like this to the Revit community.";
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.linkLabel2);
            this.groupBox6.Controls.Add(this.linkLabel1);
            this.groupBox6.Controls.Add(this.label4);
            this.groupBox6.Location = new System.Drawing.Point(239, 215);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(398, 201);
            this.groupBox6.TabIndex = 14;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Support Free Revit Tools Like This";
            // 
            // btnLogFile
            // 
            this.btnLogFile.Location = new System.Drawing.Point(7, 484);
            this.btnLogFile.Name = "btnLogFile";
            this.btnLogFile.Size = new System.Drawing.Size(97, 23);
            this.btnLogFile.TabIndex = 15;
            this.btnLogFile.Text = "Show Log File";
            this.btnLogFile.UseVisualStyleBackColor = true;
            this.btnLogFile.Click += new System.EventHandler(this.btnLogFile_Click);
            // 
            // cmbSearchString
            // 
            this.cmbSearchString.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSearchString.FormattingEnabled = true;
            this.cmbSearchString.Location = new System.Drawing.Point(84, 19);
            this.cmbSearchString.Name = "cmbSearchString";
            this.cmbSearchString.Size = new System.Drawing.Size(560, 21);
            this.cmbSearchString.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Search String";
            // 
            // SearchForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(669, 574);
            this.Controls.Add(this.btnLogFile);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cmbParameter);
            this.Controls.Add(this.lblParameter);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(200, 200);
            this.Name = "SearchForm";
            this.Text = "Revit String Search";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox cmbCategory;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.CheckBox chkRegex;
    private System.Windows.Forms.CheckBox chkWholeWord;
    private System.Windows.Forms.CheckBox chkMatchCase;
    private System.Windows.Forms.CheckBox chkBuiltInParams;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.ComboBox cmbParameter;
    private System.Windows.Forms.Label lblParameter;
    private System.Windows.Forms.RadioButton rbProject;
    private System.Windows.Forms.RadioButton rbView;
    private System.Windows.Forms.RadioButton rbSelection;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.CheckBox chkUserParams;
    private System.Windows.Forms.CheckBox chkStandardParams;
    private System.Windows.Forms.GroupBox groupBox5;
    private System.Windows.Forms.RadioButton rbElementType;
    private System.Windows.Forms.RadioButton rbNonElementType;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnLogFile;
        private System.Windows.Forms.CheckBox chkRvtLinks;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbSearchString;
    }
}