using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class MainWindow : Window, Driver
    {
        StudyLayout layout;
        public MainWindow()
        {
            Command.invoker = this;
            CommandStack = new Stack<Command>();

            InitializeComponent();
            mnu_View.IsEnabled = false;
            mnu_Save.IsEnabled = false;
            mnu_SaveAs.IsEnabled = false;
            mnu_Default.IsEnabled = false;
            mnu_Window.IsEnabled = false;
            //check for a default study
            string def = //Environment.GetEnvironmentVariable("MedImgDefault", EnvironmentVariableTarget.User);
                null;
            if (def != "" && def != null && Directory.Exists(def))
            {
                IStudy study = new LocalStudy(def);
                EnableOperations();
                updateCount();
                Layout.Navigate(layout);
            }
            else
            {
                layout = null;
                //openMenu();
                this.Loaded += MainWindow_Loaded;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            (new Commands.LoadStudyCom(layout)).Execute();
        }

        /// <summary>
        /// warns the user to save state then opens the study browser to
        /// select a study to open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnu_Open_Click(object sender, RoutedEventArgs e)
        {
            /*
            //if a study is being displayed prompt to save first
            if(layout != null)
            {
                bool cancel = promptSave();
                if (cancel)
                    return;
            }
            openMenu();
             */
            (new Commands.LoadStudyCom(layout)).Execute();
        }

        private void mnu_Save_Click(object sender, RoutedEventArgs e)
        {
            //layout.Study.Save(layout.Serialize());
            (new Commands.SaveCom(layout, Commands.SaveCom.SaveType.Save)).Execute();
        }

        /// <summary>
        /// Opens a folder browser to allow the user to select a directory
        /// to save into. The current study is then switched to the new directory
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnu_SaveAs_Click(object sender, RoutedEventArgs e)
        {
            /*
            StudyBrowserDialog open = new StudyBrowserDialog();
            string path = open.saveStudy();

            if(path == null)
            {
                return;
            }

            layout.Study.Save(new Uri(path), layout.Serialize());
            IStudy copy = new Study(path);
            layout = StudyIteratorFactory.Create(copy);
            Layout.Navigate(layout);
             */
            (new Commands.SaveCom(layout, Commands.SaveCom.SaveType.SaveAs)).Execute();
        }

        private void mnu_Default_Click(object sender, RoutedEventArgs e)
        {
            //layout.Study.SetDefault();
            (new Commands.SetDefaultCom(layout)).Execute();
        }

        private void mnu_Exit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void mnu_WindowPrompt(object sender, RoutedEventArgs e) 
        { 
            Commands.WindowImagesCom.PromptAndCreate();
        }
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (layout != null)
            {
                layout.MoveNext();
                updateCount();
            }
             */
            (new Commands.StepForwardCom(layout)).Execute();
        }

        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (layout != null)
            {
                layout.MovePrev();
                updateCount();
            }
             */
            (new Commands.StepBackwardCom(layout)).Execute();
        }

        /// <summary>
        /// Switches to the SingleImageLayout if possible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnu_Single_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (layout != null && !layout.GetType().Equals(typeof(SingleImageLayout)))
            {
                layout = new SingleImageLayout(layout.Study, layout);
                Layout.Navigate(layout);
                updateCount();
            }
             */
            (new Commands.SetLayoutCom(layout, typeof(SingleImageLayout))).Execute();

        }

        /// <summary>
        /// Switches to the TwoByTwoImageLayout if possible
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mnu_TwoByTwo_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (layout != null && !layout.GetType().Equals(typeof(TwoByTwoImageLayout)))
            {
                layout = new TwoByTwoImageLayout(layout.Study, layout);
                Layout.Navigate(layout);
                updateCount();
            }
             */
            (new Commands.SetLayoutCom(layout, typeof(TwoByTwoImageLayout))).Execute();
        }

        /// <summary>
        /// Updates the label denoting the position in the study
        /// </summary>
        private void updateCount()
        {
            CountLabel.Content = "Position: " + (layout.Position + 1);
        }

        /// <summary>
        /// Enables buttons that should only be clicked when an active study is available
        /// </summary>
        public void EnableOperations()
        {
            mnu_Save.IsEnabled = true;
            mnu_SaveAs.IsEnabled = true;
            mnu_View.IsEnabled = true;
            mnu_Default.IsEnabled = true;
            mnu_Window.IsEnabled = true;
        }

        /// <summary>
        /// Called whenever the window is closing. Gives the user a chance to cancel or save
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            /*
            if (layout != null)
            {
                e.Cancel = promptSave();
            }
             */
        }

        /*
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
         */

        private void mnu_Coronal_Click(object sender, RoutedEventArgs e)
        {
            /*
            if (layout != null && !layout.GetType().Equals(typeof(CoronalReconstruction)))
            {
                layout = new CoronalReconstruction(layout.Study);
                Layout.Navigate(layout);
                updateCount();
            }
             */
            (new Commands.SetLayoutCom(layout, typeof(CoronalReconstruction))).Execute();
        }

        public IStudy Study
        {
            get;
            set;
        }

        public void Navigate(StudyLayout newLayout)
        {
            Layout.Navigate(newLayout);
        }

        public Stack<Command> CommandStack
        {
            get;
            set;
        }

        private void mnu_Saggital_Click(object sender, RoutedEventArgs e)
        {
            (new Commands.SetLayoutCom(layout, typeof(SaggitalReconstruction))).Execute();
        }

        public void UpdateCount()
        {
            CountLabel.Content = "" + (this.Study.Layout.Position + 1) + " / " + this.Study.Size();
        }

        private void mnu_Window_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
