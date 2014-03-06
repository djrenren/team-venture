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
        StudyIterator layout;
        public MainWindow()
        {
            InitializeComponent();
            layout = null;
        }

        private void mnu_Open_Click(object sender, RoutedEventArgs e)
        {
            OpenStudyDialog newDialog = new OpenStudyDialog();
            string filePath = newDialog.openStudy();
            IStudy openedStudy;
            if (filePath == null)
                return;
            openedStudy = new Study(filePath);
            // Code for passing off the Study object (displaying it) goes here
            //layout = new SingleImageLayout(openedStudy);
            layout = new TwoByTwoImageLayout(openedStudy);
            Layout.Navigate(layout);

        }

        private void mnu_Save_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnu_SaveAs_Click(object sender, RoutedEventArgs e)
        {

        }

        private void mnu_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (layout != null)
            {
                layout.MoveNext();
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (layout != null)
            {
                layout.MovePrev();
            }
        }

    }
}
