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

namespace MedicalImager
{
    /// <summary>
    /// Interaction logic for WindowDialog.xaml
    /// </summary>
    public partial class WindowDialog : Window
    {
        public WindowDialog()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Min = Convert.ToInt32(MinVal.Text);
                Max = Convert.ToInt32(MaxVal.Text);
            } catch (InvalidCastException){this.Close(); return;}
            catch (FormatException) { this.Close(); return; }

            if (Min < 0 || Max > 255)
            {
                this.Close();
                return;
            }
            if (Min >= Max)
            {
                this.Close();
                return;
            }
            (new Commands.WindowImagesCom(Command.invoker.Study.Layout, 
                Min, 
                Max,
                Command.invoker.Study.Layout.Images)).Execute();
            this.Close();
        }

        public int Min { get; set; }
        public int Max { get; set; }
        public StudyLayout Layout {get; set; }
        public List<VirtualImage> List { get; set; }
    }
}
