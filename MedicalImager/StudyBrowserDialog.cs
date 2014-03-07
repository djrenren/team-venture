﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedicalImager
{
    class StudyBrowserDialog
    {
        private FolderBrowserDialog folderBrowser;

        public StudyBrowserDialog()
        {
            this.folderBrowser = new FolderBrowserDialog();
        }
        /// <summary>
        /// Opens browsing window to locate a study folder
        /// </summary>
        /// <returns>File path</returns>
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
        /// Opens a browing window to specify a save location 
        /// </summary>
        /// <returns></returns>
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
