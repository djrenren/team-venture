using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    class StepBackwardCom : Command
    {
        public StepBackwardCom(StudyIterator layout) : base(layout)
        {
        }
    }
}
