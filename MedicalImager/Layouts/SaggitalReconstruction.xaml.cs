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
    /// Interaction logic for SliceLayout.xaml
    /// </summary>
    public partial class SaggitalReconstruction : Page, StudyIterator
    {
        private int _imageWidth;
        private int _imageHeight;
        private int _numSlices;
        private int _reconstructionPos;
        //Determines if the reconstruction, or the study is being controlled with
        //next and previous buttons
        private bool _reconstructionEnabled;

        public BitmapSource Reconstruction { get; set; }

        public SaggitalReconstruction(IStudy study) : this(study, 0) {}

        public SaggitalReconstruction(IStudy study, int pos)
        {
            InitializeComponent();
            Current = new ObservableCollection<BitmapImage>();
            Reconstruction = null;
            Images = new List<StudyImage>();
            _reconstructionEnabled = false;
            for (int i = 0; i < study.Size(); i++)
            {
                Images.Add(new StudyImage(study[i]));
            }
            DataContext = this;
            //gets a sample image from the study if possible
            BitmapImage sample = Images.Count > 0 ? Images.ElementAt(0).getBitmapImage() : null;
            if(sample ==  null)
            {
                _imageWidth = 0;
                _imageHeight = 0;
                _numSlices = 0;
            }
            else
            {
                _imageWidth = sample.PixelHeight;
                _imageHeight = Images.Count;
                _numSlices = sample.PixelWidth;
            }
            _reconstructionPos = 0;
            createNextImage();
            Position = pos;
        }

        private void createNextImage()
        {
            Console.WriteLine("w: " + _imageWidth + " h: " + _imageHeight);
            PixelFormat pf = PixelFormats.Bgr32;
            int rawStride = (_imageWidth * 32 +7)/8;
            byte[] rawImage = new byte[rawStride * _imageHeight];
            for(int i = 0; i < Images.Count; i++)
            {
                
                Images.ElementAt(i).Source.CopyPixels(new Int32Rect(_numSlices - _reconstructionPos - 1, 0, 1, _imageWidth), 
                    rawImage, 
                    4, 
                    (Images.Count-i-1)*(rawStride));
            }

            // Create a BitmapSource.
            BitmapSource bitmap = BitmapSource.Create(_imageWidth, _imageHeight,
                96, 96, pf, null,
                rawImage, rawStride);
            
            // Create an image element;
            Reconstruction = bitmap;
            //Slice.Width = _imageWidth;
            //Slice.Height = _imageHeight;
            Slice.Source = Reconstruction;
        }


        public bool MovePrev()
        {
            switch(_reconstructionEnabled)
            {
                case true:
                    if (_reconstructionPos < 1)
                        return false;
                    else
                    {
                        _reconstructionPos--;
                        createNextImage();
                        return true;
                    }
                case false:
                    if (Position > 0)
                    {
                        Position--;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

            }
            return false;
        }

        private int _position = -1;

        public int Position
        {
            get
            {
                return _position;
            }
            set
            {
                if(_position != value)
                {
                    if (value < 0 || value >= Images.Count)
                        return;
                    else
                    {
                        _position = value;
                        if (Current.Count == 0)
                        {
                            Current.Add(Images.ElementAt(value).getBitmapImage());
                        }
                        else
                        {
                            Current[0] = Images.ElementAt(value).getBitmapImage();
                        }
                        Orig.Source = Current[0];
                    }
                }
            }
        }

        public string Serialize()
        {
            throw new NotImplementedException();
        }

        public IStudy Study
        {
            get { throw new NotImplementedException(); }
        }

        public ObservableCollection<BitmapImage> Current { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        object System.Collections.IEnumerator.Current
        {
            get { throw new NotImplementedException(); }
        }

        public bool MoveNext()
        {
            switch(_reconstructionEnabled)
            {
                case true:
                        if (_reconstructionPos >= _numSlices - 1)
                        {
                            return false;
                        }
                        else
                        {
                            _reconstructionPos++;
                            createNextImage();
                            return true;
                        }
                case false:
                        if (Position >= Images.Count - 1)
                            return false;
                        else
                        {
                            Position++;
                            return true;
                        }
            }
            return false;
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }


        public List<StudyImage> Images
        {
            get;
            set;
        }


        public void Serialize(System.IO.FileStream stream)
        {
            throw new NotImplementedException();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            _reconstructionEnabled = !_reconstructionEnabled;
        }

    }
}
