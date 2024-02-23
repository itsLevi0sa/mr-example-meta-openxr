using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;

namespace KinemotikVR
{
    public class TrackingRig : MonoBehaviour
    {
        private const InputDeviceCharacteristics HMD_CHARACS = InputDeviceCharacteristics.HeadMounted;
        private const InputDeviceCharacteristics CONTROLLER_CHARACS = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Controller;
        private const InputDeviceCharacteristics TRACKER_CHARACS = InputDeviceCharacteristics.TrackedDevice;

        private static TrackingRig _instance = null;

        [SerializeField] private XRTracker hipTracker = null;
        [SerializeField] private XRTracker leftFootTracker = null;
        [SerializeField] private XRTracker rightFootTracker = null;
        [SerializeField] private bool showDebugInfo = false;

        public System.Action OnTrackersChanged;
        public UnityEvent OnCalibrated = new UnityEvent();
        public UnityEvent OnDecalibrated = new UnityEvent();

        private readonly List<InputDevice> devices = new List<InputDevice>();
        private InputDevice hmdTracker = default;
        private bool calibrated = false;

        private readonly List<InputDevice> genericTrackers = new List<InputDevice>();
        public IReadOnlyList<InputDevice> GenericTrackers { get => genericTrackers.AsReadOnly(); }

#if DEBUG
        [System.NonSerialized] public string[] deviceNameList;
#endif

        public XRTracker HipTracker { get => hipTracker; }
        public XRTracker LeftFootTracker { get => leftFootTracker; }
        public XRTracker RightFootTracker { get => rightFootTracker; }

        public bool IsCalibrated { get => calibrated; }

        void Start()
        {
            RefreshDevices();

            InputDevices.deviceConnected += OnDeviceConnected;
            InputDevices.deviceDisconnected += OnDeviceDisconnected;
        }

        public void Init()
        {
            if (_instance == null)
            {
                _instance = this;
                Start();
            }
        }

        void OnDeviceConnected(InputDevice dev)
        {
            RefreshDevices();
            OnTrackersChanged?.Invoke();
        }

        void OnDeviceDisconnected(InputDevice dev)
        {
            RefreshDevices();
            OnTrackersChanged?.Invoke();
        }

        private void RefreshDevices()
        {
            genericTrackers.Clear();

            InputDevices.GetDevicesWithCharacteristics(InputDeviceCharacteristics.TrackedDevice, devices);

            for (int i=0; i < devices.Count; ++i)
            {
                var device = devices[i];

                if (Utils.BitfieldContainsAll((int)device.characteristics, (int)HMD_CHARACS))
                {
                    hmdTracker = device;
                    continue;
                }
                if (Utils.BitfieldContainsAll((int)device.characteristics, (int)CONTROLLER_CHARACS))
                    continue;

                // If this is a camera/lighthouse, skip:
                if (Utils.BitfieldContainsAll((int)device.characteristics, (int)InputDeviceCharacteristics.TrackingReference))
                    continue;
                
                if (!device.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 pos))
                    continue;

                if (!device.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion rot))
                    continue;

                if (device.name.ToLower().Contains("liv "))
                    continue;

