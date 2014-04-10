﻿using System;
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
            invoker.Study = new LocalStudy(filePath);
            invoker.Navigate(invoker.Study.Layout);
            invoker.EnableOperations();
        }

        public override void UnExecute()
        {

        }
    }
}
