using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace MedicalImager
{
    /// <summary>
    /// Represents a modification to an image
    /// </summary>
    public interface ImageOperation
    {
        /// <summary>
        /// Applies the operation to the given image
        /// </summary>
        /// <param name="bms">A BitmapSource to modify</param>
        /// <returns>A modified BitmapSource</returns>
        BitmapSource ApplyOperation(BitmapSource bms);
    }
}
