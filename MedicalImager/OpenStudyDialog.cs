using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MedicalImager
{
    class OpenStudyDialog : System.Windows.Forms.Form
    {
        private FolderBrowserDialog folderBrowser;

        public OpenStudyDialog()
        {
            this.folderBrowser = new FolderBrowserDialog();
            this.folderBrowser.Description = "Select a Study folder";
            this.folderBrowser.ShowNewFolderButton = false;
        }

        public string openStudy()
        {
            DialogResult result = folderBrowser.ShowDialog();
            if(result == DialogResult.OK)
            {
                return folderBrowser.SelectedPath;
            }
            return null;
        }
    }
}
