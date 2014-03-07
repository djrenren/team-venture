using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager
{
    class StudyIteratorFactory
    {
        public static StudyIterator Create(IStudy study)
        {
            string meta = study.GetMeta();
            
            if(meta == null)
            {
                return new SingleImageLayout(study);
            }

            string[] lines = meta.Split('\n');
            string repr = lines[0];
            int pos = int.Parse(lines[1]);

            if (repr == SingleImageLayout.Representation)
            {
                return new SingleImageLayout(study, pos);
            }
            else if (repr == TwoByTwoImageLayout.Representation)
            {
                return new TwoByTwoImageLayout(study, pos);
            }
            return null;
        }
    }
}
