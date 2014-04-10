using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MedicalImager
{
    public class JPGImageLoader : ImageLoader
    {
        public BitmapImage Load(Uri uriSource)
        {
            return new BitmapImage(uriSource);
        }
    }
}
