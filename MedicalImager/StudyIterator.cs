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
    interface StudyIterator : IEnumerator<ObservableCollection<BitmapImage>>
    {
        /// <summary>
        /// Moves to the previous grouping of images
        /// </summary>
        /// <returns>true if the move was successful, false otherwise</returns>
        public bool MovePrev();

        /// <summary>
        /// The current position in the study
        /// </summary>
        public int Position { get; set; }
    }
}
