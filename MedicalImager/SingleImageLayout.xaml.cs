﻿using System;
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
    /// Interaction logic for SingleImageLayout.xaml
    /// </summary>
    public partial class SingleImageLayout : Page, StudyIterator
    {
        private IStudy _study;
        public static string Representation = "1x1";

        public SingleImageLayout(IStudy study) : this(study, 0) {}

        /// <summary>
        /// Creates a SingleImageLayout at a specific position
        /// </summary>
        /// <param name="study">the study being used</param>
        /// <param name="pos">the position in the iteration</param>
        public SingleImageLayout(IStudy study, int pos)
        {
            InitializeComponent();
            _study = study;
            Current = new ObservableCollection<BitmapImage>();
            Position = pos;
            DataContext = this;
        }

        public SingleImageLayout(IStudy study, StudyIterator layout) : this(study)
        {
            Position = layout.Position;
        }

        public ObservableCollection<BitmapImage> Current { get; set; }

        private int _position = -1;


        public string Serialize()
        {
            return Representation + '\n' + Position;
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
                    if(value < 0 || value >= _study.Size())
                    {
                        //throw new IndexOutOfRangeException("No images found at position " + value);
                        return;
                    }
                    else
                    {
                        if (Current.Count == 0)
                        {
                            Current.Add(_study[value]);
                        }
                        else
                        {
                            Current[0] = _study[value];
                        }
                        _position = value;
                        Console.Out.WriteLine("New Position: " + Position);
                        Console.Out.WriteLine(Current[0]);
                        Image1.Source = Current[0];
                    }
                }
            }
        }

        /// <summary>
        /// The study being used
        /// </summary>
        public IStudy Study
        {
            get
            {
                return _study;
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
            if(Position >= _study.Size()-1)
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

        public void accept(IVisitor v)
        {
            v.visitLayout(this);
        }
    }
}
