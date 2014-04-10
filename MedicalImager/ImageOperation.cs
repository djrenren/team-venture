using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace MedicalImager
{
    public interface ImageOperation
    {
        BitmapSource ApplyOperation(BitmapSource bms);
    }
}
