using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
    /// Interaction logic for CoronalReconstruction.xaml
    /// </summary>
    [Serializable]
    public partial class CoronalReconstruction : StudyLayout
    {
        //The number of slices in the reconstruction
        private int _numSlices;

        //The current position in the reconstruction
        private int _reconstructionPos;

        //Determines if the reconstruction, or the study is being controlled with
        //next and previous buttons
        private bool _reconstructionEnabled;

        //Representation to be used when serializing
        public static string Representation = "CR";

        public override string Repr { get { return Representation; } }

        public BitmapSource Reconstruction { get; set; }


        /// <summary>
        /// Creates a CoronalReconstruction starting at the first image
        /// </summary>
        /// <param name="study">the study to display</param>
        public CoronalReconstruction(IStudy study) : this(study, 0) {}

        public CoronalReconstruction()
        {
            InitializeComponent();
            Current = new ObservableCollection<BitmapImage>();
            Reconstruction = null;
            Images = new List<VirtualImage>();
            _reconstructionEnabled = false;
            _reconstructionPos = 0;
            ReconstructionImages = new List<VirtualImage>();
            CoronalLine.Y1 = _numSlices;
            CoronalLine.Y2 = _numSlices;

        }

        public CoronalReconstruction(StudyLayoutMemento mem) : this()
        {
            this.Images = mem.Images;
            DataContext = this;
            sampleImages();
            this.Position = mem.Position;

            
        }

        public CoronalReconstruction(IStudy study, int pos) : this()
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
            //gets a sample image from the study if possible to use to determine the size
            //of the reconstruction
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

            ReconstructionImages = new List<VirtualImage>();
            for (int i = 0; i < _numSlices; i++) { ReconstructionImages.Add(null);}
            _reconstructionPos = 0;
            setImage();
        }

        /// <summary>
        /// Sets the currently displayed reconstruction
        /// </summary>
        private void setImage()
        {
            if (_numSlices == 0)
                return;
            if (ReconstructionImages.ElementAt(_reconstructionPos) == null)
            {
                VirtualImage newImg = new VirtualImage(new Loaders.ReconstructionLoader(Images, 
                    _reconstructionPos, 
                    Loaders.ReconstructionType.Coronal));
                ReconstructionImages.Insert(_reconstructionPos, newImg);
            }

            Slice.Source = ReconstructionImages.ElementAt(_reconstructionPos).Source;
        }


        /// <summary>
        /// Attempts to move to the previous position
        /// </summary>
        /// <returns>Whether or not the operation was successful</returns>
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


        /// <summary>
        /// Gets and sets the current position in the study, updating images
        /// if necessary
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
                    //The first time an image is displayed
                        if (Current.Count == 0)
                        {
                            Current.Add(Images.ElementAt(value).getBitmapImage());
                        }
                    //Subsequent displays
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

        /// <summary>
        /// The current images from the study being displayed
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

        /// <summary>
        /// The images from the study
        /// </summary>
        public override List<VirtualImage> Images
        {
            get;
            set;
        }

        /// <summary>
        /// The reconstructed imagesd
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
            CoronalLine.Visibility = _reconstructionEnabled ? Visibility.Visible : Visibility.Hidden;
            moveLine();
            Debug.WriteLine("OrigCol.Width =" + OrigCol.Width);
            Debug.WriteLine("OrigCol.ActualWidth =" + OrigCol.ActualWidth);
            Debug.WriteLine("OrigCol.MaxWidth =" + OrigCol.MaxWidth);
        }

        /// <summary>
        /// Indicates that an windowing operation has been requested for the currently
        /// displayed study image
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
        /// Indicates that a windowing operation has been requested on the currently displayed
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
            CoronalLine.Y1 = Orig.ActualHeight - newVal;
            CoronalLine.Y2 = Orig.ActualHeight - newVal;
            CoronalLine.X1 = (OrigCol.ActualWidth - Orig.ActualWidth) / 2;
            CoronalLine.X2 = ((OrigCol.ActualWidth - Orig.ActualWidth) / 2) + Orig.ActualWidth;
        }

    }
}
