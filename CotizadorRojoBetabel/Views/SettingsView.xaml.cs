using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace CotizadorRojoBetabel.Views
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView : UserControl
    {
        public SettingsView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var config = Models.Config.Current;
            EarningsTxt.Text = config.EarningsPercent.ToString();
        }

        private void EarningsTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            var earningsParsed = Int32.TryParse(EarningsTxt.Text, out int earnings);
            if (earningsParsed)
            {
                Models.Config.Current.EarningsPercent = earnings;
                Models.Config.Current.Save();
            }
        }

        private void EarningsTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
