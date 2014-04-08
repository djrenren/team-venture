﻿using System;
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
    /// <summary>
    /// List of URIs with additional saving functionality 
    /// </summary>
    public class Study : List<Uri>, IStudy
    {
        public string directory;
        List<Uri> studyPaths;

        /// <summary>
        /// Creates a new study using the images contained in a directory
        /// </summary>
        /// <param name="dir">the path to the directory holding the study's images</param>
        public Study(string dir)
        {
            string[] files = Directory.GetFiles(dir);
            directory = dir;
            //this list is used to eliminate non jpeg files
            List<string> imgPaths = new List<string>();
            foreach (string path in files)
            {
                if(!path.EndsWith(".jpg") || !path.EndsWith(".acr"))
                    continue;

                Uri uri = new Uri(path);
                base.Add(uri);
            }
        }

        public int Size()
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
        public void Save()
        {
            //string[] lines = { metadata };
            //System.IO.File.WriteAllLines(directory + @"\.data", lines);
            FileStream f = new FileStream(directory + @"\.data", FileMode.Create);
            Layout.Serialize(f);
            f.Close();
        }

        /// <summary>
        /// Saves state of the system to a new directory
        /// </summary>
        /// <param name="targetPath">The path to the new directory</param>
        /// <param name="metadata">The content to write into the .data file,
        /// this should be a serialized layout</param>
        public void Save(Uri targetPath)
        {
            if (!System.IO.Directory.Exists(targetPath.AbsolutePath))
            {
                //throw new Exception("The directory already exists: " + targetPath);
                Directory.CreateDirectory(targetPath.AbsolutePath);
            }

            //DirectoryInfo dir = Directory.CreateDirectory(targetPath.AbsolutePath);
            foreach (Uri path in base.ToArray())
            {
                String stringVer = path.ToString();
                string copyTo = Path.Combine(targetPath.AbsolutePath, Path.GetFileName(stringVer));
                try
                {
                    System.IO.File.Copy(stringVer, copyTo, true);
                }
                catch (IOException e)
                {
                    Console.Out.WriteLine("Copy operation failed");
                }
            }
            //string[] lines = { metadata };
            //System.IO.File.WriteAllLines(targetPath.AbsolutePath + @"\.data", lines);
            FileStream f = new FileStream(targetPath.AbsolutePath + @"\.data", FileMode.Create);
            Layout.Serialize(f);
            f.Close();
        }

        /// <summary>
        /// Gets the meta data from the .data file in the study's directory if
        /// the file exists
        /// </summary>
        /// <returns>the content of the .data file if it exists, 
        /// null otherwise</returns>
        public string GetMeta()
        {
            /*
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
            */
            return null;
        }


        public StudyIterator Layout
        {
            get;
            set;
        }


        public void LoadSavedData()
        {
            System.Xml.Serialization.XmlSerializer loader = new System.Xml.Serialization.XmlSerializer(typeof(StudyIterator));
            
            try
            {
                FileStream f = new FileStream(directory + "\\.data", FileMode.Open);
                Layout = (StudyIterator)loader.Deserialize(f);
                f.Close();
            }
            catch (FileNotFoundException e)
            {
                Layout = new SingleImageLayout(this);
            }   
        }
    }
}
