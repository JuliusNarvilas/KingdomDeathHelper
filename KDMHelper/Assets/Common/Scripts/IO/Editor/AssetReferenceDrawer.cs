 using Common.IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace Common.IO
{
    [CustomPropertyDrawer(typeof(AssetReference))]
    public class AssetReferenceDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect pos, SerializedProperty prop, GUIContent label)
        {
            // Check if the field has a type attribute, and get the type:
            int dotIndex = prop.propertyPath.IndexOf('.');
            string fieldName = dotIndex == -1 ? prop.propertyPath : prop.propertyPath.Substring(0, dotIndex);

            FieldInfo fieldInfo = prop.serializedObject.targetObject.GetType().GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            AssetReferenceTypeAttribute typeAttribute = (AssetReferenceTypeAttribute)Attribute.GetCustomAttribute(fieldInfo, typeof(AssetReferenceTypeAttribute));
            Type desiredType = typeAttribute != null ? typeAttribute.Type : typeof(UnityEngine.Object);


            // Begin the property:
            EditorGUI.BeginProperty(pos, label, prop);
            // Check for changes in the property:
            EditorGUI.BeginChangeCheck();
            
            SerializedProperty assetProperty = prop.FindPropertyRelative("m_Asset");
            //label.text = "{" + label.text + "}";

            assetProperty.objectReferenceValue = EditorGUI.ObjectField(pos, label, assetProperty.objectReferenceValue, desiredType, false);
            AssetReferenceInfo info = null;

            // If an object has been assigned, check if there is some problem with it:
            if (EditorGUI.EndChangeCheck() && assetProperty.objectReferenceValue != null)
            {
                string errorString = null;
                info = AssetReferenceInfo.Create(assetProperty.objectReferenceValue);

                if (info == null)
                {
                    errorString = "The assigned asset is not available for dynamic loading.";
                }
                /*
                else
                {
                    UnityEngine.Object otherAsset = DelayedAsset.FindAssetWithSameTypeAndRelativePath(assetProperty.objectReferenceValue, assetRelativePath, desiredType);
                    if (otherAsset != null)
                    {
                        errorString = "The assigned asset doesn't have a unique type and path relative to a \"Resources\" folder, see the asset \"" + AssetDatabase.GetAssetPath(otherAsset) + "\".";
                    }
                }
                */


                if (errorString != null)
                {
                    assetProperty.objectReferenceValue = null;
                    EditorUtility.DisplayDialog("AssetReference error", errorString, "OK");
                    Log.DebugLogError("AssetReference asset error: " + errorString);
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
