using RadioReform.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace RadioReform.Save
{
    class XmlSaver<T> : IWrite<T>
    {
        private string filename;
        private MyCollection<T> collection;

        public XmlSaver(string filename, MyCollection<T> collection)
        {
            this.filename = filename;
            this.collection = collection;
        }

        public void Write()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(MyCollection<T>));
            StreamWriter stream = new StreamWriter(filename);
            serializer.Serialize(stream, collection);
            stream.Close();
        }
    }
}
