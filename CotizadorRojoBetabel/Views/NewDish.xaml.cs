using CotizadorRojoBetabel.Models;
using LibreR.Controllers;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for NewDish.xaml
    /// </summary>
    public partial class NewDish : UserControl
    {
        private decimal _totalCost;
        private decimal _portionCost;
        private decimal _salePrice;
        private Dishes _dish;
        private List<Products> _products;
        private ObservableCollection<IngredientsTable> _ingredientsOC;

        internal decimal TotalCost
        {
            get
            {
                return _totalCost;
            }
            set
            {
                _totalCost = value;
                TotalCostTxt.Text = value.ToString("C");
            }
        }

        internal decimal PortionCost
        {
            get
            {
                return _portionCost;
            }
            set
            {
                _portionCost = value;
                PortionCostTxt.Text = value.ToString("C");
            }
        }

        internal decimal SalePrice
        {
            get
            {
                return _salePrice;
            }
            set
            {
                _salePrice = value;
                SalePriceTxt.Text = value.ToString("C");
            }
        }

        public NewDish(Dishes dish, List<Products> products = null)
        {
            _dish = dish;
            _products = products;
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
            if(_dish.Portions <= 0)
            {
                _ingredientsOC = new ObservableCollection<IngredientsTable>();

                if (_products != null)
                {
                    foreach (var p in _products)
                    {
                        _ingredientsOC.Add(new IngredientsTable
                        {
                            Name = p.Name,
                            Unit = p.Unit,
                            Weight = p.Weight,
                            Cost = p.Cost
                        });
                    }
                }

                IngredientsDgd.ItemsSource = _ingredientsOC;
            }
            else
            {

            }            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            //load CategoryCombo
            var LineEnums = Enum.GetNames(typeof(DishesLine)).ToList();
            var LineItems = new List<string>
            {
                "-Seleccione una opción-"
            };
            foreach (var productCategory in LineEnums)
            {
                LineItems.Add(productCategory);
            }
            LineCmb.ItemsSource = LineItems;

            //initialize values
            NameTxt.Text = _dish.Name;
            PortionsTxt.Text = _dish.Portions.ToString();
            LineCmb.SelectedIndex = 0;
            LoadColumns();
        }

        private void ProductTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            WarningTbk.Visibility = Visibility.Hidden;
        }

        private void LineCmb_GotFocus(object sender, RoutedEventArgs e)
        {
            WarningTbk.Visibility = Visibility.Hidden;
        }

        private void OnAutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            var descriptor = (PropertyDescriptor)e.PropertyDescriptor;
            var header = string.Empty;
            switch (descriptor.DisplayName)
            {
                case "Name":
                    header = "Producto";
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
            ParentView.Show_WaitView("Cargando el catálogo de platillos");

            //TODO slect all dishes from db
            List<Dishes> dishes;
            using (var db = App.DbFactory.Open())
            {
                if (_dish.Portions <= 0)
                {
                    db.Delete(_dish);
                }                    
                dishes = db.Select<Dishes>();
            }

            Dispatcher.SafelyInvoke(() =>
            {
                ParentView.Show_DishesView(dishes);
            });
        }        

        private void PortionsTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PortionsTxt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
