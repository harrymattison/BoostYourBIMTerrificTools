using System;
using System.Windows.Forms;

namespace BoostYourBIMUtils
{
  public partial class formProgress : Form
  {
    string _format;
    bool abortFlag = false;

    public formProgress( string caption, string format, int max )
    {
      _format = format;
      InitializeComponent();

      if (max > 0)
      {
          abortFlag = false;
          Text = caption;
          label1.Text = (null == format) ? caption : string.Format(format, 0);
          progressBar1.Minimum = 0;
          progressBar1.Maximum = max;
          progressBar1.Value = 0;
          Show();
          Application.DoEvents();
      }
    }

    public void Increment()
    {
        this.Invoke(new MethodInvoker(delegate
        {
            if (null != _format)
            {
                label1.Text = string.Format(_format, progressBar1.Value);
            }
            if (progressBar1.Value < progressBar1.Maximum)
            {
                progressBar1.Value++;
                //     progressBar1.Refresh();
                this.Refresh();
            }
        }));
        Application.DoEvents();
    }

    private void btnAbort_Click(object sender, EventArgs e)
    {
        btnAbort.Text = "Aborting...";
        abortFlag = true;
        this.Invoke(new MethodInvoker(delegate
        {
            btnAbort.Refresh();
        }));
    }

    public bool getAbortFlag()
    {
        return abortFlag;
    }

  }
}
