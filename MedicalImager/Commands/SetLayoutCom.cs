﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager.Commands
{
    class SetLayoutCom : Command
    {

        public SetLayoutCom(StudyIterator layout, Type type) : base(layout)
        {
        }

        public override void Execute()
        {

        }

        public override void UnExecute()
        {

        }
    }
}