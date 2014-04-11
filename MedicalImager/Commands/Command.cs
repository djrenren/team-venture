using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MedicalImager
{
    /// <summary>
    /// Basse class of all Commands
    /// </summary>
    public abstract class Command : ICommand
    {
        public static Driver invoker = null;

        public Command(StudyLayout currentState)
        {
            if(currentState != null)
                SystemState = currentState.GetData();
        }
        
        /// <summary>
        /// Executes the command
        /// </summary>
        public abstract void Execute();


        /// <summary>
        /// Part of the ICommand interface. States if commands are ready to run.
        /// All commands in the system are executable from creation on.
        /// </summary>
        public bool CanExecute(object e)
        {
            return true;
        }

        /// <summary>
        /// Part of the ICommand interface. States if commands are ready to run.
        /// All commands in the system are executable from creation on.
        /// </summary>
        public void Execute(object e)
        {
            Execute();
        }

        /// <summary>
        /// Part of the ICommand interface. 
        /// All commands in the system are executable from creation on.
        /// Event will never fire.
        /// </summary>
        public event EventHandler CanExecuteChanged;


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

        /// <summary>
        /// A Memento representing the state of the system before the command
        /// executed.
        /// </summary>
        public StudyLayoutMemento SystemState { get; private set; }
    }
}
