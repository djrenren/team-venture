using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    class SetLayoutCom : Command
    {

        Type newLayoutType;

        public SetLayoutCom(StudyIterator layout, Type type) : base(layout)
        {
            newLayoutType = type;
        }

        public override void Execute()
        {
            if(newLayoutType == typeof(SingleImageLayout))
            {
                SingleImageLayout newLayout = new SingleImageLayout(invoker.Study, invoker.Study.Layout.Position);
                invoker.Study.Layout = newLayout;
                invoker.Navigate(newLayout);
            } 
            else if (newLayoutType == typeof(TwoByTwoImageLayout))
            {
                TwoByTwoImageLayout newLayout = new TwoByTwoImageLayout(invoker.Study, invoker.Study.Layout.Position);
                invoker.Study.Layout = newLayout;
                invoker.Navigate(newLayout);
            }
            else if (newLayoutType == typeof(CoronalReconstruction))
            {
                CoronalReconstruction newLayout = new CoronalReconstruction(invoker.Study, invoker.Study.Layout.Position);
                invoker.Study.Layout = newLayout;
                invoker.Navigate(newLayout);
            }
            else if (newLayoutType == typeof(SaggitalReconstruction))
            {
                SaggitalReconstruction newLayout = new SaggitalReconstruction(invoker.Study, invoker.Study.Layout.Position);
                invoker.Study.Layout = newLayout;
                invoker.Navigate(newLayout);
            }
        }

        public override void UnExecute()
        {

        }
    }
}
