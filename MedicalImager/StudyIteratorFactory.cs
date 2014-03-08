using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager
{
    /// <summary>
    /// Factory class that creates new study iterator based of the meta data of a given study 
    /// </summary>
    class StudyIteratorFactory
    {
        /// <summary>
        /// Creates a new study iterator given a study. The new iterator created is determined 
        /// by the meta data associated with the study
        /// </summary>
        /// <param name="study">Any study </param>
        /// <returns>returns a study iterator </returns>
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
