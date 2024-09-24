using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace ImageOMatic
{
    public static class Utils
    {
        static string _bundle;
        public static string bundlePath
        {
            get
            { return _bundle; }
            set
            { _bundle = value; }
        }

        public static Parameter getParam(Element e, string paramname)
        {
#if RELEASE2013 || RELEASE2014
            return e.get_Parameter(paramname);
#else
            return e.GetOrderedParameters().FirstOrDefault(q => q.Definition.Name == paramname);
#endif
        }

        public class RollbackFailures : IFailuresPreprocessor
        {
            FailureProcessingResult IFailuresPreprocessor.PreprocessFailures(FailuresAccessor failuresAccessor)
            {
                if (failuresAccessor.GetFailureMessages().Count > 0)
                    return FailureProcessingResult.ProceedWithRollBack;
                else
                    return FailureProcessingResult.Continue;
            }
        }

        // Allow selection of FamilyInstance elements only
        public class FamilyInstanceOrDisplacementSelectionFilter : ISelectionFilter
        {
            public bool AllowElement(Element element)
            { 
                if (element is FamilyInstance)
                    return true;

#if !R2013
                if (element is DisplacementElement)
                    return true;
#endif

                return false;
            }
            public bool AllowReference(Reference refer, XYZ point)
            { return false; }
        }

        public static bool isDouble(string s)
        {
            double result;
            if (Double.TryParse(s, out result))
                return true;
            return false;
        }

        public static bool isInt(string s)
        {
            int result;
            if (Int32.TryParse(s, out result))
                return true;
            return false;
        }

        public static string validateDouble(string s, string name)
        {
            if (isDouble(s))
                return "";
            else
                return "'" + name + "' must be a double\n";
        }

        public static string validateInt(string s, string name)
        {
            if (isInt(s))
                return "";
            else
                return "'" + name + "' must be a integer\n";
        }
    }
}
