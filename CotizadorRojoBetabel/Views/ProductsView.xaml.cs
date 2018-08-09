using CotizadorRojoBetabel.Models;
using LibreR.Controllers;
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
                case "InitialWeight":
                    header = "PesoInicial";
                    break;
                case "Waste":
                    header = "%Merma";
                    break;
                case "Yield":
                    header = "%Rendimiento";
                    break;
                case "YieldFactor":
                    header = "FactorRend";
                    break;
                case "FinalWeight":
                    header = "PesoFinal";
                    break;
                case "DrainWeight":
                    header = "PesoDrenado";
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
    }
}
