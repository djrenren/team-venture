using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    class SaveCom : Command
    {
        private SaveType _saveMethod;

        public SaveCom(StudyIterator currentState, SaveType saveMethod) : base(currentState)
        {
            _saveMethod = saveMethod;
        }

        public override void Execute()
        {
            switch(_saveMethod)
            {
                case SaveType.Save: invoker.Study.Save(); break;
            }
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
