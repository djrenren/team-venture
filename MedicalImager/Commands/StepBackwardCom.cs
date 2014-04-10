using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    class StepBackwardCom : Command
    {
        public StepBackwardCom(StudyLayout layout) : base(layout)
        {
        }

        public override void Execute()
        {
            invoker.Study.Layout.MovePrev();
            AddToList();
        }

        public override void UnExecute()
        {
        }
    }
}
