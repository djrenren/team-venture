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

        public SaveCom(StudyLayout currentState, SaveType saveMethod) : base(null)
        {
            _saveMethod = saveMethod;
        }

        public override void Execute()
        {
            switch(_saveMethod)
            {
                case SaveType.Save: invoker.Study.Save(); break;
            }

            AddToList();
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
