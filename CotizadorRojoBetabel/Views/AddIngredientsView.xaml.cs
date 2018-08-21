using CotizadorRojoBetabel.Models;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for AddIngredientsView.xaml
    /// </summary>
    public partial class AddIngredientsView : UserControl
    {
        private Dishes _dish;
        private List<Ingredients> _ingredients;
        private List<Products> _products;
        private ObservableCollection<Products> _productsOc;

        public AddIngredientsView(Dishes dish, List<Products> products)
        {
            _dish = dish;
            try
            {
                _ingredients = _dish.Ingredients.ToList();
            }
            catch (ArgumentNullException)
            {
                _ingredients = new List<Ingredients>();
            }
            _products = products;
            InitializeComponent();
        }

        private void LoadColumns()
        {
            _productsOc = new ObservableCollection<Products>();
            foreach (var product in _products)
            {
                _productsOc.Add(product);
            }

            ProductsDgd.ItemsSource = _productsOc;

        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadColumns();
        }

        private void ProductTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            WarningTbk.Visibility = Visibility.Hidden;
        }

        private void ProductsDgd_GotFocus(object sender, RoutedEventArgs e)
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
                case "Category":
                    header = "Categoría";
                    break;
                case "Price":
                    header = "Precio";
                    break;
                case "Unit":
                    header = "Unidad";
                    break;
                case "Weight":
                    header = "Peso/Cantidad";
                    break;
                case "Waste":
                    header = "%Merma";
                    break;
                case "Yield":
                    header = "%Rendimiento";
                    break;
                case "Cost":
                    header = "Costo";
                    break;
            }

            e.Column.Header = header;
        }

        private void PortionsTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
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

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var quantityParsed = decimal.TryParse(PortionsTxt.Text, out decimal quantity);

            if (!(ProductsDgd.SelectedItem is Products product))
            {
                WarningTbk.Text = "Seleccione un producto de la lista para agregarlo como ingrediente";
                WarningTbk.Visibility = Visibility.Visible;
            }
            else if (!quantityParsed || quantity <= 0)
            {
                WarningTbk.Text = "Verifique que la cantidad ingresada";
                WarningTbk.Visibility = Visibility.Visible;
            }
            else
            {
                Ingredients ingredient = new Ingredients
                {
                    Dish = _dish.Id,
                    Ingredient = product,
                    Quantity = quantity
                };

                using (var db = App.DbFactory.Open())
                {
                    db.Save(ingredient);
                }
                _ingredients.Add(ingredient);
                _dish.Ingredients = _ingredients.ToArray();
                ParentView.Show_NewDishView(_dish);
            }

        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            ParentView.Show_NewDishView(_dish);
        }

        private void SearchTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTxt.Text == "" || SearchTxt.Text == string.Empty)
            {
                ProductsDgd.ItemsSource = _productsOc;
            }
            else
            {
                var filteredList = from prod in _productsOc
                                   where prod.Name.Contains(SearchTxt.Text)
                                   select prod;

                ProductsDgd.ItemsSource = filteredList;
            }
        }
    }
}
