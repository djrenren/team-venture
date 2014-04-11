using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    class LoadStudyCom : Command
    {
        public LoadStudyCom(StudyLayout layout) : base (layout)
        {

        }
        
        public override void Execute()
        {
            StudyBrowserDialog newDialog = new StudyBrowserDialog();
            string filePath = newDialog.openStudy();
            if (filePath == null)
                return;
            LocalStudy l = new LocalStudy(filePath);
            while(l.Size() == 0 && l.studyPaths.Length != 0)
            {
                newDialog = new StudyBrowserDialog();
                filePath = newDialog.openStudy(filePath);
                if (filePath == null)
                    return;
                l = new LocalStudy(filePath);
            }
            invoker.Study = new LocalStudy(filePath);
            invoker.Navigate(invoker.Study.Layout);
            invoker.EnableOperations();
            invoker.UpdateCount();
        }

        public override void UnExecute()
        {

        }
    }
}
