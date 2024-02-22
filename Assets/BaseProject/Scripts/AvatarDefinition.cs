using RootMotion.FinalIK;
using UnityEngine;

namespace KinemotikVR
{
    public class AvatarDefinition : MonoBehaviour
    {
        private const float eyeHeightToPelvisHeightRatio = 3.5f / 7f;

        [SerializeField] private VRIK _vrik;

        public Vector3 viewPosition;

        public Pose headOffset { get; private set; }

        public Pose pelvisOffset { get; private set; }

        public Pose leftFootOffset { get; private set; }

        public Pose rightFootOffset { get; private set; }

        private void Awake()
        {
            float eyeHeight = viewPosition.y;
            Vector3 centerLocalPosition = transform.InverseTransformPoint(_vrik.references.root.position);

            headOffset = GetOffset(_vrik.references.head, new Pose(new Vector3(centerLocalPosition.x, eyeHeight, centerLocalPosition.z), Quaternion.identity));
            pelvisOffset = GetOffset(_vrik.references.pelvis, new Pose(new Vector3(centerLocalPosition.x, eyeHeight * eyeHeightToPelvisHeightRatio, centerLocalPosition.z), Quaternion.identity));
            leftFootOffset = GetOffset(GetFootReference(_vrik.references.leftFoot, _vrik.references.leftToes), GetFootTarget(centerLocalPosition, _vrik.references.leftFoot, _vrik.references.leftToes));
            rightFootOffset = GetOffset(GetFootReference(_vrik.references.rightFoot, _vrik.references.rightToes), GetFootTarget(centerLocalPosition, _vrik.references.rightFoot, _vrik.references.rightToes));
        }

        private Pose GetOffset(Transform reference, Pose targetLocalPose)
        {
            return new Pose(
                transform.InverseTransformPoint(reference.position - transform.TransformPoint(targetLocalPose.position)),
                Quaternion.Inverse(transform.rotation * targetLocalPose.rotation) * reference.rotation);
        }

        private Transform GetFootReference(Transform foot, Transform toes)
        {
            return toes ? toes : foot;
        }

        private Pose GetFootTarget(Vector3 centerLocalPosition, Transform foot, Transform toes)
        {
            if (toes)
            {
                // find the point on the vector between the foot and the toes that crosses centerLocalPosition.z
                Vector3 localFootPosition = transform.InverseTransformPoint(foot.position);
                Vector3 localToesPosition = transform.InverseTransformPoint(toes.position);

                float a = localFootPosition.x;
                float b = localToesPosition.x;

                // centerLocalPosition.z = localFootPosition.z + (localToesPosition.z - localFootPosition.z) * t
                float t = (centerLocalPosition.z - localFootPosition.z) / (localToesPosition.z - localFootPosition.z);

                return new Pose(
                    new Vector3(Mathf.Lerp(a, b, t), 0, centerLocalPosition.z),
                    Quaternion.Inverse(transform.rotation) * Quaternion.LookRotation(Vector3.ProjectOnPlane(toes.position - foot.position, transform.up), transform.up));
            }
            else
            {
                return new Pose(
                    new Vector3(centerLocalPosition.x + transform.InverseTransformPoint(foot.position).x, 0, centerLocalPosition.z),
                    Quaternion.identity);
            }
        }
    }
}