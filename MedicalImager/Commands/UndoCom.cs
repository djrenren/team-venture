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
            // Don't pop if there's nothing to pop
            if (invoker.CommandStack.Count == 0)
                return;
            Command last = invoker.CommandStack.Pop();
            // The SystemState may be null, don't try to reconstruct it
            if (last.SystemState == null) return;
            Command.invoker.Study.Layout = StudyLayout.Reconstruct(last.SystemState);
            Command.invoker.Navigate(Command.invoker.Study.Layout);
        }

        public override void UnExecute()
        {
            throw new NotImplementedException();
        }
    }
}
