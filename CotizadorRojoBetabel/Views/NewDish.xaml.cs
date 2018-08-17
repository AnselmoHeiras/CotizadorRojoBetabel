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

        public NewDish(Dishes dish)
        {
            _dish = dish;
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

                if (_dish.Ingredients != null)
                {
                    foreach (var p in _dish.Ingredients)
                    {
                        var ingredient = new IngredientsTable
                        {
                            Name = p.Ingredient.Name,
                            Unit = p.Ingredient.Unit,
                            Weight = p.Quantity,
                            Cost = Math.Round(p.Ingredient.Cost * p.Quantity, 2)
                        };
                        _ingredientsOC.Add(ingredient);
                        TotalCost = TotalCost + ingredient.Cost;
                    }
                }

                IngredientsDgd.ItemsSource = _ingredientsOC;
            }
            else
            {
                _ingredientsOC = new ObservableCollection<IngredientsTable>();

                if (_dish.Ingredients != null)
                {
                    foreach (var p in _dish.Ingredients)
                    {
                        var ingredient = new IngredientsTable
                        {
                            Name = p.Ingredient.Name,
                            Unit = p.Ingredient.Unit,
                            Weight = p.Quantity,
                            Cost = Math.Round(p.Ingredient.Cost * p.Quantity, 2)
                        };
                        _ingredientsOC.Add(ingredient);
                        TotalCost = TotalCost + ingredient.Cost;
                    }
                }

                IngredientsDgd.ItemsSource = _ingredientsOC;
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
            if (_dish.Portions <= 0)
            {
                NameTxt.Text = _dish.Name;
                PortionsTxt.Text = _dish.Portions.ToString();
                LineCmb.SelectedIndex = 0;
            }
            else
            {
                NameTxt.Text = _dish.Name;
                PortionsTxt.Text = _dish.Portions.ToString();
                LineCmb.SelectedValue = _dish.Line.ToString();
                InstructionsTxt.Text = _dish.Instructions;
                NotesTxt.Text = _dish.Notes;
                PortionCost = Math.Round(TotalCost / _dish.Portions, 2);
                SalePrice = Math.Round((PortionCost * (Config.Current.EarningsPercent / 100)) + PortionCost, 2);
            }

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

        private void IngredientsDgd_GotFocus(object sender, RoutedEventArgs e)
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
            var lineParsed = Enum.TryParse(LineCmb.Text, out DishesLine dishLine);

            if (NameTxt.Text == "" || NameTxt.Text == " " || NameTxt.Text == string.Empty)
            {
                WarningTbk.Text = "Ingrese el nombre del platillo antes de guardarlo";
                WarningTbk.Visibility = Visibility.Visible;
            }
            else if (TotalCost <= 0)
            {
                WarningTbk.Text = "Ingrese ingredientes antes de guardar el platillo";
                WarningTbk.Visibility = Visibility.Visible;
            }
            else if (PortionCost <= 0)
            {
                WarningTbk.Text = "Ingrese el número de porciones antes de guardar el platillo";
                WarningTbk.Visibility = Visibility.Visible;
            }
            else if (!lineParsed || LineCmb.Text == "-Seleccione una opción-")
            {
                WarningTbk.Text = "Verifique que haya seleccionado la linea a la que pertenece el platillo";
                WarningTbk.Visibility = Visibility.Visible;
            }
            else
            {
                _dish.Instructions = InstructionsTxt.Text;
                _dish.Name = NameTxt.Text;
                _dish.Portions = Int32.Parse(PortionsTxt.Text);
                _dish.Line = dishLine;
                _dish.Notes = NotesTxt.Text;

                using (var db = App.DbFactory.Open())
                {
                    db.Update(_dish);
                }
                ParentView.Show_AddDishPhoto(_dish);
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            ParentView.Show_WaitView("Cancelando platillo");
            List<Ingredients> ingredientsList = new List<Ingredients>();

            if (_dish.Ingredients != null)
            {
                ingredientsList = _dish.Ingredients.ToList();
            }

            
            var selectedIngredients = ingredientsList.Where(x => x.Dish == _dish.Id);

            using (var db = App.DbFactory.Open())
            {
                foreach(var ingredient in selectedIngredients)
                {
                    db.Delete(ingredient);
                }
                _dish.Ingredients = null;
                db.Delete(_dish);
            }

            ParentView.Show_DishesView();
        }        

        private void PortionsTxt_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void PortionsTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TotalCost > 0)
            {
             var portionsParsed = decimal.TryParse(PortionsTxt.Text, out decimal portions);
                if (portionsParsed && portions > 0)
                {
                    PortionCost = Math.Round(TotalCost / portions, 2);
                    SalePrice = Math.Round((PortionCost * (Config.Current.EarningsPercent / 100)) + PortionCost, 2);
                    WarningTbk.Visibility = Visibility.Hidden;

                }
                else
                {
                    WarningTbk.Text = "Verifique que la porción ingresada sea un número mayor a 0";
                    WarningTbk.Visibility = Visibility.Visible;
                }                
            }
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            ParentView.Show_AddIngredientsView(_dish);
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(IngredientsDgd.SelectedItem is IngredientsTable ingredient)) return;

            List<Ingredients> ingredientsList = _dish.Ingredients.ToList();
            var selectedIngredient = ingredientsList.SingleOrDefault(x => x.Dish == _dish.Id && x.Ingredient.Name == ingredient.Name);
            ingredientsList.Remove(selectedIngredient);
            _dish.Ingredients = ingredientsList.ToArray();

            using (var db = App.DbFactory.Open())
            {
                db.Delete(selectedIngredient);
            }

            _ingredientsOC.Remove(ingredient);
            foreach (var p in _ingredientsOC)
            {
                TotalCost = TotalCost + p.Cost;
            }
        }
    }
}
