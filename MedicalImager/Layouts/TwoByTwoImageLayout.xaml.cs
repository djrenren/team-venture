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
    /// Interaction logic for SingleImageLayout.xaml
    /// </summary>
    public partial class TwoByTwoImageLayout : StudyLayout
    {

        public static string Representation = "2x2";

        public TwoByTwoImageLayout(IStudy study) : this(study, 0)
        {
        }

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

        public TwoByTwoImageLayout(IStudy study, StudyLayout layout) : this(study)
        {
            Position = layout.Position;
        }

        public ObservableCollection<BitmapImage> Current { get; set; }

        public string Serialize(){
            return Representation + '\n' + Position;
        }

        private int _position = -1;
        
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
                if(value != _position)
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
                        //This is for the first time setting images
                        if(Current.Count == 0)
                        {
                            for (int i = 0; _position + i < Images.Count && i < 4; i++)
                                Current.Add(Images.ElementAt(_position + i).getBitmapImage());
                        }
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

        public void Dispose()
        {

        }


        public override List<VirtualImage> Images
        {
            get;
            set;
        }


        public override void Serialize(System.IO.FileStream stream)
        {
            XmlSerializer x = new XmlSerializer(this.GetType());
            x.Serialize(stream, this);
        }
    }
}
