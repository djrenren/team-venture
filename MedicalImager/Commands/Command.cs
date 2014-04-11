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

        public Command(StudyLayout currentState)
        {
            //TODO copy here
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
            invoker.CommandStack.Push(this);
        }

        public StudyLayout SystemState { get; private set; }
    }
}
