﻿
using Common.Helpers;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Game.IO.InfoDB
{

    public class InfoDBController : ScriptableObject
    {
#if UNITY_EDITOR
        [MenuItem("ScriptableObject/Create InfoDBController", false, 100)]
        public static void CreateAsset()
        {
            ScriptableObjectHelper.CreateAsset<InfoDBController>();
        }
#endif

        public List<InfoDBSource> Sources;
        

        public void Reset()
        {
            int count = Sources.Count;
            for (int i = 0; i < count; ++i)
            {
                if (Sources[i] != null)
                {
                    Sources[i].Reset();
                }
            }
        }

        public InfoDBSource Find(string name)
        {
            InfoDBSource result = null;
            int count = Sources.Count;
            for (int i = 0; i < count; ++i)
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