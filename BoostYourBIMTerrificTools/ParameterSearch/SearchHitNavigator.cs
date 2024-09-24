#region Namespaces
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.Linq;
using OfficeOpenXml;
using System.IO;
using BoostYourBIMTerrificTools;
using Autodesk.Revit.UI;
#endregion // Namespaces

namespace StringSearch
{
  /// <summary>
  /// Modeless form to dsiplay a list of all string search hits.
  /// User can double click on any row to zoom to and highlight
  /// that element.
  /// </summary>
  partial class SearchHitNavigator : Form
  {
    static SearchHitNavigator _singleton_instance = null;
    static Point? _last_location = null;
    static Size _last_size;

    public static bool IsShowing
    {
      get
      {
        return null != _singleton_instance;
      }
    }

    public static void Show(
      SortableBindingList<SearchHit> data,
      Ribbon.SetElementId set_id,
      JtWindowHandle h )
    {
      if( null == _singleton_instance )
      {
        _singleton_instance
          = new SearchHitNavigator(
            data, set_id );

        _singleton_instance.Load 
          += new EventHandler( OnLoad );

        _singleton_instance.FormClosing
          += new FormClosingEventHandler(
            OnFormClosing );

        _singleton_instance.FormClosing
          += new FormClosingEventHandler(
            OnFormClosing );

        _singleton_instance.Disposed 
          += new EventHandler( OnDisposed );

        _singleton_instance.Show( h );
      }
      else
      {
        _singleton_instance.dataGridView1.DataSource
          = data;
      }
    }

    public static void Shutdown()
    {
      if( null != _singleton_instance )
      {
        _singleton_instance.Close();
      }
    }

    static void OnLoad( 
      object sender, 
      EventArgs e )
    {
      if( null != _last_location )
      {
        _singleton_instance.Location 
          = ( Point ) _last_location;

        _singleton_instance.Size = _last_size;
      }
    }

    static void OnFormClosing( 
      object sender, 
      FormClosingEventArgs e )
    {
      _last_location = _singleton_instance.Location;
      _last_size = _singleton_instance.Size;
    }

    static void OnDisposed( 
      object sender, 
      EventArgs e )
    {
      _singleton_instance = null;
    }

    Ribbon.SetElementId _set_id;

    SearchHitNavigator(
      SortableBindingList<SearchHit> a,
      Ribbon.SetElementId set_id )
    {
      InitializeComponent();
      dataGridView1.DataSource = a;
      dataGridView1.CellDoubleClick 
        += new DataGridViewCellEventHandler( 
          dataGridView1_CellDoubleClick );
      _set_id = set_id;
    }

    void SetElementIdFromRow(
      int rowIndex,
      bool doubleClick )
    {
      // Do something on double click, 
      // except when on the header:

      if( rowIndex > -1 )
      {
        _set_id( getIdForRow(dataGridView1.Rows[rowIndex]).IntegerValue);
      }
    }

        private Autodesk.Revit.DB.ElementId getIdForRow(DataGridViewRow row)
        {

            int n = row.Cells.Count;

            DataGridViewCell cell = row.Cells[n - 1];

            int id = (int)cell.Value;

            return new Autodesk.Revit.DB.ElementId(id);
        }

    void dataGridView1_CellDoubleClick( 
      object sender, 
      DataGridViewCellEventArgs e )
    {
      SetElementIdFromRow( e.RowIndex, true );
    }

        private void btnShow_Click(object sender, EventArgs e)
        {
            DataGridViewRow selectedRow = dataGridView1.SelectedRows.Cast<DataGridViewRow>().FirstOrDefault();
            if (selectedRow == null)
                return;

            try
            {
                new UIDocument(Utils.doc).ShowElements(getIdForRow(selectedRow));
            }
            catch
            { }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            string filename = "";
            using (SaveFileDialog fileDialog = new SaveFileDialog())
            {
                fileDialog.DefaultExt = ".xlsx";
                fileDialog.FileName = Path.GetFileNameWithoutExtension(Utils.doc.PathName);
                fileDialog.AutoUpgradeEnabled = false;
                fileDialog.AddExtension = true;
                fileDialog.Filter = "Excel Files | *.xlsx";
                if (fileDialog.ShowDialog() == DialogResult.Cancel)
                    return;
                filename = fileDialog.FileName;
            }

            if (File.Exists(filename))
                File.Delete(filename);

            using (ExcelPackage package = new ExcelPackage(new System.IO.FileInfo(filename)))
            {
                ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Parameters");
                for (int r = 0; r < dataGridView1.Rows.Count; r++)
                {
                    for (int c = 0; c < dataGridView1.Columns.Count; c++)
                    {
                        DataGridViewCell cell = dataGridView1.Rows[r].Cells[c];
                        if (cell.Value == null)
                            continue;
                        sheet.Cells[r + 1, c + 1].Value = cell.Value.ToString();
                    }
                }
                sheet.Cells.AutoFitColumns();
                package.Save();
            }
            try
            {
                Process.Start(filename);
            }
            catch
            { }
        }
    }
}
