namespace LevelGenerator
{
  partial class FormElevation
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
            this.label1 = new System.Windows.Forms.Label();
            this.TBElevation = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(107, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Base Level Elevation";
            // 
            // TBElevation
            // 
            this.TBElevation.Location = new System.Drawing.Point(154, 78);
            this.TBElevation.Name = "TBElevation";
            this.TBElevation.Size = new System.Drawing.Size(88, 20);
            this.TBElevation.TabIndex = 1;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(228, 118);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 27);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "OK";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 10);
            this.label2.MaximumSize = new System.Drawing.Size(320, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(318, 39);
            this.label2.TabIndex = 0;
            this.label2.Text = "There is no level in the current document, so this tool will create a default bas" +
    "e row in the level table. Please specify the base level elevation. It can be les" +
    "s than 0.";
            // 
            // FormElevation
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(330, 158);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.TBElevation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormElevation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Base Level Elevation";
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox TBElevation;
    private System.Windows.Forms.Button btnClose;
    private System.Windows.Forms.Label label2;
  }
}