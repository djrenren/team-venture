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
    /// Interaction logic for SaggitalReconstruction.xaml
    /// </summary>
    [Serializable]
    public partial class SaggitalReconstruction : StudyLayout
    {
        //The number of slices that can be created
        private int _numSlices;

        //The position of the reconstruction
        private int _reconstructionPos;

        //Determines if the reconstruction, or the study is being controlled with
        //next and previous buttons
        private bool _reconstructionEnabled;

        //Representation to be used when serializing
        public static string Representation = "SR";
        public override string Repr { get { return Representation; } }

        public BitmapSource Reconstruction { get; set; }


        /// <summary>
        /// Creates a SaggitalReconstruction starting at the first image
        /// </summary>
        /// <param name="study">The study to load Uri's from</param>
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

            SaggLine.Y1 = _numSlices;
            SaggLine.Y2 = _numSlices;
        }

        /// <summary>
        /// Sets the displayed reconstruction image
        /// </summary>
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

        /// <summary>
        /// Attempts to move to the previous position
        /// </summary>
        /// <returns>Whether or not the move was successful</returns>
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


        /// <summary>
        /// Changes the study position, and updates the displayed image
        /// </summary>
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
                    //first time displaying an image
                    if (Current.Count == 0)
                    {
                        Current.Add(Images.ElementAt(value).getBitmapImage());
                    }
                    //subsequent displays
                    else
                    {
                        Current[0] = Images.ElementAt(value).getBitmapImage();
                    }
                    Orig.Source = Current[0];
                }
            }
        }

        /// <summary>
        /// Returns a string representation of the layout
        /// </summary>
        /// <returns>a string representing the layout</returns>
        public string Serialize()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// The currently displayed study image
        /// </summary>
        public ObservableCollection<BitmapImage> Current { get; set; }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Attempts to move to the next position
        /// </summary>
        /// <returns>Whether or not the move was successful</returns>
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

        /// <summary>
        /// The VirtualImages being displayed as part of the study
        /// </summary>
        public override List<VirtualImage> Images
        {
            get;
            set;
        }

        /// <summary>
        /// The virtual images being displayed as part of the reconstruction
        /// </summary>
        public List<VirtualImage> ReconstructionImages
        {
            get;
            set;
        }

        public override void Serialize(System.IO.FileStream stream)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Toggles navigation control between the study and the reconstruction
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            _reconstructionEnabled = !_reconstructionEnabled;
            SaggLine.Visibility = _reconstructionEnabled ? Visibility.Visible : Visibility.Hidden;
        }

        /// <summary>
        /// Indicates a window operation was requested on the currently displayed
        /// study image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image0RtClick_Click(object sender, RoutedEventArgs e)
        {
            Commands.WindowImagesCom com = Commands.WindowImagesCom.PromptAndCreate(Images.ElementAt(_position));
            if (com != null)
                com.Execute();
        }

        /// <summary>
        /// Indicates that a window operation was requested on the currently displayed
        /// reconstruction image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image1RtClick_Click(object sender, RoutedEventArgs e)
        {
            Commands.WindowImagesCom com = Commands.WindowImagesCom.PromptAndCreate(ReconstructionImages.ElementAt(_reconstructionPos));
            if (com != null)
                com.Execute();
            setImage();
        }

        private void moveLine()
        {
            int newVal = (int)((double)_reconstructionPos * (Orig.ActualHeight / (double)_numSlices));
            SaggLine.Y1 = Orig.ActualHeight - newVal;
            SaggLine.Y2 = Orig.ActualHeight - newVal;
            SaggLine.X1 = (OrigCol.ActualWidth - Orig.ActualWidth) / 2;
            SaggLine.X2 = ((OrigCol.ActualWidth - Orig.ActualWidth) / 2) + Orig.ActualWidth;
        }

    }
}
