using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    class WindowImagesCom : Command
    {
        private int _min;
        private int _max;
        private WindowOp _op;
        private List<VirtualImage> _images;

        public WindowImagesCom(StudyLayout layout, int min, int max, List<VirtualImage> images) : base(layout)
        {
            _min = min;
            _max = max;
            _images = images;
            _op = new WindowOp(min, max);
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
                if (d.Min > 0 && !(d.Min >= d.Max) && d.Max <= 255)
                {

                }
            
            }
        }

        public override void Execute()
        {
            foreach(VirtualImage i in _images)
            {
                i.AddOperation(_op);
            }
        }

        public override void UnExecute()
        {

        }
    }
}
