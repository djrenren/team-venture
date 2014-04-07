using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace MedicalImager
{
    class StudyImage
    {
        public StudyImage(Uri uri)
        {
            // TODO: Complete member initialization
        }
        public BitmapSource Source { get; set; }

        List<ImageOperation> Operations { get; set; }

        public BitmapImage getBitmapImage()
        {
            throw new NotImplementedException();
        }
    }
}
