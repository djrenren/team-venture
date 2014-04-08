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

        public static readonly enum SaveType
        {
            Save,
            SaveAs
        }
    }
}
