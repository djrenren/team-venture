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
    public partial class TwoByTwoImageLayout : Page, StudyIterator
    {
        private IStudy _study;

        public TwoByTwoImageLayout(IStudy study)
        {
            InitializeComponent();
            _study = study;
            Current = new ObservableCollection<BitmapImage>();
            _position = -1;
            Reset();
            DataContext = this;
            // image = new BitmapImage(new Uri("file:///C:/Users/John/Desktop/picture-show-flickr-promo.jpg"));
            //Image1.Source = image;
        }

        public TwoByTwoImageLayout(IStudy study, StudyIterator layout) : this(study)
        {
            Position = layout.Position;
        }

        public ObservableCollection<BitmapImage> Current { get; set; }

        private int _position;
        /// <summary>
        /// Gets and sets the position
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
                    if(value < 0 || value >= (_study.size()))
                    {
                        throw new IndexOutOfRangeException("No images found at position " + value);
                    }
                    else
                    {
                        //_position = 4 * ((int)((value - 1)/4));
                        _position = value - (value % 4);
                        Console.Out.WriteLine("Value given: " + value);
                        Console.Out.WriteLine("New Position: " + _position);
                        
                        if(Current.Count == 0)
                        {
                            for (int i = 0; _position + i < _study.size() && i < 4; i++)
                                Current.Add(_study[_position + i]);
                        }
                        else
                        {
                            for (int i = 0; i < 4; i++)
                                if (_position + i < _study.size())
                                    Current[i] = _study[_position + i];
                                else
                                    Current[i] = null;
                        }
                        
                    }
                }
            }
        }

        public IStudy Study
        {
            get
            {
                return _study;
            }
        }

        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        public void Reset()
        {
            Position = 0;
        }

        public bool MoveNext()
        {
            if(Position + 4 > (_study.size()-1))
            {
                return false;
            }
            else
            {
                Position += 4;
                return true;
            }
        }

        public bool MovePrev()
        {
            if(Position <= 1)
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
