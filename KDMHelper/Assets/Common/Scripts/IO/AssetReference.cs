using System;
using UnityEngine;

namespace Common.IO
{

    [Serializable]
    public class AssetReference : IDisposable , ISerializationCallbackReceiver
    {
        public enum ReferenceType
        {
            AssetBundle,
            Resource
        }


#if UNITY_EDITOR
        [SerializeField]
        private UnityEngine.Object m_Asset;
        private DateTime m_LastUpdate;
#endif
        [SerializeField, HideInInspector]
        private AssetReferenceInfo m_Info;
        private AssetReferenceLoadHandle m_LoadHandle;
        private UnityEngine.Object m_LoadedAsset;
        
        public AssetReferenceInfo GetInfo()
        {
            return m_Info;
        }

        public AssetReferenceLoadHandle LoadAsset()
        {
            if (m_Info != null && !string.IsNullOrEmpty(m_Info.FilePath))
            {
                m_LoadHandle = AssetReferenceLoadHandle.Create(m_Info);
            }
            return m_LoadHandle;
        }

        public T GetAsset<T>() where T : UnityEngine.Object
        {
            if(m_LoadedAsset == null && m_LoadHandle != null)
            {
                m_LoadedAsset = m_LoadHandle.GetAsset<T>();
            }
            return m_LoadedAsset as T;
        }

        public bool IsDone()
        {
            return m_LoadHandle == null || m_LoadHandle.IsDone();
        }

        public void Dispose()
        {
            if (m_LoadHandle != null)
            {
                m_LoadHandle.Dispose();
                m_LoadHandle = null;
                m_LoadedAsset = null;
            }
        }

        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (m_Asset != null)
            {
                //if the inspector is focused on something with AssetReference,
                //OnBeforeSerialize() is spamed
                var now = DateTime.UtcNow;
                var timeDiff = now - m_LastUpdate;
                if (timeDiff.TotalSeconds >= 1.0)
                {
                    m_LastUpdate = now;
                    m_Info = AssetReferenceInfo.Create(m_Asset);
                }
            }
            else
            {
                m_Info = null;
            }
#endif
        }

        public void OnAfterDeserialize()
        {
        }
    }
}
