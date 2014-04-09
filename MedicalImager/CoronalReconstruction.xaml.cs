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
        private int imageWidth;
        private int imageHeight;
        private int numSlices;

        public BitmapSource Reconstruction { get; set; }

        public CoronalReconstruction(IStudy study) : this(study, 0) {}

        public CoronalReconstruction(IStudy study, int pos)
        {
            InitializeComponent();
            Current = new ObservableCollection<BitmapImage>();
            Reconstruction = null;
            Position = pos;
            for (int i = 0; i < study.Size(); i++)
            {
                StudyImage studyImg = new StudyImage(study[i]);
            }
            DataContext = this;
            //gets a sample image from the study if possible
            BitmapImage sample = Images.Count > 0 ? Images.ElementAt(0).getBitmapImage() : null;
            if(sample ==  null)
            {
                imageWidth = 0;
                imageHeight = 0;
                numSlices = 0;
            }
            else
            {
                imageWidth = sample.PixelWidth;
                imageHeight = Images.Count;
                numSlices = sample.PixelHeight;
            }
        }

        private void createNextImage()
        {
            Console.WriteLine("w: " + imageWidth + " h: " + imageHeight);
            PixelFormat pf = PixelFormats.Bgr32;
            int rawStride = (imageWidth * 5);
            byte[] rawImage = new byte[rawStride * imageHeight];
            for(int i = 0; i < Images.Count; i++)
            {
                Images.ElementAt(i).Source.CopyPixels(new Int32Rect(0, numSlices - Position - 1, imageWidth, 1), 
                    rawImage, 
                    rawStride, 
                    (Images.Count-i-1)*(rawStride));
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


        public List<StudyImage> Images
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }


        public void Serialize(System.IO.FileStream stream)
        {
            throw new NotImplementedException();
        }


        List<StudyImage> StudyIterator.Images
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }
    }
}
