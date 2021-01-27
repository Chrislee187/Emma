using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Emma.XamlControls.ViewModels;

namespace Emma.XamlControls
{
    public partial class MainEmmaToolWindowControl : UserControl
    {
        private MainEmmaToolWindowViewModel ViewModel => (MainEmmaToolWindowViewModel) DataContext;

        public MainEmmaToolWindowControl()
        {
            InitializeComponent();
            Focusable = true;
            IsTabStop = true;

            // var textBox = SearchByName.FindChild("PART_EditableTextBox",typeof(TextBox)) as TextBox;
            // textBox?.Focus();
            // var textBox = SearchByName.Template.FindName("PART_EditableTextBox",SearchByName) as TextBox;
            // textBox?.Focus();
            // var textBox = this.FindChild("PART_EditableTextBox",typeof(TextBox)) as TextBox;
            // textBox?.Focus();

            SearchByName.Focus();

        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            ShowState();
            ViewModel.Search();
        }

        private void ShowState()
        {
            var msg = $"Member Search: {ViewModel.MemberSearch}\n" +
                   $"Extending Type Search: {ViewModel.ExtendingTypeSearch}\n" +
                   $"Return Type Search: {ViewModel.ReturnTypeSearch}\n" 
                ;
            MessageBox.Show($"{msg}", $"Show State");
        }

        private void MethodsList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var em = (ExtensionMethodViewModel) e.AddedItems[0];
                ViewModel.MethodSelected(em);
                MessageBox.Show($"{em.Name}");
            }
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            ShowState();
        }
    }

    public static class UIChildFinder
    {
        public static DependencyObject FindChild(this DependencyObject reference, string childName, Type childType)
        {
            DependencyObject foundChild = null;
            if (reference != null)
            {   
                int childrenCount = VisualTreeHelper.GetChildrenCount(reference);
                for (int i = 0; i < childrenCount; i++)
                {
                    var child = VisualTreeHelper.GetChild(reference, i);
                    // If the child is not of the request child type child
                    if (child.GetType() != childType)
                    {
                        // recursively drill down the tree
                        foundChild = FindChild(child, childName, childType);
                    }
                    else if (!string.IsNullOrEmpty(childName))
                    {
                        var frameworkElement = child as FrameworkElement;
                        // If the child's name is set for search
                        if (frameworkElement != null && frameworkElement.Name == childName)
                        {
                            // if the child's name is of the request name
                            foundChild = child;
                            break;
                        }
                    }
                    else
                    {
                        // child element found.
                        foundChild = child;
                        break;
                    }
                }
            }
            return foundChild;
        }
    }
}
