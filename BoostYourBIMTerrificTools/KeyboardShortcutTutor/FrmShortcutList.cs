using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace BoostYourBIMTerrificTools.KeyboardShortcutTutor
{
    public partial class FrmShortcutList : Form
    {
        public FrmShortcutList(Dictionary<string, Utils.Pair<string, string>> dict)
        {
            InitializeComponent();
            List<DataGridViewRow> rows = new List<DataGridViewRow>();

            gridShortcuts.ColumnCount = 2;
            gridShortcuts.Columns[0].Name = "Command";
            gridShortcuts.Columns[0].FillWeight = 200;
            gridShortcuts.Columns[1].Name = "Shortcut";
            gridShortcuts.Columns[1].FillWeight = 100;

            foreach (var entry in dict)
            {
                Utils.Pair<string, string> pair = entry.Value;
                string shortcut = pair.First;
                string command = pair.Second;
                string[] row = new string[] { command.Replace("&#xA;", " "), shortcut.Replace("#"," ")};
                DataGridViewRow thisRow = new DataGridViewRow();
                thisRow.CreateCells(gridShortcuts, row);
                rows.Add(thisRow);
            }
            gridShortcuts.Rows.AddRange(rows.ToArray());
            gridShortcuts.AutoResizeColumns();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}