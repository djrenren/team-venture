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

        /// <summary>
        /// Creates a new study using the images contained in a directory
        /// </summary>
        /// <param name="dir">the path to the directory holding the study's images</param>
        public Study(string dir)
        {
            filePaths = Directory.GetFiles(dir);
            directory = dir;
            //this list is used to eliminate non jpeg files
            List<string> imgPaths = new List<string>();
            foreach (string path in filePaths)
            {
                if(!path.EndsWith(".jpg"))
                    continue;
                imgPaths.Add(path);
                Uri uri = new Uri(path);
                BitmapImage bm = new BitmapImage (uri);
                base.Add(bm);
            }
            filePaths = imgPaths.ToArray<string>();
        }

        public int size()
        {
            return this.Count;
        }

        public void SetDefault()
        {
            Environment.SetEnvironmentVariable("MedImgDefault", directory, EnvironmentVariableTarget.User);
        }

        /// <summary>
        /// Saves the state of the system to the study's directory
        /// </summary>
        /// <param name="metadata">The content to write into the .data file,
        /// this should be a serialized layout</param>
        public void Save(string metadata)
        {
            string[] lines = { metadata };
            System.IO.File.WriteAllLines(directory + @"\.data", lines);
        }

        /// <summary>
        /// Saves state of the system to a new directory
        /// </summary>
        /// <param name="targetPath">The path to the new directory</param>
        /// <param name="metadata">The content to write into the .data file,
        /// this should be a serialized layout</param>
        public void Save(Uri targetPath, string metadata)
        {
            if (!System.IO.Directory.Exists(targetPath.AbsolutePath))
            {
                //throw new Exception("The directory already exists: " + targetPath);
                Directory.CreateDirectory(targetPath.AbsolutePath);
            }

            //DirectoryInfo dir = Directory.CreateDirectory(targetPath.AbsolutePath);

            foreach (string path in filePaths)
            {
                string copyTo = Path.Combine(targetPath.AbsolutePath, Path.GetFileName(path));
                try
                {
                    System.IO.File.Copy(path, copyTo, true);
                }
                catch (IOException e)
                {
                    Console.Out.WriteLine("Copy operation failed");
                }
            }
            string[] lines = { metadata };
            System.IO.File.WriteAllLines(targetPath.AbsolutePath + @"\.data", lines);
        }

        /// <summary>
        /// Gets the meta data from the .data file in the study's directory if
        /// the file exists
        /// </summary>
        /// <returns>the content of the .data file if it exists, 
        /// null otherwise</returns>
        public string GetMeta()
        {
            try
            {
                var reader = new StreamReader(directory + "\\.data");
                string meta = reader.ReadToEnd();
                reader.Close();
                return meta;
            }
            catch (IOException e)
            {
                return null;
            }
        }
    }
}
