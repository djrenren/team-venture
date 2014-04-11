using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MedicalImager
{
    /// <summary>
    /// Interface for the image layouts 
    /// </summary>
    [Serializable()]
    public abstract class StudyLayout : Page
    {

        /// <summary>
        /// Moves to the previous grouping of images
        /// </summary>
        /// <returns>true if the move was successful, false otherwise</returns>
        public abstract bool MovePrev();


        /// <summary>
        /// Moves to the previous grouping of images
        /// </summary>
        /// <returns>true if the move was successful, false otherwise</returns>
        public abstract bool MoveNext();

        /// <summary>
        /// The current position in the study
        /// </summary>
        public abstract int Position { get; set; }

        /// <summary>
        /// Creates a texy representation that can be written to the disk.
        /// Uneccessary data should be disposed of before serializing.
        /// </summary>
        /// <param name="stream">The filestream to write to</param>
        public abstract void Serialize(FileStream stream);

        public abstract List<VirtualImage> Images
        {
            get; set;
        }

    }
}
