using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace MedicalImager
{
    public interface IStudy
    {
        int size();
        BitmapImage this[int index]
        {
            get;
            set;
        }

        string GetMeta();

        void Save(string targetUri, string metadata);

    }
}
