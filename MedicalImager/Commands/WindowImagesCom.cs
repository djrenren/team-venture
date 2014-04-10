using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    class WindowImagesCom : Command
    {
        public WindowImagesCom(StudyIterator layout) : base(layout)
        {

        }

        public static void PromptAndCreate()
        {
            WindowDialog d = new WindowDialog();
            int min, max;
            d.ShowDialog();
            if(d.DialogResult == true)
            {
                min = d.
            }
        }

        public override void Execute()
        {

        }

        public override void UnExecute()
        {

        }
    }
}
