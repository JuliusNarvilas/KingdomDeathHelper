using Common.Properties.Enumeration;
using Game.Model.Character;
using Game.Model.Events;
using Game.Model.Resources;
using Game.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;

namespace Game.Model
{

    [Serializable]
    public class Campaign
    {
        public static Campaign GetCurrent()
        {
            return null;
        }

        public string Name;
        public List<SettlementTimelineRecord> Events;
        

        [OnDeserialized]
        internal void OnDeserializedMethod(StreamingContext context)
        {

        }


        /*
        public XmlSchema GetSchema()
        {
            return null;
        }
        public void ReadXml(XmlReader reader)
        {
            bool wasEmpty = reader.IsEmptyElement;

            reader.Read();

            if (wasEmpty)
            {
                return;
            }

            while (reader.NodeType != XmlNodeType.EndElement)
            {
                reader.ReadStartElement(ItemTagName);

                reader.ReadStartElement(KeyTagName);
                TKey key = (TKey)keySerializer.Deserialize(reader);
                reader.ReadEndElement();

                reader.ReadStartElement(ValueTagName);
                TValue value = (TValue)valueSerializer.Deserialize(reader);
                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                reader.MoveToContent();
            }

            reader.ReadEndElement();
        }


        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer keySerializer = new XmlSerializer(typeof(TKey));
            XmlSerializer valueSerializer = new XmlSerializer(typeof(TValue));

            writer.WriteElementString("CampaignType", CampaignType.GetValue());

            writer.WriteStartElement(Expansions);
            List<EnumProperty<string>> Expansions
            foreach (TKey key in this.Keys)
            {
                

                writer.WriteStartElement(ValueTagName);
                TValue value = this[key];
                valueSerializer.Serialize(writer, value);
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
        }
        */
    }
}
