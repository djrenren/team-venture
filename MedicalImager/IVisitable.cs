using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager
{
    interface IVisitable
    {
        public void accept(IVisitor v);
    }
}
