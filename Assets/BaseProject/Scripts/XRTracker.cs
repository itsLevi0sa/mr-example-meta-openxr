using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace KinemotikVR
{
    public class XRTracker : MonoBehaviour
    {
        private Transform localTransform;
        private InputDevice device;
        private string prevDeviceSerial = "";

        void OnEnable()
        {
            localTransform = transform;
            InputDevices.deviceDisconnected += OnDeviceDisconnected;
        }

        void OnDisable()
        {
            InputDevices.deviceDisconnected -= OnDeviceDisconnected;
        }

        public bool IsValid { get => device.isValid; }

        public void SetDevice(InputDevice dev)
        {
            device = dev;

            if (dev.isValid)
            {
                prevDeviceSerial = dev.serialNumber;
                gameObject.SetActive(true);
                Update();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }

        public void UnsetDevice()
        {
            prevDeviceSerial = "";
            device = default(InputDevice);
            gameObject.SetActive(false);
        }

        public bool MatchesPrevious(InputDevice dev)
        {
            if (string.Compare(prevDeviceSerial, dev.serialNumber) == 0)
            {
                SetDevice(dev);
                return true;
            }

            return false;
        }

        void OnDeviceDisconnected(InputDevice dev)
        {
            if (!dev.isValid || string.Compare(dev.serialNumber, prevDeviceSerial) == 0)
                SetDevice(dev);
        }

        void Update()
        {
            if (string.IsNullOrEmpty(device.name))
                return;

            Vector3 pos;
            Quaternion rot;
            if (device.TryGetFeatureValue(CommonUsages.devicePosition, out pos))
            {
                localTransform.localPosition = pos;
            }
            else
            {
                Debug.LogError("devicePosition not supported by " + device.name);
            }

            if (device.TryGetFeatureValue(CommonUsages.deviceRotation, out rot))
            {
                localTransform.localRotation = rot;
            }
            else
            {
                Debug.LogError("deviceRotation not supported by " + device.name);
            }
        }
    }
}