using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace TreeViewWithCheckBoxes
{
    public partial class Window : UserControl
    {
        public Window()
        {
            InitializeComponent();

            CommandBindings.Add(
                new CommandBinding(
                    ApplicationCommands.Undo,
                    (sender, e) => // Execute
                    {                        
                        e.Handled = true;
                        foreach (FooViewModel vm in tree.Items.Cast<FooViewModel>())
                        {
                            vm.IsChecked = false;
                            foreach (FooViewModel child in vm.Children)
                            {
                                child.IsChecked = false;
                            }
                        }
                        tree.Focus();
                    },
                    (sender, e) => // CanExecute
                    {
                        e.Handled = true;
                        e.CanExecute = true;
                    }));

            tree.Focus();
        }

    }
}