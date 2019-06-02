using Game.Model;
using Game.Model.Character;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Game.IO.Save
{

    [Serializable]
    public class SaveData
    {
        private static readonly XmlSerializerNamespaces s_Namespaces;
        private static readonly XmlSerializer s_Serializer;

        static SaveData()
        {
            s_Namespaces = new XmlSerializerNamespaces();
            s_Namespaces.Add(string.Empty, string.Empty);
            s_Serializer = new XmlSerializer(typeof(SaveData));
        }

        public static SaveData Load(Stream i_Stream)
        {
            var reader = XmlReader.Create(i_Stream);
            reader.MoveToContent();
            SaveData result = s_Serializer.Deserialize(reader) as SaveData;
            return result;
        }



        public Campaign Campaign;
        public List<Survivor> Survivors;



        public void Save(Stream o_Stream)
        {
            var writer = XmlWriter.Create(o_Stream);
            writer.WriteStartDocument();

            s_Serializer.Serialize(writer, this, s_Namespaces);

            writer.WriteEndDocument();
            writer.Flush();
        }
    }
}
