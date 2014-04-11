using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MedicalImager
{
    /// <summary>
    /// An operation that scales the values of pixels in images
    /// </summary>
    [Serializable]
    class WindowOp : ImageOperation
    {

        private int _min;

        private int _max;

        /// <summary>
        /// Creates a new window operation
        /// </summary>
        /// <param name="min">The minimum pixel value</param>
        /// <param name="max">The maximum pixel value</param>
        public WindowOp(int min, int max)
        {
            _min = min;
            _max = max;
        }

        /// <summary>
        /// Applies the Windowing operation to the given image
        /// </summary>
        /// <param name="bms">The image to apply the windowing to</param>
        /// <returns>a bitmap source representing the modified image</returns>
        public BitmapSource ApplyOperation(BitmapSource bms)
        {
            double m = 255 / (_max - _min);
            double b = -1 * 255 * _min / (_max - _min);


            int rawStride = (bms.PixelWidth * bms.Format.BitsPerPixel + 7) / 8;
            byte[] rawImage = new byte[rawStride * bms.PixelHeight];
            bms.CopyPixels(rawImage, rawStride, 0);
            for(int i = 0; i < rawImage.Length; i++)
            {
                if(rawImage[i] < _min)
                {
                    rawImage[i] = 0;
                }
                else if(rawImage[i] > _max)
                {
                    rawImage[i] = 255;
                }
                else
                {
                    rawImage[i] = (byte)(rawImage[i] * m + b);
                }
            }
            BitmapSource bitmap = BitmapSource.Create(bms.PixelWidth, bms.PixelHeight,
                96, 96, bms.Format, null,
                rawImage, rawStride);

            return bitmap;
        }
    }
}
