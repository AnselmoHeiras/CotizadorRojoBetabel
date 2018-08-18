using CotizadorRojoBetabel.Controllers;
using CotizadorRojoBetabel.Models;
using Microsoft.Win32;
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
            if (App.HasProducts())
            {
                ParentView.Show_CreateNewDishView();
            }
            else
            {
                ParentView.Show_MessageView("No hay productos registrados\nRegistre productos para poder crear platillos",
                    //affirmative action
                    delegate
                    {
                        ParentView.Show_DishesView();
                    },
                    "Aceptar",
                    //negative action
                    null,
                    null,
                    FontAwesome.WPF.FontAwesomeIcon.ExclamationCircle
                    );
            }
            
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(DishesDgd.SelectedItem is LocalDishes localDishes)) return;
            var dish = _dishesDb.SingleOrDefault(x => x.Id == localDishes.Id);
            ParentView.Show_NewDishView(dish);
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(DishesDgd.SelectedItem is LocalDishes localDishes)) return;
            var dish = _dishesDb.SingleOrDefault(x => x.Id == localDishes.Id);
            try
            {
                using (var db = App.DbFactory.Open())
                {
                    var ingredients = db.Select<Ingredients>().Where(x => x.Dish == dish.Id).ToArray();
                    foreach (var i in ingredients)
                    {
                        db.Delete(i);
                    }
                    db.Delete(dish);
                }
                ParentView.Show_MessageView("El platillo se ha eliminado con éxito",
                    //affirmative action
                    delegate
                    {
                        ParentView.Show_DishesView();
                    },
                    "Aceptar",
                    //negative action
                    null,
                    null,
                    FontAwesome.WPF.FontAwesomeIcon.CheckCircle
                    );
            }
            catch (Exception)
            {
                ParentView.Show_MessageView("Hubo un problema al eliminar el platillo\nComuniquese a soporte técnico",
                    //affirmative action
                    delegate
                    {
                        ParentView.Show_DishesView();
                    },
                    "Aceptar",
                    //negative action
                    null,
                    null,
                    FontAwesome.WPF.FontAwesomeIcon.ExclamationCircle
                    );
            }
        }

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(DishesDgd.SelectedItem is LocalDishes localDishes)) return;
            var dish = _dishesDb.SingleOrDefault(x => x.Id == localDishes.Id);

            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "PDF file (*.pdf)|*.pdf"
            };
            if (saveDialog.ShowDialog() == true)
            {
                var s = saveDialog.FileName;
                var fileCreated = DocumentsCreator.Dish(dish, saveDialog.FileName);
                if (fileCreated)
                {
                    ParentView.Show_MessageView("El documento ha sido creado con éxito",
                        //affirmative action
                        delegate
                        {
                            ParentView.Show_DishesView();
                        },
                        "Aceptar",
                        //negative action
                        null,
                        null,
                        FontAwesome.WPF.FontAwesomeIcon.CheckCircle
                        );
                }
                else
                {
                    ParentView.Show_MessageView("Hubo un problema al crear el documento\nComuniquese a soporte técnico",
                    //affirmative action
                    delegate
                    {
                        ParentView.Show_DishesView();
                    },
                    "Aceptar",
                    //negative action
                    null,
                    null,
                    FontAwesome.WPF.FontAwesomeIcon.ExclamationCircle
                    );
                }
            }
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
