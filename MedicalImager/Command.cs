using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager
{
    public abstract class Command
    {
        public static Driver invoker = null;

        public Command(StudyIterator currentState)
        {
            SystemState = currentState;
        }
        
        /// <summary>
        /// Executes the command
        /// </summary>
        public abstract void Execute();

        /// <summary>
        /// Reverts the execution of the command
        /// </summary>
        public abstract void UnExecute();

        /// <summary>
        /// Adds the command to the invoker's list of commands
        /// </summary>
        public void AddToList()
        {
            invoker.CommandList.Add(this);
        }

        public StudyIterator SystemState { get; private set; }
    }
}
