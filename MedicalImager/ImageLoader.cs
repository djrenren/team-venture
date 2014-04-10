using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MedicalImager
{
    public interface ImageLoader
    {
        BitmapImage Load(Uri uriSource);
    }
}
