using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    /// <summary>
    /// Command to undo operations
    /// </summary>
    class UndoCom : Command
    {
        public UndoCom(StudyLayout layout) : base(layout)
        {

        }

        /// <summary>
        /// Restores the system to a previous state
        /// </summary>
        public override void Execute()
        {
            if (invoker.CommandStack.Count == 0)
                return;

            Command last;
            do
            {
                last = invoker.CommandStack.Pop();
            }
            while (invoker.CommandStack.Count > 0 && last.SystemState == null);
            
            Command.invoker.Study.Layout = StudyLayout.Reconstruct(last.SystemState);
            Command.invoker.Navigate(Command.invoker.Study.Layout);
            Command.invoker.UpdateCount();
        }

        public override void UnExecute()
        {
            throw new NotImplementedException();
        }
    }
}
