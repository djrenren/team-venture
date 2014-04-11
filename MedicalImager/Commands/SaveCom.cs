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
                case SaveType.SaveAs:
                    StudyBrowserDialog open = new StudyBrowserDialog();
                    string path = open.saveStudy();

                    if(path == null)
                    {
                        return;
                    }
                    invoker.Study.Save(new Uri(path));
                    invoker.Study = new LocalStudy(path);
                    invoker.Navigate(invoker.Study.Layout);
                    Command.invoker.CommandStack.Clear();
                    break;
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
