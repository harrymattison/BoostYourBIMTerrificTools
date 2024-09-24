using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using VCExtensibleStorageExtension;
using VCExtensibleStorageExtension.Attributes;
using VCExtensibleStorageExtension.ElementExtensions;

namespace BoostYourBIMTerrificTools.CreatedHow
{
    public static class Utils
    {
        public const string schemaGuidString = "65C861FF-154B-4869-A587-8003CC2BB587";
        public static List<Tuple<string, ElementId>> createdIds;
        public static EventHandlerWithString ExternalEventWhatChanged;

        public static void ControlledApplication_DocumentChanged(object sender, Autodesk.Revit.DB.Events.DocumentChangedEventArgs e)
        {
            string transactionNames = string.Join(",", e.GetTransactionNames());
            foreach (ElementId id in e.GetAddedElementIds())
            {
                createdIds.Add(new Tuple<string, ElementId> (transactionNames, id));
            }
            if (!createdIds.Any())
                return;

            ExternalEventWhatChanged.Raise(transactionNames);
        }
    }

    [Schema(Utils.schemaGuidString,
        "CreatedHowSchema2",
        ReadAccessLevel = AccessLevel.Application, 
        VendorId = "BYBM", 
        ApplicationGUID = "11a8cb22-6d67-4841-aa1d-176781313b92",
        WriteAccessLevel = AccessLevel.Application)]
    public class CreatedHowEntity : IRevitEntity
    {        
        [Field]
        public string TransactionNames { get; set; }

        [Field]
        public string Date { get; set; }
    }

    public class EventHandlerWithString : RevitEventWrapper<string>
    {
        public override void Execute(UIApplication app, string args)
        {
            try
            {
                if (app.ActiveUIDocument == null)
                    return;

                Document doc = app.ActiveUIDocument.Document;
                //foreach (Group g in Utils.createdIds.Select(q => doc.GetElement(q.Item2)).Where(w => w != null && w is Group).Cast<Group>())
                //{
                //    foreach (ElementId id in g.getmem)
                //    ids.AddRange(g.GetMemberIds());
                //}
                using (Transaction t = new Transaction(doc, "What Changed"))
                {
                    t.Start();
                    foreach (Tuple<string, ElementId> tup in Utils.createdIds)
                    {
                        ElementId id = tup.Item2;
                        Element e = doc.GetElement(id);
                        if (e == null)
                            continue;

                        CreatedHowEntity ent = new CreatedHowEntity
                        {
                            TransactionNames = tup.Item1,
                            Date = DateTime.Now.ToString("MM/dd/yyyy h:mm tt")
                        };

                        e.SetEntity(ent);
                    }
                    t.Commit();
                }
                Utils.createdIds.Clear();
            }
            catch (Exception ex)
            {

            }
        }
    }

    [Transaction(TransactionMode.Manual)]
    public class CreatedHow : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elementSet)
        {
            Document doc = commandData.Application.ActiveUIDocument.Document;
            List<Element> elements = new FilteredElementCollector(doc)
                .WherePasses(new ExtensibleStorageFilter(Guid.Parse(Utils.schemaGuidString))).ToList();
            using (Transaction t = new Transaction(doc, "Created How"))
            {
                t.Start();
                foreach (Element e in elements)
                {
                    CreatedHowEntity ent = e.GetEntity<CreatedHowEntity>();
                    setParam(e, "Created How", ent.TransactionNames);
                    setParam(e, "Created When", ent.Date);
                }
                t.Commit();
            }
            return Result.Succeeded;
        }

        private void setParam(Element e, string parameterName, string value)
        {
            if (value == null)
                return;

            Parameter p = e.LookupParameter(parameterName);
            if (p == null || p.StorageType != StorageType.String)
                return;
            p.Set(value);
        }

    }

}
