#region Namespaces
using System;
using System.Collections.Generic;
using System.Text;
#endregion // Namespaces

namespace StringSearch
{
  /// <summary>
  /// Manage the various string search options.
  /// </summary>
  public class SearchOptions
  {
    public SearchOptions(
      string searchString,
      string parameterName,
      bool matchCase,
      bool wholeWord,
      bool regex,
      //bool elementType,
      //bool nonElementType,
      bool builtInParams,
      //bool stringValued,
      //bool intValued,
      //bool realValued,
      //bool elementIdValued,
      //bool nonStringValued 
      bool standardParams,
      bool userParams,
      bool linkedFiles

    )
    {
      SearchString = searchString;
      ParameterName = parameterName;
      MatchCase = matchCase;
      WholeWord = wholeWord;
      Regex = regex;
      //ElementType = elementType;
      //NonElementType = nonElementType;
      BuiltInParams = builtInParams;
      //StringValued = stringValued;
      //IntValued = intValued;
      //RealValued = realValued;
      //ElementIdValued = elementIdValued;
      //NonStringValued = nonStringValued;
      StandardParams = standardParams;
      UserParams = userParams;
            LinkedFiles = linkedFiles;
    }

    public string SearchString { get; set; }
    public string ParameterName { get; set; }
    public bool MatchCase { get; set; }
    public bool WholeWord { get; set; }
    public bool Regex { get; set; }
    //public bool ElementType { get; set; }
    //public bool NonElementType { get; set; }
    public bool BuiltInParams { get; set; }
    //public bool StringValued { get; set; }
    //public bool IntValued { get; set; }
    //public bool RealValued { get; set; }
    //public bool ElementIdValued { get; set; }
    //public bool NonStringValued { get; set; }
    public bool StandardParams { get; set; }
    public bool UserParams { get; set; }

        public bool LinkedFiles { get; set; }
  }
}
