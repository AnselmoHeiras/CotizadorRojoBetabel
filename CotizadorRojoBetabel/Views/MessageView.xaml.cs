using FontAwesome.WPF;
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
    /// Interaction logic for MessageView.xaml
    /// </summary>
    public partial class MessageView : UserControl
    {
        private string _message;
        private string _aBtnContent;
        private string _bBtnContent;
        private FontAwesomeIcon _icon;
        private Action _affirmativeAction;
        private Action _negativeAction;

        public MessageView(string message, Action affirmativeAction = null, string ABtnContent = null, Action negativeAction = null, string BBtnContent = null, FontAwesomeIcon icon = FontAwesomeIcon.None)
        {
            _message = message;
            _aBtnContent = ABtnContent;
            _bBtnContent = BBtnContent;
            _icon = icon;
            _affirmativeAction = affirmativeAction;
            _negativeAction = negativeAction;

            InitializeComponent();
        }

        private void OnClick_AffirmativeBtn(object sender, RoutedEventArgs e)
        {
            _affirmativeAction.Invoke();
        }

        private void OnClick_NegativeBtn(object sender, RoutedEventArgs e)
        {
            _negativeAction.Invoke();
        }

        private void UserControl_Initialized(object sender, EventArgs e)
        {
            MessageText.Text = _message;

            if (_aBtnContent != null)
            {
                AffirmativeBtn.Content = _aBtnContent;
            }

            if (_bBtnContent != null)
            {
                NegativeBtn.Content = _bBtnContent;
            }

            if (_icon != FontAwesomeIcon.None)
            {
                Icon.Icon = _icon;
                Icon.Visibility = Visibility.Visible;
            }

            if (_affirmativeAction != null || _negativeAction != null)
            {
                ButtonsGrd.Visibility = Visibility.Visible;
                if (_affirmativeAction != null && _negativeAction == null)
                {
                    NegativeBtn.Visibility = Visibility.Collapsed;
                }
                if (_affirmativeAction == null && _negativeAction != null)
                {
                    AffirmativeBtn.Visibility = Visibility.Collapsed;
                }
            }
        }
    }
}
