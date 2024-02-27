using UnityEngine;

namespace KinemotikVR
{
    internal class SimpleHumanoidCalibrator : MonoBehaviour
    {
        private const float eyeHeightToPelvisHeightRatio = 3.5f / 7f;
        private const float distanceFromHeadToCenterOfBodyRatio = 0.04f;

        public Transform playerSpace;
        public Transform center;
        public AvatarDefinition goldie;

        [Header("Tracked Devices")]
        public Transform head;
        public Transform pelvis;
        public Transform leftFoot;
        public Transform rightFoot;

        [Header("Calibration Transforms")]
        public Transform headCalibration;
        public Transform pelvisCalibration;
        public Transform leftFootCalibration;
        public Transform rightFootCalibration;

        [Header("Offset Transforms")]
        public Transform headOffset;
        public Transform pelvisOffset;
        public Transform leftFootOffset;
        public Transform rightFootOffset;


        [ContextMenu("Calibrate!")]
        public void CalledCalibrate()
        {
            Calibrate(goldie);
        }
        public void Calibrate(AvatarDefinition avatar)
        {
            Debug.Log("Calibrate!");
            SetOffset(headOffset, avatar.headOffset);
            SetOffset(pelvisOffset, avatar.pelvisOffset);
            SetOffset(leftFootOffset, avatar.leftFootOffset);
            SetOffset(rightFootOffset, avatar.rightFootOffset);

            Transform root = avatar.transform;
            float avatarEyeHeight = avatar.viewPosition.y;
            float playerEyeHeight = head.localPosition.y;

            center.position = head.position - head.TransformVector(new Vector3(0, 0, distanceFromHeadToCenterOfBodyRatio * playerEyeHeight));
            center.rotation = Quaternion.LookRotation(Vector3.ProjectOnPlane(head.forward, playerSpace.up), playerSpace.up);

            // put center on the ground
            center.localPosition = new Vector3(center.localPosition.x, 0, center.localPosition.z);

            Vector3 leftFootPos = center.InverseTransformPoint(leftFoot.position);
            Vector3 rightFootPos = center.InverseTransformPoint(rightFoot.position);

            ApplyCalibration(playerEyeHeight, avatarEyeHeight, center, head, headCalibration, new Vector3(0, playerEyeHeight, 0), Quaternion.identity);
            ApplyCalibration(playerEyeHeight, avatarEyeHeight, center, pelvis, pelvisCalibration, new Vector3(0, playerEyeHeight * eyeHeightToPelvisHeightRatio, 0), Quaternion.identity);
            ApplyCalibration(playerEyeHeight, avatarEyeHeight, center, leftFoot, leftFootCalibration, new Vector3(leftFootPos.x, 0, 0), Quaternion.Euler(0, 15, 0));
            ApplyCalibration(playerEyeHeight, avatarEyeHeight, center, rightFoot, rightFootCalibration, new Vector3(rightFootPos.x, 0, 0), Quaternion.Euler(0, -15, 0));

            root.localScale = playerEyeHeight / avatarEyeHeight * Vector3.one;
        }

        private void SetOffset(Transform transform, Pose offset)
        {
            transform.localPosition = offset.position;
            transform.localRotation = offset.rotation;
        }

        private void ApplyCalibration(float playerEyeHeight, float avatarEyeHeight, Transform center, Transform device, Transform calibration, Vector3 target, Quaternion targetRotation)
        {
            calibration.localPosition = device.InverseTransformPoint(center.TransformPoint(target));
            calibration.localRotation = Quaternion.Inverse(device.rotation) * (Quaternion.Inverse(targetRotation) * center.rotation);
            calibration.localScale = playerEyeHeight / avatarEyeHeight * Vector3.one;
        }
    }
}