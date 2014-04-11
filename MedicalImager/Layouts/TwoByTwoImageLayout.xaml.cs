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
using System.Xml.Serialization;

namespace MedicalImager
{
    /// <summary>
    /// Presents a 2x2 grid of images from a study
    /// </summary>
    [Serializable]
    public partial class TwoByTwoImageLayout : StudyLayout
    {

        public static string Representation = "2x2";
        public override string Repr { get { return Representation; } }

        /// <summary>
        /// Creates a TwoByTwoImageLayout. Images start by default with the
        /// first image in the study
        /// </summary>
        /// <param name="study">The study to get image Uri's from</param>
        public TwoByTwoImageLayout(IStudy study) : this(study, 0)
        {
        }


        public TwoByTwoImageLayout(StudyLayoutMemento mem)
        {
            InitializeComponent();
            this.Images = mem.Images;
            Current = new ObservableCollection<BitmapImage>();
            this.Position = mem.Position;
            DataContext = this;
        }

        /// <summary>
        /// Create A TwoByTwoImageLayout starting at a specific position
        /// </summary>
        /// <param name="study">The study to get image Uri's from</param>
        /// <param name="pos">The image position to start at</param>
        public TwoByTwoImageLayout(IStudy study, int pos)
        {
            InitializeComponent();
            Current = new ObservableCollection<BitmapImage>();
            Images = new List<VirtualImage>();
            for (int i = 0; i < study.Size(); i++)
            {
                Images.Add(new VirtualImage(study[i]));
            }
            Position = pos;
            DataContext = this;
        }

        /// <summary>
        /// Creates a new TwoByTwoImageLayout given another StudyLayout
        /// </summary>
        /// <param name="study">The study to get image Uri's from</param>
        /// <param name="layout">The StudyLayout to transfer data from</param>
        public TwoByTwoImageLayout(IStudy study, StudyLayout layout) : this(study)
        {
            Position = layout.Position;
        }

        /// <summary>
        /// The currently displayed images
        /// </summary>
        public ObservableCollection<BitmapImage> Current { get; set; }

        /// <summary>
        /// Serializes the layout
        /// </summary>
        /// <returns>a string representation of the layout</returns>
        public string Serialize(){
            return Representation + '\n' + Position;
        }

        
        /// <summary>
        /// Gets and sets the position, handles all iteration
        /// </summary>
        public override int Position
        {
            get
            {
                return _position;
            }

            set
            {
                if(value < 0 || value >= Images.Count)
                {
                    //throw new IndexOutOfRangeException("No images found at position " + value);
                    return;
                }
                else
                {
                    _position = value - (value % 4);
                    Console.WriteLine(_position);

                    //This is for the first time setting images, adds images
                    //to the list
                    if(Current.Count == 0)
                    {
                        for (int i = 0; _position + i < 4; i++)
                            if (_position + i < Images.Count)
                            Current.Add(Images.ElementAt(_position + i).getBitmapImage());
                            else
                                Current.Add(null);
                    }
                    //after the list has been created 
                    else
                    {
                        for (int i = 0; i < 4; i++)
                            if (_position + i < Images.Count)
                                Current[i] = Images.ElementAt(_position + i).getBitmapImage();
                            else
                                Current[i] = null;
                    }
                        
                }
            }
        }

        /// <summary>
        /// moves back to the first image
        /// </summary>
        public void Reset()
        {
            Position = 0;
        }

        /// <summary>
        /// Moves to the next set of images if possible
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        public override bool MoveNext()
        {
            if(Position + 4 > (Images.Count-1))
            {
                return false;
            }
            else
            {
                Position += 4;
                return true;
            }
        }

        /// <summary>
        /// Moves to the previous set of images if possible
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        public override bool MovePrev()
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

        public void Dispose() {}

        /// <summary>
        /// The VirtualImages that are being displayed
        /// </summary>
        public override List<VirtualImage> Images
        {
            get;
            set;
        }

        /// <summary>
        /// Serializes the layout by writing a string representation of it to 
        /// a file stream
        /// </summary>
        /// <param name="stream">A FileStream to serialize to</param>
        public override void Serialize(System.IO.FileStream stream)
        {
            XmlSerializer x = new XmlSerializer(this.GetType());
            x.Serialize(stream, this);
        }

        /// <summary>
        /// Indicates the top left image has been selected to be windowed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image0RtClick_Click(object sender, RoutedEventArgs e)
        {
            if (Current[0] != null)
                Commands.WindowImagesCom.PromptAndCreate(Images.ElementAt(_position));
        }

        /// <summary>
        /// Indicates that the top right image has been selected to be windowed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image1RtClick_Click(object sender, RoutedEventArgs e)
        {
            if (Current[1] != null)
                Commands.WindowImagesCom.PromptAndCreate(Images.ElementAt(_position+1));
        }

        /// <summary>
        /// Indicates that the bottom left image has been selected to be windowed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image2RtClick_Click(object sender, RoutedEventArgs e)
        {
            if (Current[2] != null)
                Commands.WindowImagesCom.PromptAndCreate(Images.ElementAt(_position+2));
        }

        /// <summary>
        /// Indicates that the bottom right image has been selected to be windowed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image3RtClick_Click(object sender, RoutedEventArgs e)
        {
            if (Current[3] != null)
                Commands.WindowImagesCom.PromptAndCreate(Images.ElementAt(_position+3));
        }
    }
}
