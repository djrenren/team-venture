using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MedicalImager
{
    /// <summary>
    /// Interface for the image layouts 
    /// </summary>
    public interface StudyIterator : IEnumerator<ObservableCollection<BitmapImage>>, IVisitable
    {
        /// <summary>
        /// Moves to the previous grouping of images
        /// </summary>
        /// <returns>true if the move was successful, false otherwise</returns>
        bool MovePrev();

        /// <summary>
        /// The current position in the study
        /// </summary>
        int Position { get; set; }

        /// <summary>
        /// Creates a texy representation that can be written to the disk
        /// </summary>
        /// <returns>the text representation of the layout</returns>
        string Serialize();

        IStudy Study { get; }
    }
}
