using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Media.Imaging;

namespace MedicalImager
{
    class SinglePageIterator : StudyIterator
    {
        private Study study;
        public ObservableCollection<BitmapImage> Current {get; set;}
        object IEnumerator.Current
        {
            get
            {
                return Current;
            }
        }
        public SinglePageIterator(Study s) 
        {
            study = s;
            Current = new ObservableCollection<BitmapImage>();
            Current.Insert(0, new BitmapImage());
        }

        public void Reset()
        {

        }

        public bool MoveNext()
        {
            return true;
        }

        public void Dispose()
        {

        }
    }
}
