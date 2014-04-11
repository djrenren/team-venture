using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace MedicalImager
{
    public class VirtualImage
    {
        ImageLoader loadingStrategy;

        public VirtualImage(Uri uri)
        {
            // TODO: Complete member initialization
            _uri = uri;
            switch(Path.GetExtension(uri.ToString()))
            {
                case ".jpg": loadingStrategy = new Loaders.JpegLoader(uri); break;
                case ".acr": loadingStrategy = new Loaders.AcrLoader(uri); break;
            }
            Operations = new List<ImageOperation>();
        }

        public VirtualImage(ImageLoader loadStrat)
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
                    Console.WriteLine("operated");
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

        public void AddOperation(ImageOperation op)
        {
            Operations.Add(op);
            _source = op.ApplyOperation(_source);
        }

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
