using UnityEditor;
using UnityEngine;

namespace Common
{
    [CustomPropertyDrawer(typeof(ShowOnlyAttribute))]
    public class ShowOnlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty prop, GUIContent label)
        {
            string valueStr = null;
            switch (prop.propertyType)
            {
                case SerializedPropertyType.Integer:
                    valueStr = prop.intValue.ToString();
                    break;
                case SerializedPropertyType.Boolean:
                    valueStr = prop.boolValue.ToString();
                    break;
                case SerializedPropertyType.Float:
                    valueStr = prop.floatValue.ToString("0.00000");
                    break;
                case SerializedPropertyType.String:
                    valueStr = prop.stringValue;
                    break;
                default:
                    valueStr = null;
                    break;
            }
            if (valueStr == null)
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUI.PropertyField(position, prop, prop.isExpanded);
                }
            }
            else
            {
                var labelPos = position;
                labelPos.width = EditorGUIUtility.labelWidth;
                var contentPos = position;
                contentPos.x += labelPos.width;
                contentPos.width = position.width - labelPos.width;

                EditorGUI.LabelField(labelPos, label.text);
                EditorGUI.SelectableLabel(contentPos, valueStr);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }
}
