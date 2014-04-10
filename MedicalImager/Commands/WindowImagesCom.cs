using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    class WindowImagesCom : Command
    {
        public WindowImagesCom(StudyLayout layout, int min, int max, List<VirtualImage> images) : base(layout)
        {

        }

        public WindowImagesCom(StudyLayout layout, int min, int max, VirtualImage image) : base(layout)
        {

        }


        public static void PromptAndCreate()
        {
            WindowDialog d = new WindowDialog();
            int min, max;
            d.ShowDialog();
            if(d.DialogResult == true)
            {
            
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
