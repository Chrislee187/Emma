using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Emma.XamlControls
{
    /// <summary>
    /// Interaction logic for MainEmmaToolWindowControl.xaml
    /// </summary>
    public partial class MainEmmaToolWindowControl : UserControl
    {
        public MainEmmaToolWindowControl()
        {
            this.InitializeComponent();
        }

        private void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(
                string.Format(System.Globalization.CultureInfo.CurrentUICulture, "Invoked '{0}'", this.ToString()),
                "Emma");
        }

    }
}
