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
    public class Study : List<BitmapImage>, IStudy
    {
        public string directory;
        string[] filePaths;

        public Study(string dir)
        {
            filePaths = Directory.GetFiles(dir);
            directory = dir;
            foreach (string path in filePaths)
            {
                if(!path.EndsWith(".jpg"))
                    continue;
                Uri uri = new Uri(path);
                BitmapImage bm = new BitmapImage (uri);
                base.Add(bm);
            }
        }

        public int size()
        {
            return this.Count;
        }

        public void Save(string metadata)
        {
            string[] lines = { metadata };
            System.IO.File.WriteAllLines(directory + @"\.data", lines);
        }

        public void Save(Uri targetPath, string metadata)
        {
            if (System.IO.Directory.Exists(targetPath.AbsolutePath))
            {
                throw new Exception("The directory already exists: " + targetPath);
            }

            DirectoryInfo dir = Directory.CreateDirectory(targetPath.AbsolutePath);

            foreach (string path in filePaths)
            {
                string copyTo = Path.Combine(targetPath.AbsolutePath, Path.GetFileName(path));
                System.IO.File.Copy(path, copyTo, true);
            }
            string[] lines = { metadata };
            System.IO.File.WriteAllLines(targetPath + @"\.data", lines);
        }
        public string GetMeta()
        {
            var reader = new StreamReader(directory + "\\.data");
            return reader.ReadToEnd();
        }
    }
}
