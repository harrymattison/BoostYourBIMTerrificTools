namespace LevelDisplacer
{
    partial class FrmLevelDisplacer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLevelDisplacer));
            this.grpOffset = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtZ = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtY = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtX = new System.Windows.Forms.TextBox();
            this.chkHide = new System.Windows.Forms.CheckBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblLearn = new System.Windows.Forms.LinkLabel();
            this.lblPatreon = new System.Windows.Forms.LinkLabel();
            this.label4 = new System.Windows.Forms.Label();
            this.grpOffset.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpOffset
            // 
            this.grpOffset.Controls.Add(this.label3);
            this.grpOffset.Controls.Add(this.txtZ);
            this.grpOffset.Controls.Add(this.label2);
            this.grpOffset.Controls.Add(this.txtY);
            this.grpOffset.Controls.Add(this.label1);
            this.grpOffset.Controls.Add(this.txtX);
            resources.ApplyResources(this.grpOffset, "grpOffset");
            this.grpOffset.Name = "grpOffset";
            this.grpOffset.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // txtZ
            // 
            resources.ApplyResources(this.txtZ, "txtZ");
            this.txtZ.Name = "txtZ";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txtY
            // 
            resources.ApplyResources(this.txtY, "txtY");
            this.txtY.Name = "txtY";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txtX
            // 
            resources.ApplyResources(this.txtX, "txtX");
            this.txtX.Name = "txtX";
            // 
            // chkHide
            // 
            resources.ApplyResources(this.chkHide, "chkHide");
            this.chkHide.Name = "chkHide";
            this.chkHide.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            resources.ApplyResources(this.btnOK, "btnOK");
            this.btnOK.Name = "btnOK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.lblLearn);
            this.groupBox1.Controls.Add(this.lblPatreon);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // lblLearn
            // 
            resources.ApplyResources(this.lblLearn, "lblLearn");
            this.lblLearn.Name = "lblLearn";
            this.lblLearn.TabStop = true;
            this.lblLearn.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblLearn_LinkClicked);
            // 
            // lblPatreon
            // 
            resources.ApplyResources(this.lblPatreon, "lblPatreon");
            this.lblPatreon.Name = "lblPatreon";
            this.lblPatreon.TabStop = true;
            this.lblPatreon.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblPatreon_LinkClicked);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // FrmLevelDisplacer
            // 
            this.AcceptButton = this.btnOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.chkHide);
            this.Controls.Add(this.grpOffset);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmLevelDisplacer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.grpOffset.ResumeLayout(false);
            this.grpOffset.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpOffset;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtZ;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtY;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtX;
        private System.Windows.Forms.CheckBox chkHide;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.LinkLabel lblLearn;
        private System.Windows.Forms.LinkLabel lblPatreon;
        private System.Windows.Forms.Label label4;
    }
}