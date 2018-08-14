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
            Current.Dispatcher.Invoke(() => {
                var oldView = Current.Transition.Content as UserControl;
                var view = new ProductsView();
                Current.Transition.Content = view;
                ViewLoaded?.Invoke(view);
                App.Log.Message($"ProductsView", "VIEW-LOADED");
                if (!(oldView is WaitView)) ViewUnloaded?.Invoke(oldView);
            });
        }

        internal static void Show_NewProductView(Products product = null)
        {
            Current.Dispatcher.Invoke(() => {
                var oldView = Current.Transition.Content as UserControl;
                var view = new NewProductView(product);
                Current.Transition.Content = view;
                ViewLoaded?.Invoke(view);
                App.Log.Message($"NewProductView, Product: {product.Serialize(LibreR.Models.Enums.Serializer.OneLine)}", "VIEW-LOADED");
                if (!(oldView is WaitView)) ViewUnloaded?.Invoke(oldView);
            });
        }

        internal static void Show_DishesView(List<Dishes> dishes)
        {
            Current.Dispatcher.Invoke(() => {
                var oldView = Current.Transition.Content as UserControl;
                var view = new DishesView(dishes);
                Current.Transition.Content = view;
                ViewLoaded?.Invoke(view);
                App.Log.Message($"DishesView, Dishes: {dishes.Serialize(LibreR.Models.Enums.Serializer.OneLine)}", "VIEW-LOADED");
                if (!(oldView is WaitView)) ViewUnloaded?.Invoke(oldView);
            });
        }

        internal static void Show_NewDishView()
        {
            Current.Dispatcher.Invoke(() => {
                var oldView = Current.Transition.Content as UserControl;
                var view = new NewDish();
                Current.Transition.Content = view;
                ViewLoaded?.Invoke(view);
                App.Log.Message($"NewDishView", "VIEW-LOADED");
                if (!(oldView is WaitView)) ViewUnloaded?.Invoke(oldView);
            });
        }

        private void ProductCatalogBtn_Click(object sender, RoutedEventArgs e)
        {
            Show_ProductsView();
        }

        private void DishesCatalogBtn_Click(object sender, RoutedEventArgs e)
        {
            Show_WaitView("Cargando el catálogo de platillos");

            //TODO slect all dishes from db
            List<Dishes> dishes;
            using (var db = App.DbFactory.Open())
            {
                dishes = db.Select<Dishes>();
            }

            Dispatcher.SafelyInvoke(()=>
            {
                Show_DishesView(dishes);
            });
        }
    }
}
