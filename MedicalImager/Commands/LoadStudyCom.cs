using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    /// <summary>
    /// Command for loading a study
    /// </summary>
    class LoadStudyCom : Command
    {

        private string studyLocation;

        public LoadStudyCom(StudyLayout layout, string studyLocation) : base (layout)
        {
            this.studyLocation = studyLocation;
        }

        /// <summary>
        /// Prompts the user to select a study to load
        /// </summary>
        /// <returns>A LoadStudyCom, or null if the user cancels</returns>
        public static LoadStudyCom PromptAndCreate()
        {
            StudyBrowserDialog newDialog = new StudyBrowserDialog();
            string filePath = newDialog.openStudy();
            if (filePath == null)
                return null;
            return new LoadStudyCom(invoker.Study != null ? invoker.Study.Layout : null, filePath);
            
        }

        /// <summary>
        /// Loads the study at the selected directory. If the directory only
        /// contains other directories it prompts the user to select a nested
        /// directory.
        /// </summary>
        public override void Execute()
        {
            StudyBrowserDialog newDialog;
            LocalStudy l = new LocalStudy(studyLocation);
            while (l.Size() == 0 && l.studyPaths.Length != 0)
            {
                newDialog = new StudyBrowserDialog();
                studyLocation = newDialog.openStudy(studyLocation);
                if (studyLocation == null)
                    return;
                l = new LocalStudy(studyLocation);
            }
            
            invoker.Study = l;
            invoker.Navigate(invoker.Study.Layout);
            invoker.EnableOperations();
            invoker.UpdateCount();
            invoker.CommandStack.Clear();
        }

        public override void UnExecute()
        {

        }
    }
}
