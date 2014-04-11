using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MedicalImager
{
    /// <summary>
    /// Interface for image loading strategies
    /// </summary>
    public interface ImageLoader
    {
        /// <summary>
        /// Loads the image as a BitmapSource
        /// </summary>
        /// <returns>the loaded image</returns>
        BitmapSource LoadImage();
    }
}
