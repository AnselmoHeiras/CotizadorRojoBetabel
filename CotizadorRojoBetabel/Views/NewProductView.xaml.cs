using CotizadorRojoBetabel.Models;
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
    /// Interaction logic for NewProductView.xaml
    /// </summary>
    public partial class NewProductView : UserControl
    {
        public NewProductView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //load CategoryCombo
            var CategoryEnums = Enum.GetNames(typeof(ProductCategory)).ToList();
            var CategoryItems = new List<string>
            {
                "-Seleccione una opción"
            };
            foreach (var productCategory in CategoryEnums)
            {
                CategoryItems.Add(productCategory);
            }
            CategoryCmb.ItemsSource = CategoryItems;
            CategoryCmb.SelectedIndex = 0;

            //load PackageUnitCombo
            var UnitEnums = Enum.GetNames(typeof(PackageUnit)).ToList();
            var UnitItems = new List<string>
            {
                "-Seleccione una opción"
            };
            foreach (var packageUnit in UnitEnums)
            {
                UnitItems.Add(packageUnit);
            }
            UnitCmb.ItemsSource = UnitItems;
            UnitCmb.SelectedIndex = 0;

            //limit characters on percent fields
            WasteTxt.MaxLength = 3;
            YieldTxt.MaxLength = 3;
        }

        private void ResetFields()
        {
            ProductTxt.Text = string.Empty;
            CategoryCmb.SelectedIndex = 0;
            PriceTxt.Text = string.Empty;
            UnitCmb.SelectedIndex = 0;
            InitialWeightTxt.Text = string.Empty;
            WasteTxt.Text = string.Empty;
            YieldTxt.Text = string.Empty;
            YieldFactorTxt.Text = string.Empty;
            FinalWeightTxt.Text = string.Empty;
            DrainWeightTxt.Text = string.Empty;
            CostTxt.Text = "$0.00";
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CleanBtn_Click(object sender, RoutedEventArgs e)
        {
            ResetFields();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            ParentView.Show_ProductsView();
        }

        private void PriceTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void WasteTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void YieldTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void InitialWeightTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void YieldFactorTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void FinalWeightTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }

        private void DrainWeightTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            bool approvedDecimalPoint = false;

            if (e.Text == ".")
            {
                if (!((TextBox)sender).Text.Contains("."))
                    approvedDecimalPoint = true;
            }

            if (!(char.IsDigit(e.Text, e.Text.Length - 1) || approvedDecimalPoint))
                e.Handled = true;
        }
    }
}
