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
            mnu_Default.IsEnabled = false;

            //check for a default study
            string def = Environment.GetEnvironmentVariable("MedImgDefault", EnvironmentVariableTarget.User);
            if (def != "" && def != null && Directory.Exists(def))
            {
                IStudy study = new Study(def);
                layout = StudyIteratorFactory.Create(study);
                enableButtons();
                updateCount();
                Layout.Navigate(layout);
            }
            else
            {
                layout = null;
                openMenu();
            }
        }

        private void mnu_Open_Click(object sender, RoutedEventArgs e)
        {
            //if a study is being displayed prompt to save first
            if(layout != null)
            {
                bool cancel = promptSave();
                if (cancel)
                    return;
            }
            openMenu();
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

        private void mnu_Default_Click(object sender, RoutedEventArgs e)
        {
            layout.Study.SetDefault();
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
            mnu_Default.IsEnabled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (layout != null)
            {
                e.Cancel = promptSave();
            }
        }

        private void openMenu()
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

        /// <summary>
        /// Prompts the user to save the study state
        /// </summary>
        /// <returns>true if the user selected to cancel their operation</returns>
        private bool promptSave()
        {
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result = MessageBox.Show(
                "Would you like to save your current study state first? Unsaved data may be lost!",
                "Medical Image Viewer",
                button,
                icon);

            // Process message box results 
            switch (result)
            {
                case MessageBoxResult.Yes:
                    layout.Study.Save(layout.Serialize());
                    return false;
                case MessageBoxResult.No:
                    return false;
                case MessageBoxResult.Cancel:
                    return true;
                default: return true;
            }
        }
    }
}
