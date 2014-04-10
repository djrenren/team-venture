using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MedicalImager.Loaders
{
    class JpegLoader : ImageLoader
    {
        private Uri _uri;

        public JpegLoader(Uri uri)
        {
            _uri = uri;
        }

        public BitmapSource LoadImage()
        {
            return (new BitmapImage(_uri));
        }
    }
}
