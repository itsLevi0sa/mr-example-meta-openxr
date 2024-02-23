using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.Events;

namespace KinemotikVR
{
    public enum Handedness
    {
        Unknown = -1,
        Left,
        Right
    }

    public class XRInputController : MonoBehaviour
    {
        public const float TRIGGER_ENGAGE_THRESHOLD = 0.9f;

        static List<InputDevice> devices = new List<InputDevice>();

        public Handedness handedness = default;
        public UnityEvent OnMenuButtonPress = new UnityEvent();
        public UnityEvent OnTriggerPulled = new UnityEvent();

        protected InputDeviceCharacteristics characteristics = 0;
        protected InputDevice device;
        protected bool menuButtonPressed = false;
        protected bool triggerPulled = false;
        protected InputShimBase inputShim = default;

        public InputDevice Device { get => device; }

        protected void Awake()
        {
            if (handedness == Handedness.Left)
            {
                characteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left;
            }
            else if (handedness == Handedness.Right)
            {
                characteristics = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right;
            }
        }

        protected void OnEnable()
        {
            InputDevices.deviceConnected += OnDeviceConnected;
            InputDevices.deviceDisconnected += OnDeviceDisconnected;

            InputDevices.GetDevicesWithCharacteristics(characteristics, devices);
            if (devices.Count > 0)
                OnDeviceConnected(devices[0]);

            devices.Clear();
            InputDevices.GetDevices(devices);

            int count = 0;
            foreach (var dev in devices)
            {
                Debug.Log("Device " + count + ": " + dev.name + ":\n" + ((int)dev.characteristics).ToString());
                ++count;
            }
        }

        protected void OnDisable()
        {
            InputDevices.deviceConnected -= OnDeviceConnected;
            InputDevices.deviceDisconnected -= OnDeviceDisconnected;
        }

        void OnDeviceConnected(InputDevice dev)
        {
            if (!device.isValid && Utils.BitfieldContainsAll((int)dev.characteristics, (int)characteristics))
            {
                device = dev;

#if STEAMVR
                inputShim = new SteamVRInputShim();
                (inputShim as SteamVRInputShim).Init(handedness);
#else
                inputShim = new XRInputShim();
                (inputShim as XRInputShim).Init(dev, handedness);
#endif
            }
        }

        void OnDeviceDisconnected(InputDevice dev)
        {
            if (dev == device)
            {
                dev = new InputDevice();

                if (inputShim != null && inputShim is XRInputShim)
                {
                    (inputShim as XRInputShim).Init(device, handedness);
                }
            }
        }

        void Update()
        {
            if (inputShim == null)
                return;

            var buttonVal = inputShim.GetMenuButton();

            if (buttonVal != menuButtonPressed)
            {
                menuButtonPressed = buttonVal;

                if (menuButtonPressed)
                    OnMenuButtonPress.Invoke();
            }

            buttonVal = inputShim.GetTriggerValue() > TRIGGER_ENGAGE_THRESHOLD;

            if (buttonVal != triggerPulled)
            {
                triggerPulled = buttonVal;

                if (triggerPulled)
                    OnTriggerPulled.Invoke();
            }
        }

    }
}