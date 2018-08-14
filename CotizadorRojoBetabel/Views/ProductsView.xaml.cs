using CotizadorRojoBetabel.Controllers;
using CotizadorRojoBetabel.Models;
using LibreR.Controllers;
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
    /// Interaction logic for ProductsView.xaml
    /// </summary>
    public partial class ProductsView : UserControl
    {
        private ObservableCollection<Products> _productsOc;

        public ProductsView()
        {
            InitializeComponent();
        }

        private void LoadColumns()
        {
            List<Products> products;
            using (var db = App.DbFactory.Open())
            {
               products = db.Select<Products>();
            }

            _productsOc = new ObservableCollection<Products>();
            foreach(var product in products)
            {
                _productsOc.Add(product);
            }

            ProductsDgd.ItemsSource = _productsOc;

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

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            ParentView.Show_NewProductView();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {

            LoadColumns();
        }

        private void UpdateBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(ProductsDgd.SelectedItem is Products product)) return;
            ParentView.Show_NewProductView(product);
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            if (!(ProductsDgd.SelectedItem is Products product)) return;
            try
            {
                using (var db = App.DbFactory.Open())
                {
                  db.Delete(product);
                }
                ParentView.Show_MessageView("El producto se ha eliminado con éxito",
                    //affirmative action
                    delegate
                    {
                        ParentView.Show_ProductsView();
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
                ParentView.Show_MessageView("Hubo un problema al agregar el producto\nComuniquese a soporte técnico",
                    //affirmative action
                    delegate
                    {
                        ParentView.Show_ProductsView();
                    },
                    "Aceptar",
                    //negative action
                    null,
                    null,
                    FontAwesome.WPF.FontAwesomeIcon.ExclamationCircle
                    );
            }
        }

        private void SearchTxt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(SearchTxt.Text == "" || SearchTxt.Text == string.Empty)
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

        private void PrintBtn_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog
            {
                Filter = "PDF file (*.pdf)|*.pdf"
            };
            if (saveDialog.ShowDialog() == true)
            {
                var s = saveDialog.FileName;
                var fileCreated = DocumentsCreator.ProductsList(_productsOc, saveDialog.FileName);
                if (fileCreated)
                {
                    ParentView.Show_MessageView("El documento ha sido creado con éxito",
                        //affirmative action
                        delegate
                        {
                            ParentView.Show_ProductsView();
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
                        ParentView.Show_ProductsView();
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
    }
}
