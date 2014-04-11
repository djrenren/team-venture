using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    /// <summary>
    /// Command to set the current study as the default
    /// </summary>
    class SetDefaultCom : Command
    {

        public SetDefaultCom(StudyLayout layout) : base(layout)
        {
        }

        /// <summary>
        /// Sets the current study as the default
        /// </summary>
        public override void Execute()
        {

            invoker.Study.SetDefault();
        }

        public override void UnExecute()
        {

        }
    }
}
