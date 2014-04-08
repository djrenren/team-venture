using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager
{
    class Driver
    {
        public Driver()
        {
            Command.invoker = this;
        }

        public List<Command> commandList;

        public IStudy study { get; set; }
    }
}
