using System.Windows;
using System.Windows.Controls;
using Emma.XamlControls.ViewModels;

namespace Emma.XamlControls
{
    public partial class MainEmmaToolWindowControl : UserControl
    {
        private MainEmmaToolWindowViewModel ViewModel => (MainEmmaToolWindowViewModel) DataContext;

        public MainEmmaToolWindowControl()
        {
            InitializeComponent();
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
}