                // If it's a generic tracker:
                if (Utils.BitfieldContainsAll((int)device.characteristics, (int)TRACKER_CHARACS))
                {
                    genericTrackers.Add(device);

                    // If this is a device that was already assigned and is just re-connecting, 
                    // re-assign it (which happens automatically in MatchesPrevious()):
                    if (leftFootTracker != null && leftFootTracker.MatchesPrevious(device))
                        continue;
                    else if (rightFootTracker != null && rightFootTracker.MatchesPrevious(device))
                        continue;
                    else if (hipTracker != null && hipTracker.MatchesPrevious(device))
                        continue;
                }
            }

#if DEBUG
            if (showDebugInfo)
            {
                deviceNameList = new string[devices.Count];

                int i = 0;
                foreach (var device in devices)
                {
                    List<InputFeatureUsage> features = new List<InputFeatureUsage>();
                    device.TryGetFeatureUsages(features);

                    System.Text.StringBuilder sb = new System.Text.StringBuilder();

                    sb.AppendLine("\nFeatures:");

                    foreach (var feature in features)
                    {
                        sb.AppendLine(feature.name + ": " + feature.type.Name.ToString());
                    }

                    sb.AppendLine("\nRole: " + device.role.ToString());

                    var charcs = device.characteristics.GetIndividualFlags();

                    sb.AppendLine("\nCharacteristics:");
                    foreach (var c in charcs)
                    {
                        sb.AppendLine(c.ToString());
                    }

                    deviceNameList[i] = device.name;

                    Debug.Log(device.manufacturer + " - " + device.name + "\n" + sb.ToString());
                    ++i;
                }
            }
#endif
        }

        public void ToggleCalibration()
        {
            if (!calibrated)
                Calibrate();
            else
            {
                calibrated = false;
                OnDecalibrated.Invoke();
            }
        }

        /// <summary>
        /// Assigns roles to any generic trackers present.
        /// </summary>
        /// <returns>Returns true if the minimum trackers were present to complete calibration.</returns>
        public bool Calibrate()
        {
            if (_instance == null)
            {
                _instance = this;
                Start();
            }

            hipTracker.UnsetDevice();
            leftFootTracker.UnsetDevice();
            rightFootTracker.UnsetDevice();

            List<InputDevice> tempDeviceList = new List<InputDevice>();
            tempDeviceList.AddRange(genericTrackers);

            if (hmdTracker == null)
            {
                Debug.LogError("Could not calibrate: No HMD tracker detected.");
                return false;
            }

            // Construct a plane from the HMD's up and forward vectors:
            if (!hmdTracker.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion hmdRot))
            {
                Debug.LogError("Could not calibrate: Unable to get HMD rotation.");
                return false;
            }

            hmdTracker.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 hmdPos);

            Vector3 forward = hmdRot * Vector3.forward;
            Vector3 up = hmdRot * Vector3.up;
            Plane bodyPlane = new Plane(hmdPos, hmdPos + up, hmdPos + forward);

            // If we have three trackers, find the top-most as that will be our hip tracker:
            if (tempDeviceList.Count >= 3)
            {
                float topY = float.MinValue;
                int topTrackerIndex = -1;

                for (int i=0; i < tempDeviceList.Count; ++i)
                {
                    if (!tempDeviceList[i].TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 pos))
                        continue;

                    if (pos.y > topY)
                    {
                        topY = pos.y;
                        topTrackerIndex = i;
                    }
                }

                if (topTrackerIndex < 0)
                {
                    Debug.LogError("Could not calibrate: Unable to get position of additional trackers.");
                    return false;
                }

                hipTracker.SetDevice(tempDeviceList[topTrackerIndex]);
                tempDeviceList.RemoveAt(topTrackerIndex);
            }
            
            // Find the bottom-most two trackers and these will be or feet:
            if (tempDeviceList.Count >= 2)
            {
                tempDeviceList.Sort(CompareTrackerYPositions);

                // The first two elements will be our bottom two trackers, 
                // so determine which is left and which is right:
                if (!tempDeviceList[0].TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 ultPos))
                {
                    Debug.LogError("Could not calibrate: Unable to get position of lowest tracker: " + tempDeviceList[0].name);
                    return false;
                }
                if (!tempDeviceList[1].TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 penultPos))
                {
                    Debug.LogError("Could not calibrate: Unable to get position of second lowest tracker: " + tempDeviceList[1].name);
                    return false;
                }

                // If the first element is on the left:
                if (bodyPlane.GetSide(ultPos))
                {
                    rightFootTracker.SetDevice(tempDeviceList[0]);
                    leftFootTracker.SetDevice(tempDeviceList[1]);
                }
                else // the first element is on the right:
                {
                    rightFootTracker.SetDevice(tempDeviceList[1]);
                    leftFootTracker.SetDevice(tempDeviceList[0]);
                }
            }
            else if (tempDeviceList.Count == 1) // Else assume we only have a hip tracker:
            {
                hipTracker.SetDevice(tempDeviceList[0]);
            }

            calibrated = true;
            OnCalibrated.Invoke();

            return true;
        }

        private int CompareTrackerYPositions(InputDevice a, InputDevice b)
        {
            if (!a.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 pos1))
                return -1;
            if (!b.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 pos2))
                return 1;

            if (pos1.y < pos2.y)
                return -1;
            else if (pos1.y > pos2.y)
                return 1;
            else
                return 0;
        }
    }
}