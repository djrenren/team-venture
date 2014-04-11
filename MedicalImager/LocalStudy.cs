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
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows;

namespace MedicalImager
{
    /// <summary>
    /// List of URIs with additional saving functionality 
    /// </summary>
    public class LocalStudy : List<Uri>, IStudy
    {
        //The study's directory
        public string directory;

        //The filepaths to nested folders
        public string[] studyPaths;

        /// <summary>
        /// Creates a new study using the images contained in a directory
        /// </summary>
        /// <param name="dir">the path to the directory holding the study's images</param>
        public LocalStudy(string dir)
        {
            try
            {
                string[] files = Directory.GetFiles(dir);
                directory = dir;
                //this list is used to eliminate non jpeg files
                List<string> imgPaths = new List<string>();
                foreach (string path in files)
                {
                    if (!path.EndsWith(".jpg") && !path.EndsWith(".acr"))
                        continue;

                    Uri uri = new Uri(path);
                    base.Add(uri);
                }
                studyPaths = Directory.GetDirectories(dir);
                LoadSavedData();
            }
            catch (IOException e)
            {
                MessageBox.Show("Error loading study: " + e.Message);
            }
        }

        /// <summary>
        /// Gets the size of the study
        /// </summary>
        /// <returns>the number of items in the study</returns>
        public int Size()
        {
            return this.Count;
        }

        /// <summary>
        /// Sets this study as the default study
        /// </summary>
        public void SetDefault()
        {
            Environment.SetEnvironmentVariable("MedImgDefault", directory, EnvironmentVariableTarget.User);
        }

        /// <summary>
        /// Saves the state of the system to the study's directory
        /// </summary>
        /// <param name="metadata">The content to write into the .data file,
        /// this should be a serialized layout</param>
        public void Save()
        {
            StudyLayoutMemento data = Layout.GetData();

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream f = new FileStream(directory + @"\" + ".data", FileMode.Create); 
            formatter.Serialize(f, data);
            f.Close();
        }

        /// <summary>
        /// Saves state of the system to a new directory
        /// </summary>
        /// <param name="targetPath">The path to the new directory</param>
        public void Save(Uri targetPath)
        {
            if (!System.IO.Directory.Exists(targetPath.AbsolutePath))
            {
                Directory.CreateDirectory(targetPath.AbsolutePath);
            }

            //DirectoryInfo dir = Directory.CreateDirectory(targetPath.AbsolutePath);
            foreach (Uri path in base.ToArray())
            {
                string stringVer = path.OriginalString;
                string copyTo = Path.Combine(targetPath.AbsolutePath.ToString(), Path.GetFileName(stringVer));
                try
                {
                    System.IO.File.Copy(stringVer, copyTo, true);
                }
                catch (IOException e)
                {
                    Console.Out.WriteLine("Copy operation failed: " + e.ToString());
                }
            }

            foreach (string dirPath in Directory.GetDirectories(directory, "*",
                SearchOption.AllDirectories))
                Directory.CreateDirectory(dirPath.Replace(directory, targetPath.OriginalString));

            foreach (string newPath in Directory.GetFiles(directory, "*.*",
                SearchOption.AllDirectories))
                File.Copy(newPath, newPath.Replace(directory, targetPath.OriginalString), true);

            StudyLayoutMemento data = Layout.GetData();

            BinaryFormatter formatter = new BinaryFormatter();
            FileStream f = new FileStream(directory + @"\" + ".data", FileMode.Create); 
            formatter.Serialize(f, data);
            f.Close();
        }

        /// <summary>
        /// Gets and sets the current layout
        /// </summary>
        public StudyLayout Layout
        {
            get;
            set;
        }

        /// <summary>
        /// Loads the study from memory
        /// </summary>
        public void LoadSavedData()
        {

            try
            {
                FileStream f = new FileStream(directory + "\\.data", FileMode.Open);
                using (f)
                {
                    BinaryFormatter formatter = new BinaryFormatter();
                    StudyLayoutMemento m = formatter.Deserialize(f) as StudyLayoutMemento;
                    Layout = StudyLayout.Reconstruct(m);
                    if (Layout != null) return;
                }
            }
            catch (Exception e)

            {
                
            }
            Layout = new SingleImageLayout(this);



            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream imgStream = new FileStream(directory + "\\.img", FileMode.Open);
                Layout.Images = formatter.Deserialize(imgStream) as List<VirtualImage>;
            }
            catch (FileNotFoundException) { }

        }
    }
}
