using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager
{
    abstract class Command
    {
        public static Driver invoker = null;

        public Command(StudyIterator currentState)
        {
            SystemState = currentState;
        }
        
        /// <summary>
        /// Executes the command
        /// </summary>
        public void Execute();

        /// <summary>
        /// Reverts the execution of the command
        /// </summary>
        public void UnExecute();

        /// <summary>
        /// Adds the command to the invoker's list of commands
        /// </summary>
        void addToList()
        {
            invoker.commandList.Add(this);
        }

        public StudyIterator SystemState { get; private set; }
    }
}
