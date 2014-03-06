using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public partial class SingleImageLayout : Page, StudyIterator
    {
        private IStudy _study;

        public SingleImageLayout(IStudy study)
        {
            InitializeComponent();
            DataContext = this;
            _study = study;
            Reset();
            // image = new BitmapImage(new Uri("file:///C:/Users/John/Desktop/picture-show-flickr-promo.jpg"));
            //Image1.Source = image;
        }

        public ObservableCollection<BitmapImage> Current { get; set; }

        private int _position;
        /// <summary>
        /// Gets and sets the position. The position is 1 indexed
        /// </summary>
        public int Position
        {
            get
            {
                return _position;
            }

            set
            {
                if(value != _position)
                {
                    if(value < 0 || value > _study.size())
                    {
                        throw new IndexOutOfRangeException("No images found at position " + value);
                    }
                    else
                    {
                        Current.Clear();
                        Current.Add(_study[value]);
                    }
                }
            }
        }

        //public BitmapImage image;

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Reset()
        {
            if (Current != null)
            {
                Current.Clear();
            }
            Position = 0;
        }

        public bool MoveNext()
        {
            if(Position >= _study.size())
            {
                return false;
            }
            else
            {
                Position++;
                return true;
            }
        }

        public bool MovePrev()
        {
            if(Position < 1)
            {
                return false;
            }
            else
            {
                Position--;
                return true;
            }
        }

        public void Dispose()
        {

        }
    }
}
