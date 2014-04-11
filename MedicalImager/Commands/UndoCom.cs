using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    class UndoCom : Command
    {
        public UndoCom(StudyLayout layout) : base(layout)
        {

        }

        public override void Execute()
        {
            Command last = invoker.CommandStack.Pop();
            Command.invoker.Study.Layout = last.SystemState;
        }

        public override void UnExecute()
        {
            throw new NotImplementedException();
        }
    }
}
