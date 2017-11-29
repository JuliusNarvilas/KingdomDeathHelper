﻿using Common;
using Game.IO.InfoDB;
using Game.Properties;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game
{
    public class ApplicationManager : SingletonMonoBehaviour<ApplicationManager>
    {
        public enum EState
        {
            None,
            Loading,
            Ready
        }

        private static EState s_State = EState.None;
        public static EState State { get { return s_State; } }

        public static void Initialise()
        {
            if(s_State == EState.None)
            {
                s_State = EState.Loading;
                SceneManager.LoadSceneAsync("GlobalAdditiveScene", LoadSceneMode.Additive);
            }
        }

        [SerializeField]
        private InfoDBController m_InfoDB;
        public InfoDBController InfoDB { get { return m_InfoDB; } }

        protected new void Awake()
        {
            base.Awake();
            s_State = EState.Ready;
        }

        private void Start()
        {
            XmlSerializerNamespaces s_Namespaces = new XmlSerializerNamespaces();
            s_Namespaces.Add(string.Empty, string.Empty);
            XmlSerializer s_Serializer = new XmlSerializer(typeof(KDMNumericalProperty));

            KDMNumericalProperty test = new KDMNumericalProperty(2);
            test.AddModifier(new KDMCustomNumericalPropertyModifier(3, "testType"));
            test.AddModifier(new KDMCustomNumericalPropertyModifier(-1, "testType"));
            test.AddModifier(new KDMCustomNumericalPropertyModifier(2, "testType"));

            List<KDMNumericalProperty> testList = new List<KDMNumericalProperty>();
            testList.Add(test);
            testList.Add(test);

            MemoryStream mem = new MemoryStream();
            var writer = XmlWriter.Create(mem);
            writer.WriteStartDocument();

            s_Serializer.Serialize(writer, test, s_Namespaces);

            writer.WriteEndDocument();
            writer.Flush();

            mem.Position = 0;
            var sr = new StreamReader(mem);
            var myStr = sr.ReadToEnd();

            Debug.Log(myStr);
            
            mem.Position = 0;

            
            var reader = XmlReader.Create(mem);
            reader.MoveToContent();
            KDMNumericalProperty result = s_Serializer.Deserialize(reader) as KDMNumericalProperty;
            /*
                        Debug.Log(result[0].GetModifier());
                        Debug.Log(result[1].GetType());
                        */

            Debug.Log(result.GetModifierCount());

            mem.Dispose();
        }

        private void OnApplicationQuit()
        {
            //TODO: autosave
        }
    }
}
