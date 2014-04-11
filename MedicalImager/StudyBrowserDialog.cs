using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedicalImager
{
    /// <summary>
    /// Creates dialog windows that allow for the selection of study
    /// directories for loading and for saving.
    /// </summary>
    class StudyBrowserDialog
    {
        private FolderBrowserDialog folderBrowser;

        public StudyBrowserDialog()
        {
            this.folderBrowser = new FolderBrowserDialog();
        }

        /// <summary>
        /// Creates a dialog window for selecting a directory to open with a study
        /// </summary>
        /// <returns>the file path as a string, null if no selection was made</returns>
        public string openStudy()
        {
            this.folderBrowser.Description = "Select a Study folder";
            this.folderBrowser.ShowNewFolderButton = false;
            DialogResult result = folderBrowser.ShowDialog();
            if(result == DialogResult.OK)
            {
                return folderBrowser.SelectedPath;
            }
            return null;
        }

        /// <summary>
        /// Opens the folder browser at a given filepath
        /// </summary>
        /// <param name="filepath">the filepath to open at</param>
        /// <returns></returns>
        public string openStudy(string filepath)
        {
            this.folderBrowser.Description = "Select a nested Study";
            this.folderBrowser.ShowNewFolderButton = false;
            this.folderBrowser.SelectedPath = filepath;
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                return folderBrowser.SelectedPath;
            }
            return null;
        }

        /// <summary>
        /// Creates a dialog window for selecting a directory to save a study in
        /// </summary>
        /// <returns>the file path as a string, null if no selection was made</returns>
        public string saveStudy()
        {
            this.folderBrowser.Description = "Select a folder or make a new folder (recommended) to save all images in";
            this.folderBrowser.ShowNewFolderButton = true;
            DialogResult result = folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
            {
                return folderBrowser.SelectedPath;
            }
            return null;
        }
    }
}
