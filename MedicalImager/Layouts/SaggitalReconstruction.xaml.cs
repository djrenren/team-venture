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
    [Serializable]
    public partial class SaggitalReconstruction : StudyLayout
    {
        private int _numSlices;
        private int _reconstructionPos;
        //Determines if the reconstruction, or the study is being controlled with
        //next and previous buttons
        private bool _reconstructionEnabled;

        //Representation to be used when serializing
        public static string Representation = "SR";
        public override string Repr { get { return Representation; } }

        public BitmapSource Reconstruction { get; set; }


        public SaggitalReconstruction(IStudy study) : this(study, 0) {}


        public SaggitalReconstruction()
        {
            InitializeComponent();
            Current = new ObservableCollection<BitmapImage>();
            Reconstruction = null;
            Images = new List<VirtualImage>();
            _reconstructionEnabled = false;
            _reconstructionPos = 0;
            ReconstructionImages = new List<VirtualImage>();
            SaggLine.X1 = _numSlices;
            SaggLine.X2 = _numSlices;

        }

        public SaggitalReconstruction(StudyLayoutMemento mem) : this()
        {
            this.Images = mem.Images;
            DataContext = this;
            sampleImages();
            this.Position = mem.Position;

            
        }
        
        public SaggitalReconstruction(IStudy study, int pos) : this()
        {
            Images = new List<VirtualImage>();
            for (int i = 0; i < study.Size(); i++)
            {
                Images.Add(new VirtualImage(study[i]));
            }
            DataContext = this;
            //gets a sample image from the study if possible
            sampleImages();
            Position = pos;
        }

        private void sampleImages()
        {
            BitmapImage sample = Images.Count > 0 ? Images.ElementAt(0).getBitmapImage() : null;
            if (sample == null)
            {
                _numSlices = 0;
            }
            else
            {
                _numSlices = sample.PixelHeight;
            }
            for (int i = 0; i < _numSlices; i++) { ReconstructionImages.Add(null); }
            setImage();

            SaggLine.X1 = _numSlices;
            SaggLine.X2 = _numSlices;
        }

        private void setImage()
        {
            if (_numSlices == 0)
                return;
            if (ReconstructionImages.ElementAt(_reconstructionPos) == null)
            {
                VirtualImage newImg = new VirtualImage(new Loaders.ReconstructionLoader(Images,
                    _reconstructionPos,
                    Loaders.ReconstructionType.Saggital));
                ReconstructionImages.Insert(_reconstructionPos, newImg);
            }

            Slice.Source = ReconstructionImages.ElementAt(_reconstructionPos).Source;
        }


        public override bool MovePrev()
        {
            if (_reconstructionEnabled)
            {
                    if (_reconstructionPos < 1)
                        return false;
                    else
                    {
                        _reconstructionPos--;
                        setImage();
                        moveLine();
                        return true;
                    }
            } else {
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
        }


        public override int Position
        {
            get
            {
                return _position;
            }
            set
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


        public override bool MoveNext()
        {
            if (_reconstructionEnabled)
            {
                if (_reconstructionPos >= _numSlices - 1)
                {
                    return false;
                }
                else
                {
                    _reconstructionPos++;
                    setImage();
                    moveLine();
                    return true;
                }
            }
            else
            {
                if (Position >= Images.Count - 1)
                    return false;
                else
                {
                    Position++;
                    return true;
                }
            }
        }

        public void Reset()
        {
            throw new NotImplementedException();
        }


        public override List<VirtualImage> Images
        {
            get;
            set;
        }

        public List<VirtualImage> ReconstructionImages
        {
            get;
            set;
        }

        public override void Serialize(System.IO.FileStream stream)
        {
            throw new NotImplementedException();
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            _reconstructionEnabled = !_reconstructionEnabled;
            SaggLine.Visibility = _reconstructionEnabled ? Visibility.Visible : Visibility.Hidden;
            moveLine();
        }

        private void Image0RtClick_Click(object sender, RoutedEventArgs e)
        {
            Commands.WindowImagesCom.PromptAndCreate(Images.ElementAt(_position));
        }

        private void Image1RtClick_Click(object sender, RoutedEventArgs e)
        {
            Commands.WindowImagesCom.PromptAndCreate(ReconstructionImages.ElementAt(_reconstructionPos));
            setImage();
        }

        private void moveLine()
        {
            int newVal = (int)((double)_reconstructionPos * (Orig.ActualWidth / (double)_numSlices))
                + (((int)OrigCol.ActualWidth - (int)Orig.ActualWidth) / 2);
            SaggLine.Y1 = (OrigRow.ActualHeight - Orig.ActualHeight) / 2;
            SaggLine.Y2 = ((OrigRow.ActualHeight - Orig.ActualHeight) / 2) + Orig.ActualHeight;
            SaggLine.X1 = newVal;
            SaggLine.X2 = newVal;
        }

    }
}
