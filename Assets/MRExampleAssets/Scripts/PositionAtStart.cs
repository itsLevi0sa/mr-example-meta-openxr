using System.Collections;
using UnityEngine;
using UnityEngine.XR.Content.UI.Layout;
using UnityEngine.XR.Interaction.Toolkit.Utilities;
using static UnityEditor.FilePathAttribute;

public class PositionAtStart : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The starting target.")]
    Transform m_Target;
    Vector3 initialPos;
    public float lerpDuration = 1f; // Duration of interpolation
    public LazyFollower lazyFollower;

    [SerializeField]
    [Tooltip("Adjusts the follow point from the target by this amount.")]
    Vector3 m_TargetOffset = Vector3.forward;

    bool m_IgnoreX = true;
    bool startFollow = false;
    Vector3 m_TargetPosition;

    private const float k_StartDelay = 2.0f;

    private void Start()
    {
        initialPos = this.gameObject.transform.position;
    }

    [ContextMenu("StartFollow")]
    public void StartFollow()
    {
        StartCoroutine(DelayFollow());
    }

    IEnumerator DelayFollow()
    {
        float elapsedTime = 0f;

        var targetRotation = m_Target.rotation;
        var newTransform = m_Target;
        var targetEuler = targetRotation.eulerAngles;
        targetRotation = Quaternion.Euler
        (
            m_IgnoreX ? 0f : targetEuler.x,
            targetEuler.y,
            targetEuler.z
        );
        var forward = (transform.position - m_Target.position).normalized;
        BurstMathUtility.LookRotationWithForwardProjectedOnPlane(forward, Vector3.up, out targetRotation);

        newTransform.rotation = targetRotation;
        m_TargetPosition = m_Target.position + newTransform.TransformVector(m_TargetOffset);

        this.GetComponent<Animator>().SetTrigger("SpawnObject");
        this.GetComponent<AudioSource>().Play();

        while (elapsedTime < lerpDuration)
        {
            // Increment elapsedTime over time
            elapsedTime += Time.deltaTime;

            // Calculate interpolation factor
            float t = Mathf.Clamp01(elapsedTime / lerpDuration);

            // Perform linear interpolation between start and end positions
            transform.position = Vector3.Lerp(initialPos, m_TargetPosition, t);

            yield return null; // Wait for the next frame
        }

        // Ensure the final position is exactly at the end position
        transform.position = m_TargetPosition;
        transform.rotation = targetRotation;
        //yield return new WaitForSeconds(lerpDuration);
        //startFollow = true;
        lazyFollower.enabled = true;
    }

    public void Update()
    {
        if (startFollow)
        {
            var targetRotation = m_Target.rotation;
            var newTransform = m_Target;
            var targetEuler = targetRotation.eulerAngles;
            targetRotation = Quaternion.Euler
            (
                m_IgnoreX ? 0f : targetEuler.x,
                targetEuler.y,
                targetEuler.z
            );

            newTransform.rotation = targetRotation;
            m_TargetPosition = m_Target.position + newTransform.TransformVector(m_TargetOffset);
            transform.position = m_TargetPosition;
        }
        
    }
}
