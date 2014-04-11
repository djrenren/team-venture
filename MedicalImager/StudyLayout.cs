using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MedicalImager
{
    /// <summary>
    /// Interface for the image layouts 
    /// </summary>
    [Serializable()]
    public abstract class StudyLayout : Page
    {
        public StudyLayout() { }

        protected int _position = -1;


        public StudyLayout(StudyLayoutMemento mem)
        {
            
        }

        /// <summary>
        /// Creates a StudyLayout from the given memento
        /// </summary>
        /// <param name="mem">A StudyLayoutMemento to restore from</param>
        /// <returns>The restored StudyLayout</returns>
        public static StudyLayout Reconstruct(StudyLayoutMemento mem)
        {
            if(mem.LayoutRepr == TwoByTwoImageLayout.Representation)
                return new TwoByTwoImageLayout(mem);
            if(mem.LayoutRepr  == SingleImageLayout.Representation)
                return new SingleImageLayout(mem);
            if(mem.LayoutRepr == CoronalReconstruction.Representation)
                return new CoronalReconstruction(mem);
            if (mem.LayoutRepr == SaggitalReconstruction.Representation)
                return new SaggitalReconstruction(mem);
            return null;
        }

        /// <summary>
        /// Moves to the previous grouping of images
        /// </summary>
        /// <returns>true if the move was successful, false otherwise</returns>
        public abstract bool MovePrev();



        // Extract the representation from any given layout
        public abstract string Repr { get;  }
        
        /// <summary>
        /// Moves to the previous grouping of images
        /// </summary>
        /// <returns>true if the move was successful, false otherwise</returns>
        public abstract bool MoveNext();

        /// <summary>
        /// The current position in the study
        /// </summary>
        public abstract int Position { get; set;}

        /// <summary>
        /// Creates a texy representation that can be written to the disk.
        /// Uneccessary data should be disposed of before serializing.
        /// </summary>
        /// <param name="stream">The filestream to write to</param>
        public abstract void Serialize(FileStream stream);

        /// <summary>
        /// The VirtualImages to be displayed
        /// </summary>
        public abstract List<VirtualImage> Images
        {
            get; set;
        }

        /// <summary>
        /// Returns a StudyLayoutMemento representing the current StudyLayout
        /// </summary>
        /// <returns></returns>
        public StudyLayoutMemento GetData()
        {
            return new StudyLayoutMemento(Position, Images, Repr);
        }

    }
}
