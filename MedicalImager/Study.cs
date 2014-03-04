using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.IO;
using System.Drawing;

namespace MedicalImager
{
    class Study : List<BitmapImage>, IStudy
    {
        public string directory;
        string[] filePaths;

        public Study(string dir)
        {
            filePaths = Directory.GetFiles(dir);
            directory = dir;
            foreach (string path in filePaths)
            {
                Uri uri = new Uri(path);
                BitmapImage bm = new BitmapImage (uri);
                base.Add(bm);
            }
        }
    }
}
