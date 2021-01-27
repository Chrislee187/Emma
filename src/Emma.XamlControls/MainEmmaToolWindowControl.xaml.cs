using System.Windows;
using System.Windows.Controls;
using Emma.Core;

namespace Emma.XamlControls
{
    /// <summary>
    /// Interaction logic for MainEmmaToolWindowControl.xaml
    /// </summary>
    public partial class MainEmmaToolWindowControl : UserControl
    {
        private MainEmmaToolWindowViewModel ViewModel => (MainEmmaToolWindowViewModel) DataContext;

        public MainEmmaToolWindowControl()
        {
            this.InitializeComponent();
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            
            MessageBox.Show($"Search clicked: {SearchDebug()}",
                $"{sender.ToString()}");
        }

        private string SearchDebug()
        {
            return $"Member Search: {ViewModel.MemberSearch}\n" +
                   $"Extending Type Search: {ViewModel.ExtendingTypeSearch}\n" +
                   $"Return Type Search: {ViewModel.ReturnTypeSearch}\n"
                ;
        }


        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                var item = (ExtensionMethod) e.AddedItems[0];
                ViewModel.SignatureSelected(item);
                MessageBox.Show($"Selected: {item.Name}",
                    $"{sender.ToString()}");
            }
        }
    }
}
