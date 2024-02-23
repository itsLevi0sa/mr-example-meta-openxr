using RootMotion.FinalIK;
using UnityEditor;
using UnityEngine;

namespace KinemotikVR
{
    [RequireComponent(typeof(VRIK))]
    public class HandDirectionHelper : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField] private VRIK ik;
        [SerializeField] private Animator animator;

        private void OnDrawGizmosSelected()
        {
            Animator animator = GetComponent<Animator>();

            if (!animator || !animator.isHuman)
            {
                return;
            }

            DrawHandGizmos(animator.GetBoneTransform(HumanBodyBones.LeftHand), ik.solver.leftArm.wristToPalmAxis, ik.solver.leftArm.palmToThumbAxis, true);
            DrawHandGizmos(animator.GetBoneTransform(HumanBodyBones.RightHand), ik.solver.rightArm.wristToPalmAxis, ik.solver.rightArm.palmToThumbAxis, false);
        }

        private void DrawHandGizmos(Transform hand, Vector3 wristToPalmAxis, Vector3 palmToThumbAxis, bool invert)
        {
            if (wristToPalmAxis.sqrMagnitude < float.Epsilon)
            {
                return;
            }

            float lineLength = 0.001f;
            DrawAxis(hand.position, hand.TransformPoint(wristToPalmAxis.normalized * lineLength), "Wrist to Palm Axis", Color.red);
            DrawAxis(hand.position, hand.TransformPoint(palmToThumbAxis.normalized * lineLength), "Palm to Thumb Axis", Color.green);
            DrawAxis(hand.position, hand.TransformPoint(Vector3.Cross(wristToPalmAxis, palmToThumbAxis).normalized * (invert ? -1 : 1) * lineLength), "Palm Normal", Color.blue);
        }

        private void DrawAxis(Vector3 from, Vector3 to, string label, Color color)
        {
            Handles.color = color;
            Handles.ArrowHandleCap(0, from, Quaternion.LookRotation(to - from), Vector3.Distance(from, to) * 0.88f, EventType.Repaint);

            Vector3 pos2D = HandleUtility.WorldToGUIPointWithDepth(to);
            if (pos2D.z >= 0)
            {
                Handles.BeginGUI();
                GUIStyle style = new GUIStyle(EditorStyles.label);
                style.normal.textColor = color;
                GUI.Label(new Rect(pos2D.x + 20, pos2D.y + 10, 120, 20), label, style);
                Handles.EndGUI();
            }
        }
#endif
    }
}
