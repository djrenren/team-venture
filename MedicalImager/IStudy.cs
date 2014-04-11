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
        int Size();
        Uri this[int index]
        {
            get;
            set;
        }

        StudyLayout Layout { get; set; }

        void LoadSavedData();

        void SetDefault();

        void Save();

        void Save(Uri targetUri);

    }
}
