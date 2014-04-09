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
    public partial class CoronalReconstruction : Page, StudyIterator
    {
        private int _imageWidth;
        private int _imageHeight;
        private int _numSlices;
        private int _reconstructionPos;
        //Determines if the reconstruction, or the study is being controlled with
        //next and previous buttons
        private bool _reconstructionEnabled;

        public BitmapSource Reconstruction { get; set; }

        public CoronalReconstruction(IStudy study) : this(study, 0) {}

        public CoronalReconstruction(IStudy study, int pos)
        {
            InitializeComponent();
            Current = new ObservableCollection<BitmapImage>();
            Reconstruction = null;
            Images = new List<StudyImage>();
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
                _imageWidth = sample.PixelWidth;
                _imageHeight = Images.Count;
                _numSlices = sample.PixelHeight;
            }
            Position = pos;
        }

        private void createNextImage()
        {
            Console.WriteLine("w: " + _imageWidth + " h: " + _imageHeight);
            PixelFormat pf = PixelFormats.Bgr32;
            int rawStride = (_imageWidth * 5);
            byte[] rawImage = new byte[rawStride * _imageHeight];
            for(int i = 0; i < Images.Count; i++)
            {
                Images.ElementAt(i).Source.CopyPixels(new Int32Rect(0, _numSlices - Position - 1, _imageWidth, 1), 
                    rawImage, 
                    rawStride, 
                    (Images.Count-i-1)*(rawStride));
            }

            // Create a BitmapSource.
            BitmapSource bitmap = BitmapSource.Create(_imageWidth, _imageHeight,
                96, 96, pf, null,
                rawImage, rawStride);

            // Create an image element;
            Reconstruction = bitmap;
            Slice.Width = _imageWidth;
            Slice.Height = _imageHeight;
            Slice.Source = Reconstruction;



        }


        public bool MovePrev()
        {
            if (Position < 1)
            {
                return false;
            }
            else
            {
                Position--;
                return true;
            }
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
                    if (value < 0 || value >= _numSlices)
                        return;
                    else
                    {
                        _position = value;
                        System.Diagnostics.Debug.WriteLine("attempting to change image");
                        createNextImage();
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
            if (Position >= _numSlices - 1)
            {
                return false;
            }
            else
            {
                Position++;
                return true;
            }
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

        }

    }
}
