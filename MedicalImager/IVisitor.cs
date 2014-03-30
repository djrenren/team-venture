using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager
{
    interface IVisitor
    {
        public void visitMainWindow(MainWindow window);

        public void visitLayout(StudyIterator layout);

        public void visitStudy(IStudy study);

        public IVisitor getUndoVisitor();
    }
}
