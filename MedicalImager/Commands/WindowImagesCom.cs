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
            _min = min;
            _max = max;
            _images = new List<VirtualImage>();
            _images.Add(image);
            _op = new WindowOp(min, max);
        }


        public static void PromptAndCreate()
        {
            PromptAndCreate(null);
        }

        public static void PromptAndCreate(VirtualImage img)
        {
            WindowDialog d = new WindowDialog();
            int min, max;
            d.ShowDialog();
            if (d.DialogResult == true)
            {
                min = d.Min;
                max = d.Max;
                if(img == null)
                    (new Commands.WindowImagesCom(Command.invoker.Study.Layout,
                        min,
                        max,
                        Command.invoker.Study.Layout.Images)).Execute();
                else
                    (new Commands.WindowImagesCom(Command.invoker.Study.Layout,
                        min,
                        max,
                        img)).Execute();
            }
        }

        public override void Execute()
        {
            foreach(VirtualImage i in _images)
            {
                i.AddOperation(_op);
            }
            //forces the study to refresh the displayed images
            invoker.Study.Layout.Position = invoker.Study.Layout.Position;
        }

        public override void UnExecute()
        {

        }
    }
}
