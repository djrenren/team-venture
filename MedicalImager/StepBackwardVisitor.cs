using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager
{
    class StepBackwardVisitor : IVisitor
    {
        public void visitMainWindow(MainWindow window)
        {
            
        }

        public void visitLayout(StudyIterator layout)
        {
            layout.MovePrev();
        }

        public void visitStudy(IStudy study)
        {
            
        }

        public IVisitor getUndoVisitor()
        {
            throw new NotImplementedException();
        }
    }
}
