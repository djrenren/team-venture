using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
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
    /// Interaction logic for SingleImageLayout.xaml
    /// </summary>
    public partial class SingleImageLayout : Page, StudyLayout
    {
        public static string Representation = "1x1";

        //List<StudyImage> images;

        public SingleImageLayout(IStudy study) : this(study, 0) {}

        /// <summary>
        /// Creates a SingleImageLayout at a specific position
        /// </summary>
        /// <param name="study">the study being used</param>
        /// <param name="pos">the position in the iteration</param>
        public SingleImageLayout(IStudy study, int pos)
        {
            InitializeComponent();
            Images = new List<StudyImage>();
            Current = new ObservableCollection<BitmapImage>();
            for (int i = 0; i < study.Size(); i++)
            {
                Images.Add(new StudyImage(study[i]));
            }
            Position = pos;
            DataContext = this;
        }

        public SingleImageLayout(IStudy study, StudyLayout layout) : this(study)
        {
            Position = layout.Position;
        }

        public ObservableCollection<BitmapImage> Current { get; set; }

        private int _position = -1;


        public void Serialize(FileStream stream)
        {
            //return Representation + '\n' + Position;
            XmlSerializer x = new XmlSerializer(this.GetType());
            x.Serialize(stream, this);
        }

        /// <summary>
        /// Gets and sets the position. Handles all the iteration
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
                    if(value < 0 || value >= Images.Count)
                    {
                        //throw new IndexOutOfRangeException("No images found at position " + value);
                        return;
                    }
                    else
                    {
                        if (Current.Count == 0)
                        {
                            Current.Add(Images.ElementAt(value).getBitmapImage());
                        }
                        else
                        {
                            Current[0] = Images.ElementAt(value).getBitmapImage();
                        }
                        _position = value;
                        Image1.Source = Current[0];
                    }
                }
            }
        }

        /// <summary>
        /// The collection of images being displayed
        /// </summary>
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }

        /// <summary>
        /// Resets the iteration to the first element
        /// </summary>
        public void Reset()
        {
            Position = 0;
        }

        /// <summary>
        /// Moves to the next image if possible
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        public bool MoveNext()
        {
            if(Position >= Images.Count-1)
            {
                return false;
            }
            else
            {
                Position++;
                return true;
            }
        }

        /// <summary>
        /// Moves to the previous image if possible
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        public bool MovePrev()
        {
            if(Position < 1)
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

        public List<StudyImage> Images
        {
            get;
            set;
        }
    }
}
