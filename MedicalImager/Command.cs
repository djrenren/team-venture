using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager
{
    abstract class Command
    {
        public static Driver invoker = null;
        
        void Execute();

        void UnExecute();
    }
}
