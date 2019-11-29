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

namespace PartyPlanner.Wpf
{
    public partial class MainWindow
    {
        void ToonMelding(string melding, bool isSucces = false)
        {
            tbkFeedBack.Visibility = Visibility.Visible;
            tbkFeedBack.Text = melding;
            tbkFeedBack.Background = isSucces == true ?
                Brushes.Green :
                Brushes.Red;
        }

    }
}
