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
        private IStudy _study;
        private int imageWidth;
        private int imageHeight;
        private int numSlices;

        public BitmapSource Reconstruction { get; set; }

        public CoronalReconstruction(IStudy study) : this(study, 0) {}

        public CoronalReconstruction(IStudy study, int pos)
        {
            InitializeComponent();
            _study = study;
            Current = new ObservableCollection<BitmapImage>();
            Reconstruction = null;
            Position = pos;
            DataContext = this;
            //gets a sample image from the study if possible
            BitmapImage sample = _study.Size() > 0 ? _study[0] : null;
            if(sample ==  null)
            {
                imageWidth = 0;
                imageHeight = 0;
                numSlices = 0;
            }
            else
            {
                imageWidth = sample.PixelWidth;
                imageHeight = _study.Size();
                numSlices = sample.PixelHeight;
            }
        }

        private void createNextImage()
        {
            BitmapSource bms;
            Console.WriteLine("w: " + imageWidth + " h: " + imageHeight);
            PixelFormat pf = PixelFormats.Bgr32;
            int rawStride = (imageWidth * 5);
            byte[] rawImage = new byte[rawStride * imageHeight];
            for(int i = 0; i < _study.Size(); i++)
            {
                _study[i].CopyPixels(new Int32Rect(0, numSlices - Position - 1, imageWidth, 1), 
                    rawImage, 
                    rawStride, 
                    (_study.Size()-i-1)*(rawStride));
            }

            // Create a BitmapSource.
            BitmapSource bitmap = BitmapSource.Create(imageWidth, imageHeight,
                96, 96, pf, null,
                rawImage, rawStride);

            // Create an image element;
            Reconstruction = bitmap;
            Slice.Width = imageWidth;
            Slice.Height = imageHeight;
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
                    if (value < 0 || value >= numSlices)
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
            if (Position >= numSlices - 1)
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
    }
}
