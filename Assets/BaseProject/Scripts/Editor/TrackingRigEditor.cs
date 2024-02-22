using UnityEngine;
using KinemotikVR;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(TrackingRig))]
public class TrackingRigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var rig = (target as TrackingRig);

        if (Application.isPlaying)
        {
            if (GUILayout.Button(rig.IsCalibrated ? "Unlink" : "Calibrate"))
            {
                rig.ToggleCalibration();
            }
        }

        base.OnInspectorGUI();
    }
}
#endif