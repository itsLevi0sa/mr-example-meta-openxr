using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class XRInputShim : InputShimBase
{
    private InputDevice device = default;
    private KinemotikVR.Handedness handedness = default;

    public void Init(InputDevice dev, KinemotikVR.Handedness hand)
    {
        device = dev;
        handedness = hand;
    }

    #region button_methods

    protected bool GetButton(InputFeatureUsage<bool> usage)
    {
        bool value;

        if (device.isValid)
        {
            if (device.TryGetFeatureValue(usage, out value))
                return value;
        }

        return false;
    }

    protected float GetAxis(InputFeatureUsage<float> usage)
    {
        float value;

        if (device.isValid)
        {
            if (device.TryGetFeatureValue(usage, out value))
                return value;
        }

        return 0;
    }

    protected Vector2 GetAxis(InputFeatureUsage<Vector2> usage)
    {
        Vector2 value;

        if (device.isValid)
        {
            if (device.TryGetFeatureValue(usage, out value))
                return value;
        }

        return Vector2.zero;
    }

    public override Vector2 GetTrackpadValue()
    {
        //if (controllerType == ControllerSpecies.WinMR)
        //    return GetAxis(CommonUsages.secondary2DAxis);
        //else
            return GetAxis(CommonUsages.primary2DAxis);
    }

    public override Vector2 GetSecondary2DAxisValue()
    {
        return GetAxis(CommonUsages.secondary2DAxis);
    }

    public override float GetTriggerValue()
    {
        return GetAxis(CommonUsages.trigger);
    }

    public override float GetGripValue()
    {
        return GetAxis(CommonUsages.grip);
    }

    public override bool GetMenuButton()
    {
#if (OCULUS || PICO || IQIYI) && !STEAMVR
#if IQIYI
        if (handedness == KinemotikVR.Handedness.Right)
            return false;
#endif
        return GetButton(CommonUsages.menuButton);
#else
        return GetButton(CommonUsages.primaryButton);
#endif
    }

    public override bool GetTrackpadClicked()
    {
        //if (controllerType == ControllerSpecies.WinMR)
        //    return GetButton(CommonUsages.secondary2DAxisClick);
        //else
            return GetButton(CommonUsages.primary2DAxisClick);
    }

    public override bool GetTrackpadTouched()
    {
        //if (controllerType == ControllerSpecies.WinMR)
        //    return GetButton(CommonUsages.secondary2DAxisTouch);
        //else
            return GetButton(CommonUsages.primary2DAxisTouch);
    }

    #endregion
}
