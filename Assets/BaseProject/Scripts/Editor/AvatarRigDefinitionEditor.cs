using UnityEditor;
using UnityEngine;

namespace KinemotikVR
{
    [CustomEditor(typeof(AvatarDefinition))]
    [ExecuteAlways]
    internal class AvatarRigDefinitionEditor : Editor
    {
        private new AvatarDefinition target => (AvatarDefinition)base.target;

        private static readonly Color kMiddleColor = Color.green;

        private GUIStyle _middleLabelStyle;

        internal void OnEnable()
        {
            _middleLabelStyle = CreateLabelStyle(kMiddleColor);
        }

        internal void OnSceneGUI()
        {
            Vector3 head = target.viewPosition;

            DrawPosition("View Position", _middleLabelStyle, ref head);

            target.viewPosition = head;
        }

        private GUIStyle CreateLabelStyle(Color color)
        {
            var style = new GUIStyle();

            style.normal.textColor = color;
            style.fontSize = 20;

            return style;
        }

        private void DrawPosition(string label, GUIStyle labelStyle, ref Vector3 position)
        {
            DrawLabel(position, label, labelStyle);

            switch (Tools.current)
            {
                case Tool.Move:
                    DrawPositionHandle(label, ref position, Quaternion.identity);
                    break;
                case Tool.Transform:
                    DrawPositionHandle(label, ref position, Quaternion.identity);
                    break;
                default:
                    DrawPoint(position, labelStyle.normal.textColor);
                    break;
            }
        }

        private void DrawPositionAndRotation(string label, GUIStyle labelStyle, ref Vector3 position, ref Quaternion rotation)
        {
            DrawLabel(position, label, labelStyle);

            switch (Tools.current)
            {
                case Tool.Move:
                    DrawPositionHandle(label, ref position, rotation);
                    break;
                case Tool.Rotate:
                    DrawRotationHandle(label, ref rotation, position);
                    break;
                case Tool.Transform:
                    DrawTransformHandle("View Position", ref position, ref rotation);
                    break;
                default:
                    DrawPoint(position, labelStyle.normal.textColor);
                    break;
            }
        }

        private bool DrawPositionHandle(string label, ref Vector3 position, Quaternion rotation)
        {
            Transform transform = target.transform;

            EditorGUI.BeginChangeCheck();
            Vector3 newPosition = transform.InverseTransformPoint(Handles.PositionHandle(transform.TransformPoint(position), transform.rotation * rotation));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, label + " Move");
                position = newPosition;
                return true;
            }

            return false;
        }

        private bool DrawRotationHandle(string label, ref Quaternion rotation, Vector3 position)
        {
            Transform transform = target.transform;

            EditorGUI.BeginChangeCheck();
            Quaternion newRotation = Quaternion.Inverse(transform.rotation) * Handles.RotationHandle(transform.rotation * rotation, transform.TransformPoint(position));
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, label + " Rotate");
                rotation = newRotation;
                return true;
            }

            return false;
        }

        private bool DrawTransformHandle(string label, ref Vector3 position, ref Quaternion rotation)
        {
            Transform transform = target.transform;

            Vector3 newPosition = transform.TransformPoint(position);
            Quaternion newRotation = transform.rotation * rotation;
            EditorGUI.BeginChangeCheck();
            Handles.TransformHandle(ref position, ref rotation);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, label + " Transform");
                position = transform.InverseTransformPoint(newPosition);
                rotation = Quaternion.Inverse(transform.rotation) * newRotation;
                return true;
            }

            return false;
        }

        private void DrawLabel(Vector3 position, string text, GUIStyle style)
        {
            Transform transform = target.transform;

            Handles.BeginGUI();
            Vector3 screenPosition = HandleUtility.WorldToGUIPointWithDepth(transform.TransformPoint(position));
            if (screenPosition.z >= 0)
            {
                GUI.Label(new Rect(screenPosition.x + 20, screenPosition.y - 10, 100, 20), text, style);
            }
            Handles.EndGUI();
        }

        private void DrawPoint(Vector3 position, Color color)
        {
            Transform transform = target.transform;

            Handles.BeginGUI();
            Vector3 screenPosition = HandleUtility.WorldToGUIPointWithDepth(transform.TransformPoint(position)) - new Vector3(1, 3);
            if (screenPosition.z >= 0)
            {
                EditorGUI.DrawRect(new Rect(screenPosition, new Vector2(3, 3)), color);
            }
            Handles.EndGUI();
        }
    }

}