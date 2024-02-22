using UnityEngine;
using KinemotikVR;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(TrackingRigImport))]
public class TrackingRigEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var rig = (target as TrackingRigImport);

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