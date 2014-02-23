using System;
using System.Collections.Generic;
using System.Drawing;
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
    /// Interaction logic for SingleImageLayout.xaml
    /// </summary>
    public partial class SingleImageLayout : Page
    {
        private StudyIterator Iterator = new SinglePageIterator(new Study());
        public BitmapImage image;
        public SingleImageLayout()
        {
            InitializeComponent();
            image = new BitmapImage(new Uri("file:///C:/Users/John/Desktop/picture-show-flickr-promo.jpg"));
            Image1.Source = image;
        }
    }
}
