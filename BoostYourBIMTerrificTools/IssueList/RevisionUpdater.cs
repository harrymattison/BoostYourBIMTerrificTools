using Autodesk.Revit.DB;
using Autodesk.Revit.UI.Events;
using System;
using System.Linq;

namespace BoostYourBIMTerrificTools.IssueList
{
    public static class Utils
    {
        public static bool haveSetRevisionParameters;
        public static void Application_Idling(object sender, IdlingEventArgs e)
        {
            haveSetRevisionParameters = false;
        }
    }

    public class RevisionWatcher : IUpdater
    {
        private static AddInId _appId;
        private static UpdaterId _updaterId;

        public RevisionWatcher(AddInId id)
        {
            _appId = id;
            _updaterId = new UpdaterId(_appId, new Guid("fafbf6b2-5c09-42d1-97c1-d1b4eb593eff"));
        }

        public void Execute(UpdaterData data)
        {
            // not sure why setting the description to "•" is trigggering a change to Revision elements, but this is a workaround
            if (Utils.haveSetRevisionParameters)
                return;

            var doc = data.GetDocument();
            var revisions = new FilteredElementCollector(doc).OfClass(typeof(Revision))
                .Cast<Revision>();

            foreach (var sheet in new FilteredElementCollector(doc).OfClass(typeof(ViewSheet)).Cast<ViewSheet>())
            {
                foreach (var revision in revisions)
                {
                    SetDescription(sheet, revision, "");
                }
            }

            foreach (var revisionCloud in new FilteredElementCollector(doc).OfClass(typeof(RevisionCloud))
                .Cast<RevisionCloud>())
            {
                var revision = doc.GetElement(revisionCloud.RevisionId) as Revision;
                foreach (var sheetid in revisionCloud.GetSheetIds())
                {
                    var sheet = doc.GetElement(sheetid) as ViewSheet;
                    SetDescription(sheet, revision, "•");
                }
            }
            Utils.haveSetRevisionParameters = true;
        }

        private void SetDescription(ViewSheet sheet, Revision revision, string s)
        {
            SetParameter(sheet.LookupParameter(revision.Description), s);
        }

        private void SetParameter(Parameter p, string s)
        {
            if (p == null || p.StorageType != StorageType.String)
                return;
            p.Set(s);
        }

        public string GetAdditionalInformation()
        {
            return "";
        }

        public ChangePriority GetChangePriority()
        {
            return ChangePriority.FloorsRoofsStructuralWalls;
        }

        public UpdaterId GetUpdaterId()
        {
            return _updaterId;
        }

        public string GetUpdaterName()
        {
            return "RevisionWatcher";
        }
    }
}