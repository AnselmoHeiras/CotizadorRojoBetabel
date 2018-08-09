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
                var oldView = Current.Transition.Content as UserControl;
                var view = new MainView();
                Current.Transition.Content = view;
                ViewLoaded?.Invoke(view);
                App.Log.Message($"MainView", "VIEW-LOADED");
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

        private void ProductCatalogBtn_Click(object sender, RoutedEventArgs e)
        {
            Show_ProductsView();
        }
    }
}
