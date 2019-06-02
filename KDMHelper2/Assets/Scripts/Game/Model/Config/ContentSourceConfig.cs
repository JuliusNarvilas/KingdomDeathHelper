using Common;
using Common.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using UnityEngine;

namespace Assets.Game.Model.Config
{
    [Serializable]
    public class ContentSourceRecord
    {
        public string Name;
        public string Path;
    }

    [Serializable]
    public class ContentSourceConfig
    {
        public int Version;

        public List<ContentSourceRecord> Content = new List<ContentSourceRecord>();

        public List<ContentSourceRecord> Images = new List<ContentSourceRecord>();

        public List<ContentSourceRecord> Layouts = new List<ContentSourceRecord>();


        public bool SaveConfigFile()
        {
            try
            {
                using (var fs = new FileStream(string.Format("{0}/{1}", Application.persistentDataPath, "Config.xml"), FileMode.Create))
                {
                    XmlWriterSettings settings = new XmlWriterSettings { Indent = true, IndentChars = "\t", OmitXmlDeclaration = true };
                    using (var writer = XmlWriter.Create(fs, settings))
                    {
                        writer.WriteStartDocument();
                        XMLHelpers.Serialize(this, writer);
                        writer.WriteEndDocument();
                        writer.Flush();
                    }
                    fs.Flush();
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
    }




    [Serializable]
    public class ContentResourceRecord
    {
        public string Name;
        public TextAsset Asset;
    }


    [Serializable]
    public class ContentResourceImageRecord
    {
        public string Name;
        public Sprite Asset;
    }


    [Serializable]
    [CreateAssetMenu(fileName = "DefaultContent", menuName = "Content/Default Content Asset")]
    public class ContentResource : ScriptableObject
    {
        public int Version;

        public List<ContentResourceRecord> Content = new List<ContentResourceRecord>();

        public List<ContentResourceImageRecord> Images = new List<ContentResourceImageRecord>();

        public List<ContentResourceRecord> Layouts = new List<ContentResourceRecord>();



        public ContentSourceConfig GetConfig()
        {
            var result = new ContentSourceConfig();
            result.Version = Version;

            foreach (var record in Content)
            {
                var configRecord = new ContentSourceRecord();
                configRecord.Name = record.Name;
                configRecord.Path = string.Format("{0}.csv", record.Asset.name);

                result.Content.Add(configRecord);
            }

            return result;
        }

        public void SaveContent()
        {
            foreach (var record in Content)
            {
                if (record.Asset != null)
                {
                    try
                    {
                        File.WriteAllText(string.Format("{0}/{1}.csv", Application.persistentDataPath, record.Asset.name), record.Asset.text);
                    }
                    catch (Exception e)
                    {
                        Log.ProductionLogError(e.Message);
                    }
                }
            }

            foreach (var record in Images)
            {
                if (record.Asset != null)
                {
                    try
                    {
                        using (var fs = new FileStream(string.Format("{0}/{1}.png", Application.persistentDataPath, record.Asset.name), FileMode.Create))
                        {
                            PNGHelpers.Serialise(record.Asset, fs);
                        }
                    }
                    catch(Exception e)
                    {
                        Log.ProductionLogError(e.Message);
                    }
                }
            }
        }
    }
}
