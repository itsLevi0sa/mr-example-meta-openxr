using RootMotion.FinalIK;
using UnityEngine;

public class UpperArmRelaxer : MonoBehaviour
{
	public IK ik;

	[Tooltip("This should be the forearm bone.")]
	public Transform child;

	[Tooltip("The weight of relaxing the twist.")]
	[Range(0f, 1f)] public float weight = 1f;

	[Tooltip("If 0.5, this Transform will be twisted half way from parent to child. If 1, the twist angle will be locked to the child and will rotate with along with it.")]
	[Range(0f, 1f)] public float parentChildCrossfade = 0.15f;

	[Tooltip("Rotation offset around the twist axis.")]
	[Range(-180f, 180f)] public float twistAngleOffset;

	/// <summary>
	/// Rotate this Transform to relax it's twist angle relative to its default (initial) and current rotations.
	/// </summary>
	public void Relax()
	{
		if (weight <= 0f) return; // Nothing to do here

		Quaternion rotation = transform.rotation;
		Quaternion twistOffset = Quaternion.AngleAxis(twistAngleOffset, rotation * twistAxis);
		rotation = twistOffset * rotation;

		// Find the world space relaxed axes of the default and current rotations
		Vector3 relaxedAxisDefault = parent.rotation * parentDefaultLocalRotation * axisRelativeToSelfDefault;
		Vector3 relaxedAxisCurrent = rotation * axisRelativeToSelfDefault;

		// Cross-fade between the default and current
		Vector3 relaxedAxis = Vector3.Slerp(relaxedAxisDefault, relaxedAxisCurrent, parentChildCrossfade);

		// Convert relaxedAxis to (axis, twistAxis) space so we can calculate the twist angle
		Quaternion r = Quaternion.LookRotation(rotation * axis, rotation * twistAxis);
		relaxedAxis = Quaternion.Inverse(r) * relaxedAxis;

		// Calculate the angle by which we need to rotate this Transform around the twist axis.
		float angle = Mathf.Atan2(relaxedAxis.x, relaxedAxis.z) * Mathf.Rad2Deg;

		// Store the rotation of the child so it would not change with twisting this Transform
		Quaternion childRotation = child.rotation;

		// Twist the bone
		transform.rotation = Quaternion.AngleAxis(angle * weight, rotation * twistAxis) * rotation;

		// Revert the rotation of the child
		child.rotation = childRotation;
	}

	private Vector3 twistAxis = Vector3.right;
	private Vector3 axis = Vector3.forward;
	private Vector3 axisRelativeToSelfDefault;

	private Transform parent;

	private Quaternion parentDefaultLocalRotation;
	private Quaternion childDefaultLocalRotation;

	private void Start()
	{
		twistAxis = transform.InverseTransformDirection(child.position - transform.position);
		axis = new Vector3(twistAxis.y, twistAxis.z, twistAxis.x);

		// Axis in world space
		Vector3 axisWorld = transform.rotation * axis;

		// Store the axis in worldspace relative to the rotations of the parent and child
		axisRelativeToSelfDefault = Quaternion.Inverse(transform.rotation) * axisWorld;

		parent = transform.parent;
		parentDefaultLocalRotation = transform.localRotation;
		childDefaultLocalRotation = child.localRotation;

		if (ik != null)
		{
			IKSolver solver = ik.GetIKSolver();
			solver.OnPreUpdate += OnPreUpdate;
			solver.OnPostUpdate += OnPostUpdate;
		}
	}

    private void Update()
    {
        if (ik == null)
        {
			FixTransforms();
        }
    }

    private void LateUpdate()
	{
		if (ik == null)
		{
			Relax();
		}
	}

	private void OnDestroy()
	{
		if (ik != null)
		{
			IKSolver solver = ik.GetIKSolver();
			solver.OnPreUpdate -= OnPreUpdate;
			solver.OnPostUpdate -= OnPostUpdate;
		}
	}

	private void OnPreUpdate()
	{
		FixTransforms();
	}

	private void OnPostUpdate()
	{
		if (ik != null)
		{
			Relax();
		}
	}

	private void FixTransforms()
    {
		child.localRotation = childDefaultLocalRotation;
	}
}