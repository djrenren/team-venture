using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace MedicalImager
{
    [Serializable]
    public class StudyLayoutMemento
    {
        public int Position { get; private set;}
        public List<VirtualImage> Images { get; private set;}
        public string LayoutRepr { get; private set; }

        public StudyLayoutMemento(int position, List<VirtualImage> images, string layoutRepr){
            Position = position;
            LayoutRepr = layoutRepr;
            Images = new List<VirtualImage>();

            // Deepcopy Images using Binary serialization
            VirtualImage curr;
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            foreach(VirtualImage img in images){
                formatter.Serialize(stream, img);
                stream.Position = 0;
                curr = formatter.Deserialize(stream) as VirtualImage;
                stream.Position = 0;
                // Don't store the actual data!
                curr.Source = null;
                Images.Add(curr);
            }


        }
    }
}
