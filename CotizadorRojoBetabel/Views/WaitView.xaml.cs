using System;
using System.Collections.Generic;
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
    /// Interaction logic for WaitView.xaml
    /// </summary>
    public partial class WaitView : UserControl
    {
        private Bindings _bindings;
        public string MessageText { get => _bindings.MessageText; set => _bindings.MessageText = value; }

        public WaitView(string message)
        {
            InitializeComponent();
            DataContext = _bindings = new Bindings();
            MessageText = message;
        }

        private class Bindings : INotifyPropertyChanged
        {
            private string _messageText;

            public event PropertyChangedEventHandler PropertyChanged;

            public string MessageText
            {
                get => _messageText;
                set
                {
                    _messageText = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MessageText"));
                }
            }
        }
    }
}
