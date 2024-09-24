using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Xml;

namespace BoostYourBIMTerrificTools.ModelCompare
{
    [Transaction(TransactionMode.Manual)]
    public class ModelCompareCommand : IExternalCommand
    {
        Result IExternalCommand.Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            string inputfile;
            System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "Select Baseline File",
                Filter = "Revit Files|*.rvt",
                AutoUpgradeEnabled = false
            };
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return Result.Cancelled;
            inputfile = openFileDialog.FileName;

            string newfile;
            System.Windows.Forms.OpenFileDialog openFileDialog1 = new System.Windows.Forms.OpenFileDialog
            {
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                Title = "Select Modified File",
                Filter = "Revit Files|*.rvt",
                AutoUpgradeEnabled = false
            };
            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.Cancel)
                return Result.Cancelled;
            newfile = openFileDialog1.FileName;

            Document inputDoc = commandData.Application.Application.OpenDocumentFile(inputfile);
            string xml1 = DocToModelData(inputDoc);
            try
            {
                inputDoc.Close(false);
            }
            catch
            { }

            Document newDoc = commandData.Application.OpenAndActivateDocument(newfile).Document;
            string xml2 = DocToModelData(newDoc);

            //XmlDiff diff = new XmlDiff();
            //string diffgramFile = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(inputfile) + "-DIFF-" + Path.GetFileNameWithoutExtension(newfile) + ".xml");
            //bool isEqual = false;
            //using (XmlTextWriter xw = new XmlTextWriter(new StreamWriter(diffgramFile)))
            //{
            //    xw.Formatting = Formatting.Indented;
            //    isEqual = diff.Compare(xml1, xml2, false, xw);
            //}

            //DockablePane dp = commandData.Application.GetDockablePane(Utils.ModelComparePaneId);
            //dp.Show();
            return Result.Succeeded;
        }

        public static string DocToModelData(Document doc)
        {
            List<Element> elements = new FilteredElementCollector(doc).OfClass(typeof(Wall)).WhereElementIsNotElementType().ToList();
          //  elements.AddRange(new FilteredElementCollector(doc).WhereElementIsElementType().ToList());

            List<ElementData> elementDatas = new List<ElementData>();
            foreach (Element e in elements.OrderBy(q => q.UniqueId))
            {
                if (e.Category == null)
                    continue;

                List<XYZSerializable> locations = new List<XYZSerializable>(); ;
                List<ParameterData> parameterDatas = new List<ParameterData>();
                foreach (Parameter p in e.Parameters.Cast<Parameter>().Where(q => q.Definition != null).OrderBy(q => q.Definition.Name))
                {
                    string val = "";
                    if (p.StorageType == StorageType.Double)
                        val = p.AsValueString();
                    else if (p.StorageType == StorageType.ElementId)
                        val = p.AsElementId().IntegerValue.ToString();
                    else if (p.StorageType == StorageType.Integer)
                        val = p.AsInteger().ToString();
                    else if (p.StorageType == StorageType.String)
                        val = p.AsString();

                    parameterDatas.Add(new ParameterData(p.Definition.Name, val));
                }
                if (e.Location is LocationPoint)
                    locations.Add(new XYZSerializable((e.Location as LocationPoint).Point));
                else if (e.Location is LocationCurve)
                {
#if RELEASE2013
                    locations.Add(new XYZSerializable((e.Location as LocationCurve).Curve.get_EndPoint(0)));
                    locations.Add(new XYZSerializable((e.Location as LocationCurve).Curve.get_EndPoint(1)));
#else
                    locations.Add(new XYZSerializable((e.Location as LocationCurve).Curve.GetEndPoint(0)));
                    locations.Add(new XYZSerializable((e.Location as LocationCurve).Curve.GetEndPoint(1)));
#endif
                }
                else
                {
                    GeometryElement ge = e.get_Geometry(new Options());
                    if (ge != null)
                    {
                        List<Solid> solids = ge.Where(q => !(q is null)).Where(q => q is Solid).Cast<Solid>().ToList();
                        try
                        {
                            locations = solids.Select(q => new XYZSerializable(q.ComputeCentroid())).ToList();
                        }
                        catch
                        { }
                    }
                }

                elementDatas.Add(new ElementData
                {
                    CategoryName = e.Category.Name,
                    ElementId = e.Id.IntegerValue,
                    UniqueId = e.UniqueId,
                    Locations = locations.OrderBy(q => new XYZ(q.X, q.Y, q.Z).DistanceTo(XYZ.Zero)).ToList(),
                    ParameterData = parameterDatas
                });
            }

            ModelData md = new ModelData
            {
                ElementDatas = elementDatas
            };

            string filename = Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(doc.PathName) + ".xml");
            using (TextWriter WriteFileStream = new StreamWriter(filename))
            {
                new XmlSerializer(typeof(ModelData)).Serialize(WriteFileStream, md);
            }

            return filename;
        }

        [Serializable]
        public class XYZSerializable
        {
            public XYZSerializable()
            { }

            public XYZSerializable(XYZ xyz)
            {
                X = xyz.X;
                Y = xyz.Y;
                Z = xyz.Z;
            }

            public double X { get; set; }
            public double Y { get; set; }
            public double Z { get; set; }

        }

        public class ParameterData
        {
            public ParameterData()
            { }

            public ParameterData(string name, string value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; set; }
            public string Value { get; set; }
        }

        public class ModelData
        {
            public List<ElementData> ElementDatas { get; set; }
        }

        public class ElementData
        {
            public string UniqueId { get; set; }
            public string CategoryName { get; set; }
            public int ElementId { get; set; }
            public List<XYZSerializable> Locations { get; set; }
            public List<ParameterData> ParameterData { get; set; }
        }

    }

}
