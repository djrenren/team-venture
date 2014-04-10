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
        ImageLoader loadingStrategy;

        public StudyImage(Uri uri)
        {
            // TODO: Complete member initialization
            _uri = uri;
            switch(Path.GetExtension(uri.ToString()))
            {
                case ".jpg": loadingStrategy = new Loaders.JpegLoader(uri); break;
            }
            Operations = new List<ImageOperation>();
        }

        public StudyImage(ImageLoader loadStrat)
        {
            loadingStrategy = loadStrat;
            Operations = new List<ImageOperation>();
        }

        private Uri _uri;

        private BitmapSource _source;
        public BitmapSource Source { 
            get
            {
                if(_source == null)
                {
                    _source = loadingStrategy.LoadImage();
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
