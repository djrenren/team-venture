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
    class AcrLoader : ImageLoader
    {
        public static readonly int HEADER_OFFSET = 0x2000;

        private Uri _uri;

        public AcrLoader(Uri uri)
        {
            _uri = uri;
        }

        public BitmapSource LoadImage()
        {
            Console.WriteLine(_uri.ToString());
            byte[] fileBytes = File.ReadAllBytes(_uri.OriginalString);

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
