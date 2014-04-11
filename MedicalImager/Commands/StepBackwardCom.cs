using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    /// <summary>
    /// Command for moving to the previous image
    /// </summary>
    class StepBackwardCom : Command
    {
        public StepBackwardCom(StudyLayout layout) : base(layout)
        {
        }

        /// <summary>
        /// Attempts to move to the previous image
        /// </summary>
        public override void Execute()
        {
            invoker.Study.Layout.MovePrev();
            invoker.UpdateCount();
            AddToList();
        }

        public override void UnExecute()
        {
        }
    }
}
