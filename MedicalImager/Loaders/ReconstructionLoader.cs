using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MedicalImager.Loaders
{
    [Serializable]
    public class ReconstructionLoader : ImageLoader
    {
        private List<VirtualImage> _sourceImages;
        private int _position;
        private ReconstructionType _rtype;
        public ReconstructionLoader(List<VirtualImage> sourceImages, int position, ReconstructionType rtype)
        {
            _sourceImages = sourceImages;
            _position = position;
            _rtype = rtype;
        }

        public BitmapSource LoadImage()
        {
            int imageWidth;
            int imageHeight;
            int numSlices;
            BitmapImage sample = _sourceImages.Count > 0 ? _sourceImages.ElementAt(0).getBitmapImage() : null;

            //coronal reconstruction
            if(_rtype == ReconstructionType.Coronal)
            {
                if (sample == null)
                {
                    imageWidth = 0;
                    imageHeight = 0;
                    numSlices = 0;
                }
                else
                {
                    imageWidth = sample.PixelWidth;
                    imageHeight = _sourceImages.Count;
                    numSlices = sample.PixelHeight;
                }

                Console.WriteLine("w: " + imageWidth + " h: " + imageHeight + "position: " + _position);
                PixelFormat pf = PixelFormats.Bgr32;
                int rawStride = (imageWidth*pf.BitsPerPixel+7)/8;
                byte[] rawImage = new byte[rawStride * imageHeight];
                for(int i = 0; i < _sourceImages.Count; i++)
                {
                    _sourceImages.ElementAt(i).Source.CopyPixels(new Int32Rect(0, numSlices - _position - 1, imageWidth, 1), 
                        rawImage, 
                        rawStride, 
                        (_sourceImages.Count-i-1)*(rawStride));
                }

               // Create a BitmapSource.
                BitmapSource bitmap = BitmapSource.Create(imageWidth, imageHeight,
                    96, 96, pf, null,
                    rawImage, rawStride);
                return bitmap;
            }

            //saggital reconstruction
            else if(_rtype == ReconstructionType.Saggital)
            {
                if (sample == null)
                {
                    imageWidth = 0;
                    imageHeight = 0;
                    numSlices = 0;
                }
                else
                {
                    imageWidth = sample.PixelHeight;
                    imageHeight = _sourceImages.Count;
                    numSlices = sample.PixelWidth;
                }

                Console.WriteLine("w: " + imageWidth + " h: " + imageHeight);
                PixelFormat pf = PixelFormats.Bgr32;
                int rawStride = (imageWidth * pf.BitsPerPixel + 7) / 8;
                byte[] rawImage = new byte[rawStride * imageHeight];
                for (int i = 0; i < _sourceImages.Count; i++)
                {

                    _sourceImages.ElementAt(i).Source.CopyPixels(new Int32Rect(numSlices - _position - 1, 0, 1, imageWidth),
                        rawImage,
                        4,
                        (_sourceImages.Count - i - 1) * (rawStride));
                }

                // Create a BitmapSource.
                BitmapSource bitmap = BitmapSource.Create(imageWidth, imageHeight,
                    96, 96, pf, null,
                    rawImage, rawStride);
                return bitmap;
            }

            return null;
        }
    }

    public enum ReconstructionType
    {
        Coronal,
        Saggital
    }
}
