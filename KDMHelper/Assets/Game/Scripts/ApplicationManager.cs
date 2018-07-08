using Common;
using Common.IO;
using Common.Properties.Enumeration;
using Common.Properties.String;
using Game.IO.InfoDB;
using Game.Model;
using Game.Model.Character;
using Game.Properties;
using Game.Properties.Modifiers;
using System;
using System.Collections;
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
            Initialising,
            LoadingData,
            Ready
        }

        private static EState s_State = EState.None;
        public static EState State { get { return s_State; } }

        public static void Initialise()
        {
            if(s_State == EState.None)
            {
                s_State = EState.Initialising;
                SceneManager.LoadSceneAsync("GlobalAdditiveScene", LoadSceneMode.Additive);
            }
        }

        [SerializeField]
        private InfoDBController m_InfoDB;
        public InfoDBController InfoDB { get { return m_InfoDB; } }

        private string m_PersistentDataPath;
        public string PersistentDataPath
        {
            get { return m_PersistentDataPath; }
        }

        public event Action OnScreenSizeChange;

        private float m_LastScreenWidth = 0f;


        protected new void Awake()
        {
            base.Awake();

            m_PersistentDataPath = Application.persistentDataPath;

            s_State = EState.LoadingData;
            StartCoroutine(Load());
        }

        private void Start()
        {













            ///////////////////////////////////////////////////////////////////////
            //TESTING
            //////////////////////////////////////////////////////////////////////
            XmlSerializerNamespaces s_Namespaces = new XmlSerializerNamespaces();
            s_Namespaces.Add(string.Empty, string.Empty);
            XmlSerializer s_Serializer = new XmlSerializer(typeof(KDMNumericalProperty));

            KDMNumericalProperty test = new KDMNumericalProperty(2);
            test.AddModifier(new CustomNumericalPropertyModifier("test", 3));
            test.AddModifier(new CustomNumericalPropertyModifier("test", -1));
            test.AddModifier(new CustomNumericalPropertyModifier("test", 2));

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


            ///////////////////////////////////////////////////////////////////////
            //TESTING
            //////////////////////////////////////////////////////////////////////

            Survivor2 survivor2 = new Survivor2();

            NamedProperty namedProp = new NamedProperty();
            namedProp.Property = new ObservableStringProperty("");
            namedProp.SetName("Name");
            survivor2.Properties.Add(namedProp);

            EnumProperty.Generator gen = EnumProperty.Generator.FindCreateFactory("Gender");
            namedProp = new NamedProperty();
            namedProp.Property = new ObservableEnumProperty(gen.Create("Unknown"));
            namedProp.SetName("Gender");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("HuntXp");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new ObservableStringProperty("");
            namedProp.SetName("Name");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new ObservableStringProperty("");
            namedProp.SetName("Name");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new ObservableStringProperty("");
            namedProp.SetName("Name");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new ObservableStringProperty("");
            namedProp.SetName("Name");
            survivor2.Properties.Add(namedProp);


            mem = new MemoryStream();
            writer = XmlWriter.Create(mem);
            writer.WriteStartDocument();

            XMLHelpers.Serialize(survivor2, writer);

            writer.WriteEndDocument();
            writer.Flush();

            mem.Position = 0;
            sr = new StreamReader(mem);
            myStr = sr.ReadToEnd();

            Debug.Log(myStr);

            mem.Dispose();
















        }

        private void OnEnable()
        {
            m_LastScreenWidth = 0f;
        }

        private void Update()
        {
            if (m_LastScreenWidth != UnityEngine.Screen.width)
            {
                m_LastScreenWidth = UnityEngine.Screen.width;
                if(OnScreenSizeChange != null)
                {
                    OnScreenSizeChange();
                }
            }
        }

        private void OnApplicationQuit()
        {
            //TODO: autosave
        }


        public void DisplaySurvivor(Survivor i_Character)
        {

        }


        private IEnumerator Load()
        {
            if (InfoDB == null || InfoDB.Sources == null)
            {
                yield break;
            }

            InfoDB.Reset();
            int count = InfoDB.Sources.Count;
            for (int i = 0; i < count; ++i)
            {
                if(InfoDB.Sources[i].State == InfoDBSource.EState.Initial)
                {
                    InfoDB.Sources[i].Load();
                }
            }

            bool processing = true;
            while (processing)
            {
                processing = false;
                for (int i = 0; i < count; ++i)
                {
                    if (InfoDB.Sources[i].State <= InfoDBSource.EState.Parsing)
                    {
                        processing = true;
                        break;
                    }
                }
            }

            s_State = EState.Ready;
        }
    }
}
