using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    /// <summary>
    /// Command that applies windowing operations to images
    /// </summary>
    class WindowImagesCom : Command
    {
        private int _min;
        private int _max;
        private WindowOp _op;
        private List<VirtualImage> _images;

        /// <summary>
        /// Creates a window command for multiple images
        /// </summary>
        /// <param name="layout">the current layout</param>
        /// <param name="min">the windowing min value</param>
        /// <param name="max">the windowing max value</param>
        /// <param name="images">the images to window</param>
        public WindowImagesCom(StudyLayout layout, int min, int max, List<VirtualImage> images) : base(layout)
        {
            _min = min;
            _max = max;
            _images = images;
            _op = new WindowOp(min, max);
        }

        /// <summary>
        /// Creates a window command for a single image
        /// </summary>
        /// <param name="layout">the current layout</param>
        /// <param name="min">the windowing min val</param>
        /// <param name="max">the windowing max val</param>
        /// <param name="image">the image to window</param>
        public WindowImagesCom(StudyLayout layout, int min, int max, VirtualImage image) : base(layout)
        {
            _min = min;
            _max = max;
            _images = new List<VirtualImage>();
            _images.Add(image);
            _op = new WindowOp(min, max);
        }

        /// <summary>
        /// Prompts the user for windowing values for all images
        /// </summary>
        /// <returns>a windowing command</returns>
        public static WindowImagesCom PromptAndCreate()
        {
            return PromptAndCreate(null);
        }

        /// <summary>
        /// Prompts the user for windowing values and creates a windowing command
        /// if necessary
        /// </summary>
        /// <param name="img">the image to window, null if all images should be windowed</param>
        /// <returns>a windowing command</returns>
        public static WindowImagesCom PromptAndCreate(VirtualImage img)
        {
            WindowDialog d = new WindowDialog();
            int min, max;
            d.ShowDialog();
            if (d.DialogResult == true)
            {
                min = d.Min;
                max = d.Max;
                if(img == null)
                    return (new Commands.WindowImagesCom(Command.invoker.Study.Layout,
                        min,
                        max,
                        Command.invoker.Study.Layout.Images));
                else
                    return (new Commands.WindowImagesCom(Command.invoker.Study.Layout,
                        min,
                        max,
                        img));
            }
            return null;
        }

        /// <summary>
        /// Applies the WindowOp to the necessary images
        /// </summary>
        public override void Execute()
        {
            foreach(VirtualImage i in _images)
            {
                i.AddOperation(_op);
            }
            //forces the study to refresh the displayed images
            invoker.Study.Layout.Position = invoker.Study.Layout.Position;
            AddToList();
        }

        public override void UnExecute()
        {

        }
    }
}
