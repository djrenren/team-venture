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

namespace MedicalImager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void mnu_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenStudyDialog newDialog = new OpenStudyDialog();
        }

        private void mnu_Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnu_SaveAs_Click(object sender, RoutedEventArgs e)
        {

        }

    }
}
