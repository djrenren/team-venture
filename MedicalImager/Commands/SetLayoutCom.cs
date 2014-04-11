using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;


namespace MedicalImager.Commands
{
    class SetLayoutCom : Command
    {

        Type newLayoutType;

        public SetLayoutCom(StudyLayout layout, Type type) : base(layout)
        {
            newLayoutType = type;
            //Opens a file and serializes the object into it in binary format.
        
        }

        public override void Execute()
        {
            StudyLayoutMemento Data = invoker.Study.Layout.GetData();
            if(newLayoutType == typeof(SingleImageLayout))
            {
                SingleImageLayout newLayout = new SingleImageLayout(Data);
               
                invoker.Study.Layout = newLayout;
                invoker.Navigate(newLayout);

 
            } 
            else if (newLayoutType == typeof(TwoByTwoImageLayout))
            {
                TwoByTwoImageLayout newLayout = new TwoByTwoImageLayout(Data);

                invoker.Study.Layout = newLayout;
                invoker.Navigate(newLayout);

               
            }
            else if (newLayoutType == typeof(CoronalReconstruction))
            {
                CoronalReconstruction newLayout = new CoronalReconstruction(Data);
                invoker.Study.Layout = newLayout;
                invoker.Navigate(newLayout);

                
            }
            else if (newLayoutType == typeof(SaggitalReconstruction))
            {
                SaggitalReconstruction newLayout = new SaggitalReconstruction(Data);

                invoker.Study.Layout = newLayout;
                invoker.Navigate(newLayout);


                 
            }

            AddToList();

       

        }

        public override void UnExecute()
        {

        }
    }
}
