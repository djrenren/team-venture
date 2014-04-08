using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    class SaveCom : Command
    {
        public SaveCom(StudyIterator currentState, SaveType saveMethod) : base(currentState)
        {

        }

        public override void Execute()
        {

        }

        public override void UnExecute()
        {

        }

        public enum SaveType
        {
            Save,
            SaveAs
        }
    }
}
