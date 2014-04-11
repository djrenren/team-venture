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
    public partial class CoronalReconstruction : Page, StudyLayout
    {
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
            Images = new List<VirtualImage>();
            _reconstructionEnabled = false;
            for (int i = 0; i < study.Size(); i++)
            {
                Images.Add(new VirtualImage(study[i]));
            }
            DataContext = this;
            //gets a sample image from the study if possible
            BitmapImage sample = Images.Count > 0 ? Images.ElementAt(0).getBitmapImage() : null;
            if(sample ==  null)
            {
                _numSlices = 0;
            }
            else
            {
                _numSlices = sample.PixelHeight;
            }
            ReconstructionImages = new List<VirtualImage>();
            for(int i = 0; i < _numSlices; i++) {ReconstructionImages.Add(null);}
            _reconstructionPos = 0;
            setImage();
            Position = pos;
        }

        private void setImage()
        {
            if (_numSlices == 0)
                return;
            if(ReconstructionImages.ElementAt(_reconstructionPos) == null)
            {
                VirtualImage newImg = new VirtualImage(new Loaders.ReconstructionLoader(Images, 
                    _reconstructionPos, 
                    Loaders.ReconstructionType.Coronal));
                ReconstructionImages.Insert(_reconstructionPos, newImg);
            }

            Slice.Source = ReconstructionImages.ElementAt(_reconstructionPos).Source;
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
                        setImage();
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
                            setImage();
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


        public List<VirtualImage> Images
        {
            get;
            set;
        }

        public List<VirtualImage> ReconstructionImages
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

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Commands.WindowImagesCom.PromptAndCreate();
               
        }

        private void removeLines()
        {
            foreach(VirtualImage vimg in Images)
            {
                
            } 
        }

    }
}
