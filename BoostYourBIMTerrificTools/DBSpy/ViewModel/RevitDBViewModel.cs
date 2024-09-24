using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Autodesk.Revit.DB;

namespace BoostYourBIMTerrificTools.DBSpy
{
    public class RevitDBViewModel
    {
        public RevitDBViewModel(List<BuiltInCategory> bics)
        {
            RevitCategories = new ReadOnlyCollection<RevitCategoryViewModel>(
                (from bic in bics
                 select new RevitCategoryViewModel(bic))
                .ToList());
        }

        public ReadOnlyCollection<RevitCategoryViewModel> RevitCategories { get; }
    }
}