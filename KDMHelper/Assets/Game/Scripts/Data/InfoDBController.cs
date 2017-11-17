
using Common.Helpers;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Game.Data
{

    public class InfoDBController : ScriptableObject
    {
        public List<InfoDBSource> Sources;

        [MenuItem("ScriptableObject/Create InfoDBController", false, 100)]
        public static void CreateAsset()
        {
            ScriptableObjectHelper.CreateAsset<InfoDBController>();
        }

        public InfoDBSource Find(string name)
        {
            InfoDBSource result = null;
            for (int i = 0; i < Sources.Count; ++i)
            {
                if (Sources[i].Name == name)
                {
                    result = Sources[i];
                    break;
                }
            }
            return result;
        }
    }

}