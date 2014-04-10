using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    class StepForwardCom : Command
    {

        public StepForwardCom(StudyLayout layout) : base(layout)
        {
        }

        public override void Execute()
        {
            invoker.Study.Layout.MoveNext();
            AddToList();
        }

        public override void UnExecute()
        {

        }
    }
}
