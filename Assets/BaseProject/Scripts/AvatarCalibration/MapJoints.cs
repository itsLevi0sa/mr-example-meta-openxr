using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MapJoints : MonoBehaviour
{
    [SerializeField] private bool isReadyToMap = false;
    public GameObject[] objectsArray;
    [Header("XRRig")]
    [SerializeField] private Transform head;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;
    [Header("TargetRig")]
    [SerializeField] private GameObject networkPlayer;
    [SerializeField] private Transform networkPlayerHead;

    [Header("LeftHand")]
    [SerializeField] private Transform[] leftHandJoints = new Transform[19];
    #region LeftHand Parameters
    private Transform handRoot_L;
    private Transform wrist_L;
    private Transform index1Rot_L;
    private Transform index2Rot_L;
    private Transform index3Rot_L;
    private Transform middle1Rot_L;
    private Transform middle2Rot_L;
    private Transform middle3Rot_L;
    private Transform ring1Rot_L;
    private Transform ring2Rot_L;
    private Transform ring3Rot_L;
    private Transform pinky0Rot_L;
    private Transform pinky1Rot_L;
    private Transform pinky2Rot_L;
    private Transform pinky3Rot_L;
    private Transform thumb0Rot_L;
    private Transform thumb1Rot_L;
    private Transform thumb2Rot_L;
    private Transform thumb3Rot_L;
    #endregion

    [Header("LeftHandTarget")]
    [SerializeField] private Transform[] leftHandTargetJoints = new Transform[19];
    #region LeftHandTarget Parameters
    private Transform handRootTarget_L;
    private Transform wristTarget_L;
    private Transform index1RotTarget_L;
    private Transform index2RotTarget_L;
    private Transform index3RotTarget_L;
    private Transform middle1RotTarget_L;
    private Transform middle2RotTarget_L;
    private Transform middle3RotTarget_L;
    private Transform ring1RotTarget_L;
    private Transform ring2RotTarget_L;
    private Transform ring3RotTarget_L;
    private Transform pinky0RotTarget_L;
    private Transform pinky1RotTarget_L;
    private Transform pinky2RotTarget_L;
    private Transform pinky3RotTarget_L;
    private Transform thumb0RotTarget_L;
    private Transform thumb1RotTarget_L;
    private Transform thumb2RotTarget_L;
    private Transform thumb3RotTarget_L;
    #endregion

    [Header("RightHand")]
    [SerializeField] private Transform[] rightHandJoints = new Transform[19];
    #region RightHand Parameters
    private Transform handRoot_R;
    private Transform wrist_R;
    private Transform index1Rot_R;
    private Transform index2Rot_R;
    private Transform index3Rot_R;
    private Transform middle1Rot_R;
    private Transform middle2Rot_R;
    private Transform middle3Rot_R;
    private Transform ring1Rot_R;
    private Transform ring2Rot_R;
    private Transform ring3Rot_R;
    private Transform pinky0Rot_R;
    private Transform pinky1Rot_R;
    private Transform pinky2Rot_R;
    private Transform pinky3Rot_R;
    private Transform thumb0Rot_R;
    private Transform thumb1Rot_R;
    private Transform thumb2Rot_R;
    private Transform thumb3Rot_R;
    #endregion

    [Header("RightHandTarget")]
    [SerializeField] private Transform[] rightHandTargetJoints = new Transform[19];
    #region RightHandTarget Parameters
    private Transform handRootTarget_R;
    private Transform wristTarget_R;
    private Transform index1RotTarget_R;
    private Transform index2RotTarget_R;
    private Transform index3RotTarget_R;
    private Transform middle1RotTarget_R;
    private Transform middle2RotTarget_R;
    private Transform middle3RotTarget_R;
    private Transform ring1RotTarget_R;
    private Transform ring2RotTarget_R;
    private Transform ring3RotTarget_R;
    private Transform pinky0RotTarget_R;
    private Transform pinky1RotTarget_R;
    private Transform pinky2RotTarget_R;
    private Transform pinky3RotTarget_R;
    private Transform thumb0RotTarget_R;
    private Transform thumb1RotTarget_R;
    private Transform thumb2RotTarget_R;
    private Transform thumb3RotTarget_R;
    #endregion

    private void Start()
    {
        PopulateJoints();
    }

    [ContextMenu("Populate Joints")]
    public void PopulateJoints()
    {
        #region Populate LeftHand
        handRoot_L = leftHand.transform;
        leftHandJoints[0] = handRoot_L;
        wrist_L = FindDeepChild(leftHand.transform, "L_Wrist");
        leftHandJoints[1] = wrist_L;
        index1Rot_L = FindDeepChild(leftHand.transform, "L_IndexProximal");
        leftHandJoints[2] = index1Rot_L;
        index2Rot_L = FindDeepChild(leftHand.transform, "L_IndexIntermediate");
        leftHandJoints[3] = index2Rot_L;
        index3Rot_L = FindDeepChild(leftHand.transform, "L_IndexDistal");
        leftHandJoints[4] = index3Rot_L;
        middle1Rot_L = FindDeepChild(leftHand.transform, "L_MiddleProximal");
        leftHandJoints[5] = middle1Rot_L;
        middle2Rot_L = FindDeepChild(leftHand.transform, "L_MiddleIntermediate");
        leftHandJoints[6] = middle2Rot_L;
        middle3Rot_L = FindDeepChild(leftHand.transform, "L_MiddleDistal");
        leftHandJoints[7] = middle3Rot_L;
        ring1Rot_L = FindDeepChild(leftHand.transform, "L_RingProximal");
        leftHandJoints[8] = ring1Rot_L;
        ring2Rot_L = FindDeepChild(leftHand.transform, "L_RingIntermediate");
        leftHandJoints[9] = ring2Rot_L;
        ring3Rot_L = FindDeepChild(leftHand.transform, "L_RingDistal");
        leftHandJoints[10] = ring3Rot_L;
        pinky1Rot_L = FindDeepChild(leftHand.transform, "L_LittleProximal");
        leftHandJoints[11] = pinky1Rot_L;
        pinky2Rot_L = FindDeepChild(leftHand.transform, "L_LittleIntermediate");
        leftHandJoints[12] = pinky2Rot_L;
        pinky3Rot_L = FindDeepChild(leftHand.transform, "L_LittleTip");
        leftHandJoints[13] = pinky3Rot_L;
        thumb0Rot_L = FindDeepChild(leftHand.transform, "L_ThumbMetacarpal");
        leftHandJoints[14] = thumb0Rot_L;
        thumb1Rot_L = FindDeepChild(leftHand.transform, "L_ThumbProximal");
        leftHandJoints[15] = thumb1Rot_L;
        thumb2Rot_L = FindDeepChild(leftHand.transform, "L_ThumbDistal");
        leftHandJoints[16] = thumb2Rot_L;
        thumb3Rot_L = FindDeepChild(leftHand.transform, "L_ThumbTip");
        leftHandJoints[17] = thumb3Rot_L;
        #endregion

        #region Populate RightHand
        handRoot_R = FindDeepChild(rightHand.transform, "OculusHand_R");
        rightHandJoints[0] = handRoot_R;
        wrist_R = FindDeepChild(rightHand.transform, "b_r_wrist");
        rightHandJoints[1] = wrist_R;
        index1Rot_R = FindDeepChild(rightHand.transform, "b_r_index1");
        rightHandJoints[2] = index1Rot_R;
        index2Rot_R = FindDeepChild(rightHand.transform, "b_r_index2");
        rightHandJoints[3] = index2Rot_R;
        index3Rot_R = FindDeepChild(rightHand.transform, "b_r_index3");
        rightHandJoints[4] = index3Rot_R;
        middle1Rot_R = FindDeepChild(rightHand.transform, "b_r_middle1");
        rightHandJoints[5] = middle1Rot_R;
        middle2Rot_R = FindDeepChild(rightHand.transform, "b_r_middle2");
        rightHandJoints[6] = middle2Rot_R;
        middle3Rot_R = FindDeepChild(rightHand.transform, "b_r_middle3");
        rightHandJoints[7] = middle3Rot_R;
        ring1Rot_R = FindDeepChild(rightHand.transform, "b_r_ring1");
        rightHandJoints[8] = ring1Rot_R;
        ring2Rot_R = FindDeepChild(rightHand.transform, "b_r_ring2");
        rightHandJoints[9] = ring2Rot_R;
        ring3Rot_R = FindDeepChild(rightHand.transform, "b_r_ring3");
        rightHandJoints[10] = ring3Rot_R;
        pinky1Rot_R = FindDeepChild(rightHand.transform, "b_r_pinky1");
        rightHandJoints[11] = pinky1Rot_R;
        pinky2Rot_R = FindDeepChild(rightHand.transform, "b_r_pinky2");
        rightHandJoints[12] = pinky2Rot_R;
        pinky3Rot_R = FindDeepChild(rightHand.transform, "b_r_pinky3");
        rightHandJoints[13] = pinky3Rot_R;
        thumb0Rot_R = FindDeepChild(rightHand.transform, "b_r_thumb0");
        rightHandJoints[14] = thumb0Rot_R;
        thumb1Rot_R = FindDeepChild(rightHand.transform, "b_r_thumb1");
        rightHandJoints[15] = thumb1Rot_R;
        thumb2Rot_R = FindDeepChild(rightHand.transform, "b_r_thumb2");
        rightHandJoints[16] = thumb2Rot_R;
        thumb3Rot_R = FindDeepChild(rightHand.transform, "b_r_thumb3");
        rightHandJoints[17] = thumb3Rot_R;
        #endregion

    }

    [ContextMenu("Populate Targets")]
    public void PopulateTargets()
    {
        /*
        objectsArray = FindObjectsWithTagInAllScenes("OffsetContainer");
        foreach (GameObject obj in objectsArray)
        {
            if (obj.GetComponent<NetworkPlayerServerAttributes>().IsMine == true)
            {
                networkPlayer = obj;
            }
        }
        */

        if (networkPlayer != null)
        {
            #region Populate LeftHand Target 
            handRootTarget_L = FindDeepChild(networkPlayer.transform, "LeftHand");
            leftHandTargetJoints[0] = handRootTarget_L;
            wristTarget_L = FindDeepChild(networkPlayer.transform, "LeftHand");
            leftHandTargetJoints[1] = wristTarget_L;
            index1RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandIndex1");
            leftHandTargetJoints[2] = index1RotTarget_L;
            index2RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandIndex2");
            leftHandTargetJoints[3] = index2RotTarget_L;
            index3RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandIndex3");
            leftHandTargetJoints[4] = index3RotTarget_L;
            middle1RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandMiddle1");
            leftHandTargetJoints[5] = middle1RotTarget_L;
            middle2RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandMiddle2");
            leftHandTargetJoints[6] = middle2RotTarget_L;
            middle3RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandMiddle3");
            leftHandTargetJoints[7] = middle3RotTarget_L;
            ring1RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandRing1");
            leftHandTargetJoints[8] = ring1RotTarget_L;
            ring2RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandRing2");
            leftHandTargetJoints[9] = ring2RotTarget_L;
            ring3RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandRing3");
            leftHandTargetJoints[10] = ring3RotTarget_L;
            pinky0RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandPinky1");
            leftHandTargetJoints[11] = pinky0RotTarget_L;
            pinky1RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandPinky2");
            leftHandTargetJoints[12] = pinky1RotTarget_L;
            pinky2RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandPinky3");
            leftHandTargetJoints[13] = pinky2RotTarget_L;
            pinky3RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandPinky4");
            leftHandTargetJoints[14] = pinky3RotTarget_L;
            thumb0RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandThumb1");
            leftHandTargetJoints[15] = thumb0RotTarget_L;
            thumb1RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandThumb2");
            leftHandTargetJoints[16] = thumb1RotTarget_L;
            thumb2RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandThumb3");
            leftHandTargetJoints[17] = thumb2RotTarget_L;
            thumb3RotTarget_L = FindDeepChild(networkPlayer.transform, "LeftHandThumb4");
            leftHandTargetJoints[18] = thumb3RotTarget_L;
            #endregion

            #region Populate RightHand Target 
            handRootTarget_R = FindDeepChild(networkPlayer.transform, "OculusHand_R");
            rightHandTargetJoints[0] = handRootTarget_R;
            wristTarget_R = FindDeepChild(networkPlayer.transform, "b_r_wrist");
            rightHandTargetJoints[1] = wristTarget_R;
            index1RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_index1");
            rightHandTargetJoints[2] = index1RotTarget_R;
            index2RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_index2");
            rightHandTargetJoints[3] = index2RotTarget_R;
            index3RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_index3");
            rightHandTargetJoints[4] = index3RotTarget_R;
            middle1RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_middle1");
            rightHandTargetJoints[5] = middle1RotTarget_R;
            middle2RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_middle2");
            rightHandTargetJoints[6] = middle2RotTarget_R;
            middle3RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_middle3");
            rightHandTargetJoints[7] = middle3RotTarget_R;
            ring1RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_ring1");
            rightHandTargetJoints[8] = ring1RotTarget_R;
            ring2RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_ring2");
            rightHandTargetJoints[9] = ring2RotTarget_R;
            ring3RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_ring3");
            rightHandTargetJoints[10] = ring3RotTarget_R;
            pinky0RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_pinky0");
            rightHandTargetJoints[11] = pinky0RotTarget_R;
            pinky1RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_pinky1");
            rightHandTargetJoints[12] = pinky1RotTarget_R;
            pinky2RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_pinky2");
            rightHandTargetJoints[13] = pinky2RotTarget_R;
            pinky3RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_pinky3");
            rightHandTargetJoints[14] = pinky3RotTarget_R;
            thumb0RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_thumb0");
            rightHandTargetJoints[15] = thumb0RotTarget_R;
            thumb1RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_thumb1");
            rightHandTargetJoints[16] = thumb1RotTarget_R;
            thumb2RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_thumb2");
            rightHandTargetJoints[17] = thumb2RotTarget_R;
            thumb3RotTarget_R = FindDeepChild(networkPlayer.transform, "b_r_thumb3");
            rightHandTargetJoints[18] = thumb3RotTarget_R;
            #endregion

            networkPlayerHead = FindDeepChild(networkPlayer.transform, "Head");
        }
        else
        {
            Debug.Log("No local networkPlayer found!");
        }
        ReadyToMap();

    }
    public GameObject[] FindObjectsWithTagInAllScenes(string tag)
    {
        List<GameObject> foundObjects = new List<GameObject>();
        List<GameObject> foundPlayers = new List<GameObject>();
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.isLoaded)
            {
                GameObject[] rootObjects = scene.GetRootGameObjects();
                foreach (GameObject rootObject in rootObjects)
                {
                    // Check if the current root object has the tag
                    if (rootObject.CompareTag(tag))
                    {
                        foundObjects.Add(rootObject);

                    }
                }
            }
            // Search through the children of the root object
            foreach (GameObject obj in foundObjects)
            {
                foundPlayers = FindChildrenWithTag(obj.transform, "Player");
            }
        }

        return foundPlayers.ToArray();
    }

    private List<GameObject> FindChildrenWithTag(Transform parent, string tag)
    {
        List<GameObject> taggedChildren = new List<GameObject>();

        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                taggedChildren.Add(child.gameObject);
            }

            // Recursively search in children
            taggedChildren.AddRange(FindChildrenWithTag(child, tag));
        }

        return taggedChildren;
    }

    Transform FindDeepChild(Transform parent, string name)
    {
        foreach (Transform child in parent)
        {
            if (child.name == name)
                return child;

            Transform found = FindDeepChild(child, name);
            if (found != null)
                return found;
        }
        return null;
    }


    [ContextMenu("ReadyToMap")]
    public void ReadyToMap()
    {
        isReadyToMap = true;
    }

    private void LateUpdate()
    {
        if (isReadyToMap == true)
        {
            #region Map LeftHand
            CopyTransform(handRoot_L, handRootTarget_L);
            CopyTransform(wrist_L, wristTarget_L);
            CopyRotation(index1Rot_L, index1RotTarget_L);
            CopyRotation(index2Rot_L, index2RotTarget_L);
            CopyRotation(index3Rot_L, index3RotTarget_L);
            CopyRotation(middle1Rot_L, middle1RotTarget_L);
            CopyRotation(middle2Rot_L, middle2RotTarget_L);
            CopyRotation(middle3Rot_L, middle3RotTarget_L);
            CopyRotation(ring1Rot_L, ring1RotTarget_L);
            CopyRotation(ring2Rot_L, ring2RotTarget_L);
            CopyRotation(ring3Rot_L, ring3RotTarget_L);
            CopyRotation(pinky0Rot_L, pinky0RotTarget_L);
            CopyRotation(pinky1Rot_L, pinky1RotTarget_L);
            CopyRotation(pinky2Rot_L, pinky2RotTarget_L);
            CopyRotation(pinky3Rot_L, pinky3RotTarget_L);
            CopyRotation(thumb0Rot_L, thumb0RotTarget_L);
            CopyRotation(thumb1Rot_L, thumb1RotTarget_L);
            CopyRotation(thumb2Rot_L, thumb2RotTarget_L);
            CopyRotation(thumb3Rot_L, thumb3RotTarget_L);
            #endregion

            #region Map RightHand
            CopyTransform(handRoot_R, handRootTarget_R);
            CopyTransform(wrist_R, wristTarget_R);
            CopyRotation(index1Rot_R, index1RotTarget_R);
            CopyRotation(index2Rot_R, index2RotTarget_R);
            CopyRotation(index3Rot_R, index3RotTarget_R);
            CopyRotation(middle1Rot_R, middle1RotTarget_R);
            CopyRotation(middle2Rot_R, middle2RotTarget_R);
            CopyRotation(middle3Rot_R, middle3RotTarget_R);
            CopyRotation(ring1Rot_R, ring1RotTarget_R);
            CopyRotation(ring2Rot_R, ring2RotTarget_R);
            CopyRotation(ring3Rot_R, ring3RotTarget_R);
            CopyRotation(pinky0Rot_R, pinky0RotTarget_R);
            CopyRotation(pinky1Rot_R, pinky1RotTarget_R);
            CopyRotation(pinky2Rot_R, pinky2RotTarget_R);
            CopyRotation(pinky3Rot_R, pinky3RotTarget_R);
            CopyRotation(thumb0Rot_R, thumb0RotTarget_R);
            CopyRotation(thumb1Rot_R, thumb1RotTarget_R);
            CopyRotation(thumb2Rot_R, thumb2RotTarget_R);
            CopyRotation(thumb3Rot_R, thumb3RotTarget_R);
            #endregion

            //Map Head
            CopyTransform(head, networkPlayerHead);
        }
    }

    void CopyTransform(Transform source, Transform destination)
    {
        destination.position = source.position;
        destination.rotation = source.rotation;
        destination.localScale = source.localScale;
    }

    void CopyRotation(Transform source, Transform destination)
    {
        destination.rotation = source.rotation;
    }
}