using CotizadorRojoBetabel.Models;
using LibreR.Controllers;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for CreateNewDish.xaml
    /// </summary>
    public partial class CreateNewDish : UserControl
    {
        public CreateNewDish()
        {
            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {

        }

        private void OnClick_AcceptBtn(object sender, RoutedEventArgs e)
        {
            if (NameTxt.Text == "" || NameTxt.Text == " " || NameTxt.Text == string.Empty)
            {
                WarningText.Visibility = Visibility.Visible;
            }
            else
            {
                ParentView.Show_WaitView("Creando platillo");
                Dishes dish;
                using (var db = App.DbFactory.Open())
                {
                    db.Save(new Dishes
                    {
                        Name = NameTxt.Text
                    });
                    dish = db.Select<Dishes>().Where(x => x.Name == NameTxt.Text).FirstOrDefault();
                }
                ParentView.Show_NewDishView(dish);
            }
        }

        private void OnClick_CancelBtn(object sender, RoutedEventArgs e)
        {
            ParentView.Show_WaitView("Cargando el catálogo de platillos");

            //TODO slect all dishes from db
            List<Dishes> dishes;
            using (var db = App.DbFactory.Open())
            {
                dishes = db.Select<Dishes>();
            }

            Dispatcher.SafelyInvoke(() =>
            {
                ParentView.Show_DishesView(dishes);
            });
        }

        private void NameTxt_GotFocus(object sender, RoutedEventArgs e)
        {
            WarningText.Visibility = Visibility.Hidden;
        }
    }
}
