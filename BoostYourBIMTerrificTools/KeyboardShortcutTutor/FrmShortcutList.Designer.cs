namespace BoostYourBIMTerrificTools.KeyboardShortcutTutor
{
    partial class FrmShortcutList
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
            this.gridShortcuts = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.gridShortcuts)).BeginInit();
            this.SuspendLayout();
            // 
            // gridShortcuts
            // 
            this.gridShortcuts.AllowUserToAddRows = false;
            this.gridShortcuts.AllowUserToDeleteRows = false;
            this.gridShortcuts.AllowUserToResizeRows = false;
            this.gridShortcuts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridShortcuts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.gridShortcuts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridShortcuts.Location = new System.Drawing.Point(12, 12);
            this.gridShortcuts.Name = "gridShortcuts";
            this.gridShortcuts.ReadOnly = true;
            this.gridShortcuts.RowHeadersVisible = false;
            this.gridShortcuts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridShortcuts.ShowRowErrors = false;
            this.gridShortcuts.Size = new System.Drawing.Size(256, 418);
            this.gridShortcuts.TabIndex = 0;
            // 
            // FrmShortcutList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 442);
            this.Controls.Add(this.gridShortcuts);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmShortcutList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Shortcut List";
            ((System.ComponentModel.ISupportInitialize)(this.gridShortcuts)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView gridShortcuts;
    }
}