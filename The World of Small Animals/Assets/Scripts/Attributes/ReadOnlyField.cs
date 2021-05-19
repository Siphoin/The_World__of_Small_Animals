using UnityEditor;
using UnityEngine;
/// <summary>
/// Позволяет отображать поле в редакторе, но без возможности изменения самого поля. Для правильной работоспособности атрибута требуется дополнительно атрибут SerializeField
/// </summary>
public class ReadOnlyField : PropertyAttribute
{
#if UNITY_EDITOR
    [CustomPropertyDrawer(typeof(ReadOnlyField))]

    public class ReadOnlyElement : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, true);
            GUI.enabled = true;
        }
    }
#endif
}