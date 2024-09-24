using Autodesk.Revit.DB;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace TreeViewWithCheckBoxes
{
    public class FooViewModel : INotifyPropertyChanged
    {
        #region Data

        bool? _isChecked = false;
        FooViewModel _parent;

        #endregion // Data

        #region CreateFoos

        public static List<FooViewModel> CreateFoos()
        {
            Document doc = BoostYourBIMTerrificTools.Utils.doc;

            List<FooViewModel> listFoo = new List<FooViewModel>();

            foreach (Category cat in doc.Settings.Categories.Cast<Category>().OrderBy(q => q.Name))
            {
                FooViewModel thisCat = new FooViewModel(cat);
                List<FooViewModel> fooList = new List<FooViewModel>();
                foreach (Category subcat in cat.SubCategories.Cast<Category>().OrderBy(q => q.Name))
                {
                    fooList.Add(new FooViewModel(subcat));
                }
                if (fooList.Count > 0)
                {
                    thisCat.Children.AddRange(fooList);
                }
                listFoo.Add(thisCat);
                thisCat.Initialize();                
            }
            return listFoo.OrderBy(q => q.Name).ToList();
        }

        FooViewModel(Category cat)
        {
            Name = cat.Name;
            Id = cat.Id;
            Children = new List<FooViewModel>();
        }

        void Initialize()
        {
            foreach (FooViewModel child in this.Children)
            {
                child._parent = this;
                child.Initialize();
            }
        }

        #endregion // CreateFoos

        #region Properties

        public List<FooViewModel> Children { get; private set; }

        public bool IsInitiallySelected { get; private set; }

        public string Name { get; private set; }
        public ElementId Id { get; private set; }

        #region IsChecked

        /// <summary>
        /// Gets/sets the state of the associated UI toggle (ex. CheckBox).
        /// The return value is calculated based on the check state of all
        /// child FooViewModels.  Setting this property to true or false
        /// will set all children to the same check state, and setting it 
        /// to any value will cause the parent to verify its check state.
        /// </summary>
        public bool? IsChecked
        {
            get { return _isChecked; }
            set { SetIsChecked(value, true, true); }
        }

        void SetIsChecked(bool? value, bool updateChildren, bool updateParent)
        {
            if (value == _isChecked)
                return;

            _isChecked = value;

            if (updateChildren && _isChecked.HasValue)
                Children.ForEach(c => c.SetIsChecked(_isChecked, true, false));

            if (updateParent && _parent != null)
                _parent.VerifyCheckState();

            OnPropertyChanged("IsChecked");
        }

        void VerifyCheckState()
        {
            bool? state = null;
            for (int i = 0; i < Children.Count; ++i)
            {
                bool? current = Children[i].IsChecked;
                if (i == 0)
                {
                    state = current;
                }
                else if (state != current)
                {
                    state = null;
                    break;
                }
            }
            SetIsChecked(state, false, true);
        }

        #endregion // IsChecked

        #endregion // Properties

        #region INotifyPropertyChanged Members

        void OnPropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}