using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MedicalImager.Loaders
{
    /// <summary>
    /// Responsible for loading Acr images
    /// </summary>
    [Serializable]
    class AcrLoader : ImageLoader
    {
        //offset to skip the header
        public static readonly int HEADER_OFFSET = 0x2000;

        private Uri _uri;

        public AcrLoader(Uri uri)
        {
            _uri = uri;
        }

        /// <summary>
        /// Loads the acr image in _uri as a BitmapSource
        /// </summary>
        /// <returns>the loaded image</returns>
        public BitmapSource LoadImage()
        {
            //Console.WriteLine(_uri.);
            byte[] fileBytes = File.ReadAllBytes(_uri.AbsolutePath);

            int position = HEADER_OFFSET;

            int width = 256;
            int height = 256;

            int pixelHigh = 0;
            int pixelLow = 0;

            byte[] pixels = new byte[width * height];

            for(int i = 0; i < width*height; i++)
            {
                pixelHigh = fileBytes[position];
                position++;
                pixelLow = fileBytes[position];
                position++;

                pixels[i] = (byte)(pixelHigh << 4 | pixelLow >> 4);
            }

            BitmapSource bms = BitmapSource.Create(width, height, 96, 96, PixelFormats.Gray8, null, pixels, width);
            return bms;
        }
    }
}
