using Common.IO.FileHelpers;
using Common.IO.FileHelpers.FileLoadSpecializations;
using Assets.Game.Model.Config;
using Assets.Game.Scripts.DisplayHandler;
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

        [SerializeField]
        private string m_dataSourceConfigFile;

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

        [SerializeField]
        private DisplayHandlerDB m_displayHandlerDB;
        public DisplayHandlerDB DisplayHandlerDB { get { return m_displayHandlerDB; } }


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
            namedProp.Property = new KDMExhaustibleNumericalProperty(16);
            (namedProp.Property as KDMExhaustibleNumericalProperty).Deplete(16);
            namedProp.SetName("HuntXp");
            survivor2.Properties.Add(namedProp);



            namedProp = new NamedProperty();
            namedProp.Property = new KDMExhaustibleNumericalProperty(8);
            (namedProp.Property as KDMExhaustibleNumericalProperty).Deplete(8);
            namedProp.SetName("Weapon Proficiency");
            survivor2.Properties.Add(namedProp);

            gen = EnumProperty.Generator.FindCreateFactory("Weapon Specialisation");
            namedProp = new NamedProperty();
            namedProp.Property = new ObservableEnumProperty(gen.Create("Unknown"));
            namedProp.SetName("Weapon Specialisation");
            survivor2.Properties.Add(namedProp);


            namedProp = new NamedProperty();
            namedProp.Property = new KDMExhaustibleNumericalProperty(0);
            namedProp.SetName("Survival");
            survivor2.Properties.Add(namedProp);


            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(5);
            namedProp.SetName("Movement");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("Accuracy");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("Strength");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("Evasion");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("Luck");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("Speed");
            survivor2.Properties.Add(namedProp);



            namedProp = new NamedProperty();
            namedProp.Property = new KDMExhaustibleNumericalProperty(9);
            (namedProp.Property as KDMExhaustibleNumericalProperty).Deplete(9);
            namedProp.SetName("Courage");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMExhaustibleNumericalProperty(9);
            (namedProp.Property as KDMExhaustibleNumericalProperty).Deplete(9);
            namedProp.SetName("Understanding");
            survivor2.Properties.Add(namedProp);




            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("Brain");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("BrainInjury");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("Arms");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("ArmsInjury");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("Body");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("BodyInjury");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("Waist");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("WaistInjury");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("Legs");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("LegsInjury");
            survivor2.Properties.Add(namedProp);








            namedProp = new NamedProperty();
            namedProp.Property = new KDMListProperty();
            (namedProp.Property as KDMListProperty).AllowedTypes.Add(new PropertyType(typeof(KDMStringProperty)));
            (namedProp.Property as KDMListProperty).AllowedTypes.Add(new PropertyType(typeof(EnumProperty), "Fighting Arts"));
            (namedProp.Property as KDMListProperty).AllowedTypes.Add(new PropertyType(typeof(EnumProperty), "Secret Fighting Arts"));
            namedProp.SetName("Fighting Arts");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMListProperty();
            (namedProp.Property as KDMListProperty).AllowedTypes.Add(new PropertyType(typeof(KDMStringProperty)));
            (namedProp.Property as KDMListProperty).AllowedTypes.Add(new PropertyType(typeof(EnumProperty), "Disorders"));
            namedProp.SetName("Disorders");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new KDMListProperty();
            (namedProp.Property as KDMListProperty).AllowedTypes.Add(new PropertyType(typeof(KDMStringProperty)));
            (namedProp.Property as KDMListProperty).AllowedTypes.Add(new PropertyType(typeof(EnumProperty), "Abilities"));
            (namedProp.Property as KDMListProperty).AllowedTypes.Add(new PropertyType(typeof(EnumProperty), "Impairments"));
            namedProp.SetName("Abilities & Impairments");
            survivor2.Properties.Add(namedProp);

            /*
            namedProp = new NamedProperty();
            namedProp.Property = new KDMNumericalProperty(0);
            namedProp.SetName("ArmsInjury");
            survivor2.Properties.Add(namedProp);



            namedProp = new NamedProperty();
            namedProp.Property = new ObservableStringProperty("");
            namedProp.SetName("Name");
            survivor2.Properties.Add(namedProp);

            namedProp = new NamedProperty();
            namedProp.Property = new ObservableStringProperty("");
            namedProp.SetName("Name");
            survivor2.Properties.Add(namedProp);
            */

            mem = new MemoryStream();
            writer = XmlWriter.Create(mem);
            writer.WriteStartDocument();

            XMLHelpers.Serialize(survivor2, writer);

            writer.WriteEndDocument();
            writer.Flush();

            mem.Position = 0;
            sr = new StreamReader(mem);
            myStr = sr.ReadToEnd();

            File.WriteAllText("D:\\Temp\\test\\out.xml", myStr);
            //Debug.Log(myStr);

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
            ContentResource defaultContent = Resources.Load<ContentResource>("DefaultContent");
            ContentSourceConfig contentSrc = null;

            XMLLoadHandleT<ContentSourceConfig> loadSrcConfig = new XMLLoadHandleT<ContentSourceConfig>("Config.xml");
            loadSrcConfig.BlockingLoad();
            if (loadSrcConfig.State == FileLoadHandle.EState.Ready)
            {
                contentSrc = loadSrcConfig.GetResultT();
                //TODO: remove " + 1" to stop always writing data
                if (contentSrc.Version >= 0 && contentSrc.Version < defaultContent.Version + 1)
                {
                    contentSrc = defaultContent.GetConfig();
                    defaultContent.SaveContent();
                    contentSrc.SaveConfigFile();
                }
            }
            else
            {
                contentSrc = defaultContent.GetConfig();
                defaultContent.SaveContent();
                contentSrc.SaveConfigFile();
            }



            List<CSVLoadHandle> contentCSVHandles = new List<CSVLoadHandle>();

            int count = contentSrc.Content.Count;
            for (int i = 0; i < count; i++)
            {
                ContentSourceRecord rec = contentSrc.Content[i];
                CSVLoadHandle newHandle = new CSVLoadHandle(rec.Path);
                //newHandle.AsyncLoad();
                contentCSVHandles.Add(newHandle);
            }



            if (InfoDB == null || InfoDB.Sources == null)
            {
                yield break;
            }
            
            /*
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
            */
            s_State = EState.Ready;
        }
    }
}
