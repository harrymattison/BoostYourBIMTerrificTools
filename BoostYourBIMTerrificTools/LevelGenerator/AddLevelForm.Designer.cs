namespace LevelGenerator
{
  partial class AddLevelForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddLevelForm));
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textLevelHeight = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.LevelGrid = new System.Windows.Forms.DataGridView();
            this.Index = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelHeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LevelElevation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnRenameLevel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textCount = new System.Windows.Forms.NumericUpDown();
            this.TBNumber = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TBSuffix = new System.Windows.Forms.TextBox();
            this.TBPrefix = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.LevelGrid)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TBNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 118);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Repeat Levels";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Level Height";
            // 
            // textLevelHeight
            // 
            this.textLevelHeight.Location = new System.Drawing.Point(162, 81);
            this.textLevelHeight.Name = "textLevelHeight";
            this.textLevelHeight.Size = new System.Drawing.Size(53, 20);
            this.textLevelHeight.TabIndex = 3;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(122, 150);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(93, 29);
            this.btnAdd.TabIndex = 5;
            this.btnAdd.Text = "<< Insert";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnRemove
            // 
            this.btnRemove.Location = new System.Drawing.Point(423, 228);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(93, 29);
            this.btnRemove.TabIndex = 6;
            this.btnRemove.Text = ">> Delete";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(423, 354);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(93, 29);
            this.btnOk.TabIndex = 8;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(534, 354);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(93, 29);
            this.btnCancel.TabIndex = 9;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // LevelGrid
            // 
            this.LevelGrid.AllowUserToAddRows = false;
            this.LevelGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.LevelGrid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Index,
            this.LevelName,
            this.LevelHeight,
            this.LevelElevation});
            this.LevelGrid.Location = new System.Drawing.Point(12, 19);
            this.LevelGrid.Name = "LevelGrid";
            this.LevelGrid.RowHeadersWidth = 15;
            this.LevelGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.LevelGrid.RowTemplate.Height = 23;
            this.LevelGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.LevelGrid.Size = new System.Drawing.Size(376, 368);
            this.LevelGrid.TabIndex = 10;
            this.LevelGrid.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.LevelGrid_CellEndEdit);
            this.LevelGrid.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.LevelGrid_UserDeletingRow);
            // 
            // Index
            // 
            this.Index.HeaderText = "Index";
            this.Index.Name = "Index";
            this.Index.ReadOnly = true;
            this.Index.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.Index.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Index.Width = 55;
            // 
            // LevelName
            // 
            this.LevelName.HeaderText = "Name";
            this.LevelName.Name = "LevelName";
            this.LevelName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelName.ToolTipText = "Each level name, they can be changed from the grid";
            this.LevelName.Width = 85;
            // 
            // LevelHeight
            // 
            this.LevelHeight.HeaderText = "Level Height";
            this.LevelHeight.Name = "LevelHeight";
            this.LevelHeight.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelHeight.ToolTipText = "The distance of each level to the adjacent lower level";
            this.LevelHeight.Width = 95;
            // 
            // LevelElevation
            // 
            this.LevelElevation.HeaderText = "Elevation";
            this.LevelElevation.Name = "LevelElevation";
            this.LevelElevation.ReadOnly = true;
            this.LevelElevation.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.LevelElevation.ToolTipText = "The elevation of each level";
            this.LevelElevation.Width = 120;
            // 
            // btnRenameLevel
            // 
            this.btnRenameLevel.Location = new System.Drawing.Point(534, 228);
            this.btnRenameLevel.Name = "btnRenameLevel";
            this.btnRenameLevel.Size = new System.Drawing.Size(93, 29);
            this.btnRenameLevel.TabIndex = 7;
            this.btnRenameLevel.Text = "Rename Levels";
            this.btnRenameLevel.UseVisualStyleBackColor = true;
            this.btnRenameLevel.Click += new System.EventHandler(this.btnRenameLevel_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textCount);
            this.groupBox1.Controls.Add(this.TBNumber);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.TBSuffix);
            this.groupBox1.Controls.Add(this.TBPrefix);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.textLevelHeight);
            this.groupBox1.Location = new System.Drawing.Point(395, 19);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 195);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Insert multiple levels to table";
            // 
            // textCount
            // 
            this.textCount.Location = new System.Drawing.Point(162, 114);
            this.textCount.Name = "textCount";
            this.textCount.Size = new System.Drawing.Size(53, 20);
            this.textCount.TabIndex = 4;
            this.textCount.Tag = "";
            this.textCount.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // TBNumber
            // 
            this.TBNumber.Location = new System.Drawing.Point(89, 48);
            this.TBNumber.Name = "TBNumber";
            this.TBNumber.Size = new System.Drawing.Size(56, 20);
            this.TBNumber.TabIndex = 2;
            this.TBNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(169, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(33, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Suffix";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(82, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(69, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Start Number";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Prefix";
            // 
            // TBSuffix
            // 
            this.TBSuffix.Location = new System.Drawing.Point(162, 48);
            this.TBSuffix.Name = "TBSuffix";
            this.TBSuffix.Size = new System.Drawing.Size(53, 20);
            this.TBSuffix.TabIndex = 2;
            // 
            // TBPrefix
            // 
            this.TBPrefix.Location = new System.Drawing.Point(18, 48);
            this.TBPrefix.Name = "TBPrefix";
            this.TBPrefix.Size = new System.Drawing.Size(53, 20);
            this.TBPrefix.TabIndex = 0;
            // 
            // AddLevelForm
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(633, 396);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.LevelGrid);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnRenameLevel);
            this.Controls.Add(this.btnRemove);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AddLevelForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Level Generator";
            this.Load += new System.EventHandler(this.AddLevelForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.LevelGrid)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.textCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TBNumber)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox textLevelHeight;
    private System.Windows.Forms.Button btnAdd;
    private System.Windows.Forms.Button btnRemove;
    private System.Windows.Forms.Button btnOk;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.DataGridView LevelGrid;
    private System.Windows.Forms.Button btnRenameLevel;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox TBSuffix;
    private System.Windows.Forms.TextBox TBPrefix;
    private System.Windows.Forms.DataGridViewTextBoxColumn Index;
    private System.Windows.Forms.DataGridViewTextBoxColumn LevelName;
    private System.Windows.Forms.DataGridViewTextBoxColumn LevelHeight;
    private System.Windows.Forms.DataGridViewTextBoxColumn LevelElevation;
    private System.Windows.Forms.NumericUpDown TBNumber;
    private System.Windows.Forms.NumericUpDown textCount;
  }
}