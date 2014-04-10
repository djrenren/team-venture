using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    class SetDefaultCom : Command
    {

        public SetDefaultCom(StudyIterator layout) : base(layout)
        {
        }

        public override void Execute()
        {

        }

        public override void UnExecute()
        {

        }
    }
}
