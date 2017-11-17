using System;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Common.IO
{
    [Serializable]
    public class AssetReferenceInfo
    {
        public AssetReference.ReferenceType ReferenceType;
        public string FilePath;
        public string Src;
        public string Name;
        public string SubAssetName;
        public string AssetTypeStr;

        private Type m_AssetType;
        public Type GetAssetType()
        {
            if (m_AssetType == null && AssetTypeStr != null)
            {
                m_AssetType = System.Type.GetType(AssetTypeStr);
            }
            return m_AssetType;
        }

        public static AssetReferenceInfo Create(UnityEngine.Object i_Asset)
        {
#if UNITY_EDITOR
            AssetReferenceInfo result = null;
            string assetPath = AssetDatabase.GetAssetPath(i_Asset);
            var importer = AssetImporter.GetAtPath(assetPath);
            string bundleName = importer.assetBundleName;
            int resourcesMatchIndex;

            if (!string.IsNullOrEmpty(bundleName))
            {
                result = new AssetReferenceInfo()
                {
                    ReferenceType = AssetReference.ReferenceType.AssetBundle,
                    Src = bundleName
                };
            }
            else if ((resourcesMatchIndex = assetPath.IndexOf("/Resources/")) >= 0)
            {
                var relativePath = assetPath.Substring(resourcesMatchIndex + "/Resources/".Length);
                result = new AssetReferenceInfo()
                {
                    ReferenceType = AssetReference.ReferenceType.Resource,
                    Src = Path.ChangeExtension(relativePath, null)
                };
            }
            else
            {
                //Detect asset bundle assets that are simply nested in asset bundle directories
                string folderBundleName = FindDirectoryAssetBundle(assetPath);
                if (!string.IsNullOrEmpty(folderBundleName))
                {
                    result = new AssetReferenceInfo()
                    {
                        ReferenceType = AssetReference.ReferenceType.AssetBundle,
                        Src = folderBundleName
                    };
                }
            }

            if (result != null)
            {
                result.FilePath = assetPath;
                result.Name = Path.GetFileNameWithoutExtension(assetPath);
                var firstPathAsset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
                //detect if selected asset is subasset
                if (i_Asset.name != result.Name || i_Asset.GetType() != firstPathAsset.GetType())
                {
                    result.SubAssetName = i_Asset.name;
                }
                result.AssetTypeStr = i_Asset.GetType().AssemblyQualifiedName;
            }
            return result;
#else
            throw new NotSupportedException();
#endif
        }

        /// <summary>
        /// Recursive call to find asset bundle directory information for a given asset file path.
        /// </summary>
        /// <param name="filePath">Asset filepath.</param>
        /// <returns>Indicated file's asset bundle directory information or null if no such directory exists.</returns>
        private static string FindDirectoryAssetBundle(string filePath)
        {
#if UNITY_EDITOR
            if (filePath == null)
            {
                return null;
            }
            int lastSeperatorIndex = filePath.LastIndexOf("/");
            if (lastSeperatorIndex <= 0)
                return null;

            filePath = filePath.Substring(0, lastSeperatorIndex);
            var attributes = File.GetAttributes(filePath);
            if ((attributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                var importer = AssetImporter.GetAtPath(filePath);
                string bundleName = importer.assetBundleName;
                if (!string.IsNullOrEmpty(bundleName))
                {
                    return bundleName;
                }
            }
            return FindDirectoryAssetBundle(filePath);
#else
            throw new NotSupportedException();
#endif
        }
    }
}
