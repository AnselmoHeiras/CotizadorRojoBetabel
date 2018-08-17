using CotizadorRojoBetabel.Models;
using FontAwesome.WPF;
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
using System.Windows.Shapes;

namespace CotizadorRojoBetabel.Views
{
    /// <summary>
    /// Interaction logic for ParentView.xaml
    /// </summary>
    public partial class ParentView : Window
    {
        private static ParentView _current;
        public static ParentView Current => _current ?? (_current = new ParentView());

        public static event Action<UserControl> ViewLoaded;
        public static event Action<UserControl> ViewUnloaded;

        public ParentView()
        {
            InitializeComponent();
            ViewLoaded += OnViewLoaded;
            ViewUnloaded += OnViewUnloaded;
        }

        private void OnViewLoaded(UserControl control)
        {
            var type = control.GetType();
        }

        private void OnViewUnloaded(UserControl control)
        {
            if (control == null) return;
            var type = control.GetType();
        }

        internal static void Show_WaitView(string message)
        {
            Current.Dispatcher.Invoke(() => {
                ParentView.Current.ProductCatalogBtn.IsEnabled = false;
                ParentView.Current.DishesCatalogBtn.IsEnabled = false;
                ParentView.Current.ConfigurationBtn.IsEnabled = false;
                var oldView = Current.Transition.Content as UserControl;
                var view = new WaitView(message);
                Current.Transition.Content = view;
                ViewLoaded?.Invoke(view);
                App.Log.Message($"Wait. Message: {message}", "VIEW-LOADED");
                if (!(oldView is WaitView)) ViewUnloaded?.Invoke(oldView);
            });
        }

        internal static void Show_MainView()
        {
            Current.Dispatcher.Invoke(() => {
                ParentView.Current.ProductCatalogBtn.IsEnabled = true;
                ParentView.Current.DishesCatalogBtn.IsEnabled = true;
                ParentView.Current.ConfigurationBtn.IsEnabled = true;
                var oldView = Current.Transition.Content as UserControl;
                var view = new MainView();
                Current.Transition.Content = view;
                ViewLoaded?.Invoke(view);
                App.Log.Message($"MainView", "VIEW-LOADED");
                if (!(oldView is WaitView)) ViewUnloaded?.Invoke(oldView);
            });
        }

        internal static void Show_MessageView(string message, Action affirmativeAction = null, string ABtnContent = null, Action negativeAction = null, string BBtnContent = null, FontAwesomeIcon icon = FontAwesomeIcon.None)
        {
            Current.Dispatcher.Invoke(() => {
                ParentView.Current.ProductCatalogBtn.IsEnabled = false;
                ParentView.Current.DishesCatalogBtn.IsEnabled = false;
                ParentView.Current.ConfigurationBtn.IsEnabled = false;
                var oldView = Current.Transition.Content as UserControl;
                var view = new MessageView(message, affirmativeAction, ABtnContent, negativeAction, BBtnContent, icon);
                Current.Transition.Content = view;
                ViewLoaded?.Invoke(view);
                App.Log.Message($"MessageView, Icon: {icon}, Message: {message}", "VIEW-LOADED");
                if (!(oldView is WaitView)) ViewUnloaded?.Invoke(oldView);
            });
        }

        internal static void Show_ProductsView()
        {
            Show_WaitView("Cargando la lista de productos");

            //TODO select all products from db
            List<Products> products;
            using (var db = App.DbFactory.Open())
            {
                products = db.Select<Products>();
            }

            Current.Dispatcher.Invoke(() => {
                ParentView.Current.ProductCatalogBtn.IsEnabled = true;
                ParentView.Current.DishesCatalogBtn.IsEnabled = true;
                ParentView.Current.ConfigurationBtn.IsEnabled = true;
                var oldView = Current.Transition.Content as UserControl;
                var view = new ProductsView(products);
                Current.Transition.Content = view;
                ViewLoaded?.Invoke(view);
                App.Log.Message($"ProductsView, Products: {products.Serialize(LibreR.Models.Enums.Serializer.OneLine)}", "VIEW-LOADED");
                if (!(oldView is WaitView)) ViewUnloaded?.Invoke(oldView);
            });
        }

        internal static void Show_NewProductView(Products product = null)
        {
            Current.Dispatcher.Invoke(() => {
                ParentView.Current.ProductCatalogBtn.IsEnabled = false;
                ParentView.Current.DishesCatalogBtn.IsEnabled = false;
                ParentView.Current.ConfigurationBtn.IsEnabled = false;
                var oldView = Current.Transition.Content as UserControl;
                var view = new NewProductView(product);
                Current.Transition.Content = view;
                ViewLoaded?.Invoke(view);
                App.Log.Message($"NewProductView, Product: {product.Serialize(LibreR.Models.Enums.Serializer.OneLine)}", "VIEW-LOADED");
                if (!(oldView is WaitView)) ViewUnloaded?.Invoke(oldView);
            });
        }

