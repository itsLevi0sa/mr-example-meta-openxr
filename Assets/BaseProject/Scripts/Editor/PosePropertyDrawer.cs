using UnityEditor;
using UnityEngine;

namespace KinemotikVR
{

    [CustomPropertyDrawer(typeof(Pose))]
    internal class EulerPosePropertyDrawer : PropertyDrawer
    {
        private float _lineHeight => EditorGUIUtility.singleLineHeight * (EditorGUIUtility.wideMode ? 1f : 2f);
        private Vector3? _eulerAngles;
        private bool _foldout;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            int lines = _foldout ? 3 : 1;

            return lines * _lineHeight + (lines - 1) * EditorGUIUtility.standardVerticalSpacing;
        }

        public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (fieldInfo.FieldType != typeof(Pose))
            {
                GUI.Label(rect, $"'{property.propertyPath}' is not a Pose");
                return;
            }

            SerializedProperty positionProperty = property.FindPropertyRelative("position");
            SerializedProperty rotationProperty = property.FindPropertyRelative("rotation");

            Vector3 position = positionProperty.vector3Value;
            Vector3 euler;

            if (_eulerAngles.HasValue && Quaternion.Euler(_eulerAngles.Value) == rotationProperty.quaternionValue)
            {
                euler = _eulerAngles.Value;
            }
            else
            {
                euler = rotationProperty.quaternionValue.eulerAngles;

                if (euler.x > 180) euler.x -= 360;
                if (euler.y > 180) euler.y -= 360;
                if (euler.z > 180) euler.z -= 360;

                euler = new Vector3(RoundForInspector(euler.x), RoundForInspector(euler.y), RoundForInspector(euler.z));
            }

            _foldout = EditorGUI.Foldout(new Rect(rect.x, rect.y - EditorGUIUtility.standardVerticalSpacing, rect.width, _lineHeight), _foldout, property.displayName, true);

            if (_foldout)
            {
                EditorGUI.indentLevel += 1;

                var positionLabel = new GUIContent("Position");
                var rotationLabel = new GUIContent("Rotation");

                var positionRect = new Rect(rect.x, rect.y + _lineHeight, rect.width, _lineHeight);
                var rotationRect = new Rect(rect.x, rect.y + _lineHeight * 2 + EditorGUIUtility.standardVerticalSpacing, rect.width, _lineHeight);

                EditorGUI.BeginProperty(positionRect, label, property);
                EditorGUI.BeginChangeCheck();

                position = EditorGUI.Vector3Field(positionRect, positionLabel, position);

                if (EditorGUI.EndChangeCheck()) positionProperty.vector3Value = position;

                EditorGUI.EndProperty();

                EditorGUI.BeginProperty(rotationRect, label, property);
                EditorGUI.BeginChangeCheck();

                euler = EditorGUI.Vector3Field(rotationRect, rotationLabel, euler);

                if (EditorGUI.EndChangeCheck())
                {
                    rotationProperty.quaternionValue = Quaternion.Euler(euler);
                    _eulerAngles = euler;
                }

                EditorGUI.EndProperty();

                EditorGUI.indentLevel -= 1;
            }
        }

        private float RoundForInspector(float f)
        {
            return Mathf.Round(f * 10000) / 10000;
        }
    }

}
