using CotizadorRojoBetabel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace CotizadorRojoBetabel.Views
{
    /// <summary>
    /// Interaction logic for NewDish.xaml
    /// </summary>
    public partial class NewDish : UserControl
    {
        public NewDish()
        {
            InitializeComponent();
        }

        private class IngredientsTable
        {
            public string Name { get; set; }

            public PackageUnit Unit { get; set; }

            public decimal Weight { get; set; }

            public decimal Cost { get; set; }
        }

        private void LoadColumns()
        {

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void ProductTxt_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void LineCmb_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var descriptor = (PropertyDescriptor)e.PropertyDescriptor;
            var header = string.Empty;
            switch (descriptor.DisplayName)
            {
                case "Name":
                    header = "Platillo";
                    break;
                case "Unit":
                    header = "Unidad";
                    break;
                case "Weight":
                    header = "Pso-Cant.";
                    break;
                case "Cost":
                    header = "Costo";
                    break;
            }

            e.Column.Header = header;
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CleanBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ImgBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PortionsTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void PortionsTxt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