        internal static void Show_DishesView()
        {
            Show_WaitView("Cargando el catálogo de platillos");

            //TODO select all dishes from db
            List<Dishes> dishes;
            using (var db = App.DbFactory.Open())
            {
                dishes = db.Select<Dishes>();
            }

            Current.Dispatcher.Invoke(() => {
                ParentView.Current.ProductCatalogBtn.IsEnabled = true;
                ParentView.Current.DishesCatalogBtn.IsEnabled = true;
                ParentView.Current.ConfigurationBtn.IsEnabled = true;
                var oldView = Current.Transition.Content as UserControl;
                var view = new DishesView(dishes);
                Current.Transition.Content = view;
                ViewLoaded?.Invoke(view);
                App.Log.Message($"DishesView, Dishes: {dishes.Serialize(LibreR.Models.Enums.Serializer.OneLine)}", "VIEW-LOADED");
                if (!(oldView is WaitView)) ViewUnloaded?.Invoke(oldView);
            });
        }

        internal static void Show_CreateNewDishView()
        {
            Current.Dispatcher.Invoke(() => {
                ParentView.Current.ProductCatalogBtn.IsEnabled = false;
                ParentView.Current.DishesCatalogBtn.IsEnabled = false;
                ParentView.Current.ConfigurationBtn.IsEnabled = false;
                var oldView = Current.Transition.Content as UserControl;
                var view = new CreateNewDish();
                Current.Transition.Content = view;
                ViewLoaded?.Invoke(view);
                App.Log.Message($"CreateNewDishView", "VIEW-LOADED");
                if (!(oldView is WaitView)) ViewUnloaded?.Invoke(oldView);
            });
        }

        internal static void Show_NewDishView(Dishes dish)
        {
            Current.Dispatcher.Invoke(() => {
                ParentView.Current.ProductCatalogBtn.IsEnabled = false;
                ParentView.Current.DishesCatalogBtn.IsEnabled = false;
                ParentView.Current.ConfigurationBtn.IsEnabled = false;
                var oldView = Current.Transition.Content as UserControl;
                var view = new NewDish(dish);
                Current.Transition.Content = view;
                ViewLoaded?.Invoke(view);
                App.Log.Message($"NewDishView. Dish: {dish.Serialize(LibreR.Models.Enums.Serializer.OneLine)}", "VIEW-LOADED");
                if (!(oldView is WaitView)) ViewUnloaded?.Invoke(oldView);
            });
        }

        internal static void Show_AddIngredientsView(Dishes dish)
        {
            Show_WaitView("Cargando las lista de productos");

            //TODO select all products from db
            List<Products> products;
            using (var db = App.DbFactory.Open())
            {
                products = db.Select<Products>();
            }

            Current.Dispatcher.Invoke(() => {
                ParentView.Current.ProductCatalogBtn.IsEnabled = false;
                ParentView.Current.DishesCatalogBtn.IsEnabled = false;
                ParentView.Current.ConfigurationBtn.IsEnabled = false;
                var oldView = Current.Transition.Content as UserControl;
                var view = new AddIngredientsView(dish, products);
                Current.Transition.Content = view;
                ViewLoaded?.Invoke(view);
                App.Log.Message($"AddIngredientsView. Dish: {dish.Serialize(LibreR.Models.Enums.Serializer.OneLine)}\nProducts: {products.Serialize(LibreR.Models.Enums.Serializer.OneLine)}", "VIEW-LOADED");
                if (!(oldView is WaitView)) ViewUnloaded?.Invoke(oldView);
            });
        }

        internal static void Show_AddDishPhoto(Dishes dish)
        {
            Current.Dispatcher.Invoke(() => {
                ParentView.Current.ProductCatalogBtn.IsEnabled = false;
                ParentView.Current.DishesCatalogBtn.IsEnabled = false;
                ParentView.Current.ConfigurationBtn.IsEnabled = false;
                var oldView = Current.Transition.Content as UserControl;
                var view = new AddDishPhotoView(dish);
                Current.Transition.Content = view;
                ViewLoaded?.Invoke(view);
                App.Log.Message($"AddDishPhotoView. Dish: {dish.Serialize(LibreR.Models.Enums.Serializer.OneLine)}", "VIEW-LOADED");
                if (!(oldView is WaitView)) ViewUnloaded?.Invoke(oldView);
            });
        }

        private void ProductCatalogBtn_Click(object sender, RoutedEventArgs e)
        {
            Show_ProductsView();
        }

        private void DishesCatalogBtn_Click(object sender, RoutedEventArgs e)
        {
            Show_DishesView();
        }
    }
}
