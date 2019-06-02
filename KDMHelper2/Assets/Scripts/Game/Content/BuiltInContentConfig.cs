using Common.IO;
using Common.IO.FileHelpers.CSV;
using Common.IO.FileHelpers.FileLoadSpecializations;
using Game.Content;
using Game.Content.Layout;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.Game.Content
{
    [Serializable]
    public class KeyTextAssetPair
    {
        public string Key;
        public TextAsset Asset;
    }

    [Serializable]
    public class KeySpriteAssetPair
    {
        public string Key;
        public Sprite Asset;
    }

    [Serializable]
    public class KeyLayoutPair
    {
        public string Key;
        public ContentLayoutController Layout;
    }

    public class BuiltInContentConfig : ScriptableObject
    {
        private static BuiltInContentConfig s_Instance = null;

        public static BuiltInContentConfig FindInstance()
        {
            return Resources.Load<BuiltInContentConfig>("BuiltInContentConfig");
        }

#if UNITY_EDITOR
        public static void CreateInstance()
        {
            var resDir = new DirectoryInfo(Path.Combine(Application.dataPath, "Resources"));
            if (!resDir.Exists)
            {
                UnityEditor.AssetDatabase.CreateFolder("Assets", "Resources");
            }
            UnityEditor.AssetDatabase.CreateAsset(s_Instance, "Assets/Resources/BuiltInContentConfig.asset");
        }
#endif

        public static BuiltInContentConfig Instance
        {
            get
            {
                if (s_Instance != null)
                {
                    return s_Instance;
                }

                s_Instance = FindInstance();
                if (s_Instance != null)
                {
                    s_Instance.Initialise();

                    string json = JsonUtility.ToJson(s_Instance, true);
                    string relativeFilePath = string.Format("Resources/BuiltInContentConfigSave_{0}.json", DateTime.UtcNow.ToString("dd-MM-yyyy"));
                    File.WriteAllText(Path.Combine(Application.dataPath, relativeFilePath), json);

                    return s_Instance;
                }

                s_Instance = CreateInstance<BuiltInContentConfig>();
#if UNITY_EDITOR
                CreateInstance();
                s_Instance = FindInstance();
#endif
                s_Instance.Initialise();
                return s_Instance;
            }
        }


        //=======================================================================================================

        public List<KeyTextAssetPair> Content;
        public List<KeySpriteAssetPair> Images;
        public List<KeyLayoutPair> Layouts;


        [NonSerialized]
        public ContentManagerRecord Record;

        private void Initialise()
        {
            Record = new ContentManagerRecord();

            Record.Content = new Dictionary<string, CSVData>();
            int contentCount = Content.Count;
            for (int i = 0; i < contentCount; i++)
            {
                Record.Content[Content[i].Key] = CSVData.CreateFromString(Content[i].Asset.text);
            }

            Record.Images = new Dictionary<string, Sprite>();
            int imagesCount = Images.Count;
            for (int i = 0; i < imagesCount; i++)
            {
                Record.Images[Images[i].Key] = Images[i].Asset;
            }

            Record.Layout = new Dictionary<string, ContentLayoutController>();
            int layoutCount = Layouts.Count;
            for (int i = 0; i < layoutCount; i++)
            {
                Record.Layout[Images[i].Key] = Layouts[i].Layout;
            }
        }
    }
}
