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
            mnu_View.IsEnabled = false;
            mnu_Save.IsEnabled = false;
            mnu_SaveAs.IsEnabled = false;
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
            layout = StudyIteratorFactory.Create(openedStudy);
            enableButtons();
            Layout.Navigate(layout);
            updateCount();

        }

        private void mnu_Save_Click(object sender, RoutedEventArgs e)
        {
            layout.Study.Save(layout.Serialize());
        }

        private void mnu_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            OpenStudyDialog open = new OpenStudyDialog();
            string path = open.saveStudy();

            if(path == null)
            {
                return;
            }

            layout.Study.Save(new Uri(path), layout.Serialize());
            IStudy copy = new Study(path);
            layout = StudyIteratorFactory.Create(copy);
            Layout.Navigate(layout);
        }

        private void mnu_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            if (layout != null)
            {
                layout.MoveNext();
                updateCount();
            }
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (layout != null)
            {
                layout.MovePrev();
                updateCount();
            }
        }

        private void mnu_Single_Click(object sender, RoutedEventArgs e)
        {
            if (layout != null && !layout.GetType().Equals(typeof(SingleImageLayout)))
            {
                layout = new SingleImageLayout(layout.Study, layout);
                Layout.Navigate(layout);
                updateCount();
            }
        }

        private void mnu_TwoByTwo_Click(object sender, RoutedEventArgs e)
        {
            if (layout != null && !layout.GetType().Equals(typeof(TwoByTwoImageLayout)))
            {
                layout = new TwoByTwoImageLayout(layout.Study, layout);
                Layout.Navigate(layout);
                updateCount();
            }
        }

        private void updateCount()
        {
            CountLabel.Content = "Position: " + (layout.Position + 1);
        }

        private void enableButtons()
        {
            mnu_Save.IsEnabled = true;
            mnu_SaveAs.IsEnabled = true;
            mnu_View.IsEnabled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (layout != null)
            {
                MessageBoxButton button = MessageBoxButton.YesNoCancel;
                MessageBoxImage icon = MessageBoxImage.Warning;
                MessageBoxResult result = MessageBox.Show("Do you wish to save before exiting?",
                    "Medical Image Viewer",
                    button,
                    icon);

                // Process message box results 
                switch (result)
                {
                    case MessageBoxResult.Yes:
                        layout.Study.Save(layout.Serialize());
                        break;
                    case MessageBoxResult.No:
                        break;
                    case MessageBoxResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }
    }
}
