#region Namespaces
using System;
using System.Diagnostics;
using System.IO;
#endregion // Namespaces

namespace StringSearch
{
  /// <summary>
  /// Helper class to manage a log file.
  /// </summary>
  class JtLogFile : IDisposable
  {
    string _path;
    StreamWriter _sw;

    public JtLogFile( string basename )
    {
      _path = System.IO.Path.Combine(
        System.IO.Path.GetTempPath(),
        basename + ".log" );

      _sw = new StreamWriter( _path, true );

      _sw.WriteLine( "\r\n\r\n{0} Start string search\r\n",
        DateTime.Now.ToString( "u" ) );
    }

    public void Dispose()
    {
      _sw.WriteLine( "\r\n\r\n{0} Terminate string search\r\n",
        DateTime.Now.ToString( "u" ) );

      _sw.Close();
      _sw.Dispose();
    }

    /// <summary>
    /// Log a new entry to the file.
    /// </summary>
    public void Log( string s )
    {
      _sw.WriteLine( s );
      Debug.WriteLine( s );
    }

    public string Path
    {
      get
      {
        return _path;
      }
    }
  }
}
