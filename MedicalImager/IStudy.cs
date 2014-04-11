using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Drawing;

namespace MedicalImager
{
    /// <summary>
    /// Interface for remoteStudy and study
    /// </summary>
    public interface IStudy
    {
        /// <summary>
        /// Returns the size of th study
        /// </summary>
        /// <returns></returns>
        int Size();

        /// <summary>
        /// Study's can be accessed using bracket notation to retriece Uris
        /// </summary>
        /// <param name="index">the index of the Uri to access</param>
        /// <returns>a Uri leading to an image in the study</returns>
        Uri this[int index]
        {
            get;
            set;
        }

        /// <summary>
        /// Gets and sets the current layout
        /// </summary>
        StudyLayout Layout { get; set; }

        /// <summary>
        /// Load the study from memory
        /// </summary>
        void LoadSavedData();

        /// <summary>
        /// Sets the study as the default
        /// </summary>
        void SetDefault();

        /// <summary>
        /// Saves the study
        /// </summary>
        void Save();

        /// <summary>
        /// Saves the study to a specified target
        /// </summary>
        /// <param name="targetUri">The directory to save into</param>
        void Save(Uri targetUri);

    }
}
