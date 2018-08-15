using CotizadorRojoBetabel.Models;
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
    /// Interaction logic for DishesView.xaml
    /// </summary>
    public partial class DishesView : UserControl
    {
        private List<Dishes> _dishesDb;
        private ObservableCollection<LocalDishes> _dishesOC;

        public DishesView(List<Dishes> dishesDb)
        {
            _dishesDb = dishesDb;
            InitializeComponent();
        }

        private void LoadColumns()
        {
            _dishesOC = new ObservableCollection<LocalDishes>();
            foreach(var dish in _dishesDb)
            {
                decimal cost = 0;               

                foreach (var ingredient in dish.Ingredients)
                {
                    var productCost = Math.Round(ingredient.Ingredient.Cost * ingredient.Quantity, 2);
                    cost = Math.Round(cost + productCost, 2);
                }

                var earnings = cost * (Config.Current.EarningsPercent / 100);
                var salePrice = Math.Round(cost + earnings, 2);

                _dishesOC.Add(new LocalDishes
                {
                    Id = dish.Id,
                    Name = dish.Name,
                    Cost = cost,
                    SalePrice = salePrice
                });
            }

            DishesDgd.ItemsSource = _dishesOC;
        }

        private class LocalDishes
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public decimal Cost { get; set; }

            public decimal SalePrice { get; set; }
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
                case "Cost":
                    header = "Costo";
                    break;
                case "SalePrice":
                    header = "Precio de Venta";
                    break;
            }

            e.Column.Header = header;
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            ParentView.Show_CreateNewDishView();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SearchTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTxt.Text == "" || SearchTxt.Text == string.Empty)
            {
                DishesDgd.ItemsSource = _dishesOC;
            }
            else
            {
                var filteredList = from dish in _dishesOC
                                   where dish.Name.Contains(SearchTxt.Text)
                                   select dish;

                DishesDgd.ItemsSource = filteredList;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            LoadColumns();
        }
    }
}
