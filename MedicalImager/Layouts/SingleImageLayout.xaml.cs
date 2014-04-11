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
    [Serializable]
    public partial class SingleImageLayout : StudyLayout
    {
        public static string Representation = "1x1";
        public override string Repr { get { return Representation;} }
        public SingleImageLayout(StudyLayoutMemento mem)
        {
            InitializeComponent();
            this.Images = mem.Images;
            this.Position = mem.Position;
        }

        public SingleImageLayout(IStudy study) : this(study, 0) { }

        /// <summary>
        /// Creates a SingleImageLayout at a specific position
        /// </summary>
        /// <param name="study">the study being used</param>
        /// <param name="pos">the position in the iteration</param>
        public SingleImageLayout(IStudy study, int pos)
        {
            InitializeComponent();
            Images = new List<VirtualImage>();
            for (int i = 0; i < study.Size(); i++)
            {
                Images.Add(new VirtualImage(study[i]));
            }
            Position = pos;
            DataContext = this;
        }

        public SingleImageLayout(IStudy study, StudyLayout layout) : this(study)
        {
            Position = layout.Position;
        }

        public ObservableCollection<BitmapImage> Current = new ObservableCollection<BitmapImage>();



        public override void Serialize(FileStream stream)
        {
            //return Representation + '\n' + Position;
            //XmlSerializer x = new XmlSerializer(this.GetType());
            //x.Serialize(stream, this);
        }

        /// <summary>
        /// Gets and sets the position. Handles all the iteration
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
        public override bool MoveNext()
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
        public override bool MovePrev()
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

        public override List<VirtualImage> Images
        {
            get;
            set;
        }

        private void Image0RtClick_Click(object sender, RoutedEventArgs e)
        {
            Commands.WindowImagesCom.PromptAndCreate(Images.ElementAt(_position));
        }
    }
}
