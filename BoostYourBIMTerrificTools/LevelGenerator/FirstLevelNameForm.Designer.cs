namespace LevelGenerator
{
  partial class FirstLevelNameForm
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
      this.buttonOk = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.label7 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.TBSuffix = new System.Windows.Forms.TextBox();
      this.TBPrefix = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.TBNumber = new System.Windows.Forms.NumericUpDown();
      ((System.ComponentModel.ISupportInitialize)(this.TBNumber)).BeginInit();
      this.SuspendLayout();
      // 
      // buttonOk
      // 
      this.buttonOk.Location = new System.Drawing.Point(190, 56);
      this.buttonOk.Name = "buttonOk";
      this.buttonOk.Size = new System.Drawing.Size(73, 26);
      this.buttonOk.TabIndex = 3;
      this.buttonOk.Text = "OK";
      this.buttonOk.UseVisualStyleBackColor = true;
      this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Location = new System.Drawing.Point(269, 56);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(73, 26);
      this.buttonCancel.TabIndex = 4;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Location = new System.Drawing.Point(291, 5);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(41, 12);
      this.label7.TabIndex = 6;
      this.label7.Text = "Suffix";
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Location = new System.Drawing.Point(217, 5);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(41, 12);
      this.label6.TabIndex = 7;
      this.label6.Text = "Number";
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point(147, 5);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(41, 12);
      this.label5.TabIndex = 8;
      this.label5.Text = "Prefix";
      // 
      // TBSuffix
      // 
      this.TBSuffix.Location = new System.Drawing.Point(289, 22);
      this.TBSuffix.Name = "TBSuffix";
      this.TBSuffix.Size = new System.Drawing.Size(53, 21);
      this.TBSuffix.TabIndex = 2;
      // 
      // TBPrefix
      // 
      this.TBPrefix.Location = new System.Drawing.Point(145, 22);
      this.TBPrefix.Name = "TBPrefix";
      this.TBPrefix.Size = new System.Drawing.Size(53, 21);
      this.TBPrefix.TabIndex = 0;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(23, 25);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(101, 12);
      this.label1.TabIndex = 0;
      this.label1.Text = "Start Level Name";
      // 
      // TBNumber
      // 
      this.TBNumber.Location = new System.Drawing.Point(210, 23);
      this.TBNumber.Name = "TBNumber";
      this.TBNumber.Size = new System.Drawing.Size(61, 21);
      this.TBNumber.TabIndex = 1;
      this.TBNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      // 
      // FirstLevelNameForm
      // 
      this.AcceptButton = this.buttonOk;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(356, 90);
      this.Controls.Add(this.TBNumber);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.label6);
      this.Controls.Add(this.label5);
      this.Controls.Add(this.TBSuffix);
      this.Controls.Add(this.TBPrefix);
      this.Controls.Add(this.buttonCancel);
      this.Controls.Add(this.buttonOk);
      this.Controls.Add(this.label1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "FirstLevelNameForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Rename selected Levels";
      ((System.ComponentModel.ISupportInitialize)(this.TBNumber)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button buttonOk;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.TextBox TBSuffix;
    private System.Windows.Forms.TextBox TBPrefix;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.NumericUpDown TBNumber;
  }
}