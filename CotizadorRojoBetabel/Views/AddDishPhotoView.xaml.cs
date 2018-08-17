using CotizadorRojoBetabel.Models;
using Microsoft.Win32;
using ServiceStack.OrmLite;
using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for AddDishPhotoView.xaml
    /// </summary>
    public partial class AddDishPhotoView : UserControl
    {
        private Dishes _dish;

        public AddDishPhotoView(Dishes dish)
        {
            _dish = dish;
            InitializeComponent();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {

        }

        private void OnClick_AffirmativeBtn(object sender, RoutedEventArgs e)
        {
            if (AffirmativeBtn.Content as string == "Si")
            {

                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
                };
                if (openFileDialog.ShowDialog() == true)
                {
                    var ImageBytes = File.ReadAllBytes(openFileDialog.FileName);
                    PhotoImg.Source = App.ByteToImage(ImageBytes);
                    Icon.Visibility = Visibility.Hidden;
                    _dish.Draw = ImageBytes;
                    AffirmativeBtn.Content = "Guardar";
                    NegativeBtn.Content = "Recargar";
                }
            }
            else
            {
                using (var db = App.DbFactory.Open())
                {
                    db.Update(_dish);
                }
                ParentView.Show_MessageView("El platillo se ha guardado con éxito",
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
        }

        private void OnClick_NegativeBtn(object sender, RoutedEventArgs e)
        {
            if (NegativeBtn.Content as string == "No")
            {
                ParentView.Show_MessageView("El platillo se ha guardado con éxito",
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
                ParentView.Show_AddDishPhoto(_dish);
            }
            
        }
    }
}
