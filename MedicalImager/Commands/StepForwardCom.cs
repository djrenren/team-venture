using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    /// <summary>
    /// Command for moving ot the next image
    /// </summary>
    class StepForwardCom : Command
    {

        public StepForwardCom(StudyLayout layout) : base(layout)
        {
        }

        /// <summary>
        /// Attempts to move to the next image
        /// </summary>
        public override void Execute()
        {
            invoker.Study.Layout.MoveNext();
            invoker.UpdateCount();
            AddToList();
        }

        public override void UnExecute()
        {

        }
    }
}
