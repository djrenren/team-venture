using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace MedicalImager
{
    /// <summary>
    /// Allows for modifications of images and is used to display studies
    /// and reconstructions
    /// </summary>
    [Serializable]
    public class VirtualImage
    {
        //the strategy for loading the image
        ImageLoader loadingStrategy;

        /// <summary>
        /// Creates a new VirtualImage given a Uri. The Uri will be parsed to
        /// determine the appropriate loading strategy
        /// </summary>
        /// <param name="uri"></param>
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

        /// <summary>
        /// Creates a VirtualImage that will use the provided ImageLoader
        /// </summary>
        /// <param name="loadStrat">An ImageLoader to load the image</param>
        public VirtualImage(ImageLoader loadStrat)
        {
            loadingStrategy = loadStrat;
            Operations = new List<ImageOperation>();
        }

        private Uri _uri;

        [NonSerialized]
        private BitmapSource _source;

        /// <summary>
        /// Gets and set the BitmapSource. Loads the image using the ImageLoader
        /// then applies all image operations
        /// </summary>
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

        /// <summary>
        /// The ImageOperations for this image
        /// </summary>
        public List<ImageOperation> Operations { get; set; }

        /// <summary>
        /// Adds an image operation and applies it if necessary
        /// </summary>
        /// <param name="op"></param>
        public void AddOperation(ImageOperation op)
        {
            Operations.Add(op);
            if(_source != null)
                _source = op.ApplyOperation(_source);
        }

        /// <summary>
        /// Converts the image to a BitmapImage
        /// </summary>
        /// <returns></returns>
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
