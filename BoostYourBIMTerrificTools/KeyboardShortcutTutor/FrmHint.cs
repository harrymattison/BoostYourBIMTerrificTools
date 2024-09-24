using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace BoostYourBIMTerrificTools.KeyboardShortcutTutor
{
    public partial class FrmHint : Form
    {
        bool showingList = false;
        Dictionary<string, Utils.Pair<string, string>> dict = new Dictionary<string, Utils.Pair<string, string>>();
        public FrmHint(Dictionary<string, Utils.Pair<string, string>> thisDict, string shortcut, string commandName, int x, int y)
        {
            InitializeComponent();
            this.Location = new Point(x - this.Width - 10, y - this.Height - 10);
            dict = thisDict;
            string shortcutText = shortcut.Replace("#", " ");

            //int compare = DateTime.Compare(new DateTime(2016, 03, 01), DateTime.Today);
            //TaskDialog.Show("compare", compare.ToString());

            //if (compare < 0 && !shortcutText.Contains("W"))
            //{
            //    lblShortcut.Visible = false;
            //    lblCommandName.Visible = true;
            //    lblCommandName.Text = "1.0.4 - Expired";
            //    linkLabel1.Text = "Purchase License";
            //}
            //else
            //{
                lblShortcut.Visible = true;
                lblCommandName.Visible = true;
                lblShortcut.Text = shortcutText;
                lblCommandName.Text = commandName;
                linkLabel1.Text = "Boost Your BIM";
            //}
        }

        private void FrmHint_Shown(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Thread.Sleep(8000);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!showingList)
                this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://boostyourbim.wordpress.com/");
        }

        private void btnShowList_Click(object sender, System.EventArgs e)
        {
            using (FrmShortcutList frmShortCutList = new FrmShortcutList(dict))
            {
                showingList = true;
                if (Utils.loadShortcuts())
                {
                    frmShortCutList.ShowDialog();
                    showingList = false;
                }
                else
                {
                    TaskDialog.Show("No shortcuts file", "Shortcuts file does not exist. Open the Revit Keyboard Shortcut UI and press Export.");
                }
            }
            this.Close();
        }

        private void btnClose_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.patreon.com/BoostYourBIM");
        }

    }
}
