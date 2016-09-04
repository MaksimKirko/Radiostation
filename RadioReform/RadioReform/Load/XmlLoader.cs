using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using RadioReform.Collections;
using System.IO;

namespace RadioReform.Load
{
    public class XmlLoader<T> : IRead<T>
    {
        private string filename;

        public XmlLoader(string filename)
        {
            this.filename = filename;
        }

        public MyCollection<T> GetItems()
        {
            MyCollection<T> songs = new MyCollection<T>();
            XmlSerializer serializer = new XmlSerializer(typeof(MyCollection<T>));

            using (Stream reader = File.Open(filename, FileMode.Open))
            {
                songs = (MyCollection<T>)serializer.Deserialize(reader);
            }

            return songs;
        }
    }
}
