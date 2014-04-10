using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace MedicalImager
{
    public class StudyImage
    {
        public StudyImage(Uri uri)
        {
            // TODO: Complete member initialization
            _uri = uri;
            Operations = new List<ImageOperation>();
        }

        private Uri _uri;

        private BitmapSource _source;
        public BitmapSource Source { 
            get
            {
                if(_source == null)
                {
                    _source = new BitmapImage(_uri);
                    foreach(ImageOperation op in Operations)
                    {
                        _source = op.ApplyOperation(_source);
                    }
                }
                return _source;
            }
            set
            {
                if(_source != value)
                    _source = value;
            }
        }

        public List<ImageOperation> Operations { get; set; }

        public BitmapImage getBitmapImage()
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            MemoryStream memoryStream = new MemoryStream();
            BitmapImage bImg = new BitmapImage();

            encoder.Frames.Add(BitmapFrame.Create(Source));
            encoder.Save(memoryStream);

            bImg.BeginInit();
            bImg.StreamSource = new MemoryStream(memoryStream.ToArray());
            bImg.EndInit();

            memoryStream.Close();

            return bImg;
        }
    }
}
