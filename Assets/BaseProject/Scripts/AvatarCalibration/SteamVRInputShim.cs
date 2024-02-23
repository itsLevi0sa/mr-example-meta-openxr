using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KinemotikVR;

#if STEAMVR
using Valve.VR;
#endif

public class SteamVRInputShim : InputShimBase
{
#if STEAMVR

    private const string ACTION_PRIMARY_2D_AXIS = "axis_2d_1";
    private const string ACTION_SECONDARY_2D_AXIS = "axis_2d_2";
    private const string ACTION_PRIMARY_2D_AXIS_CLICK = "axis_2d_1_click";
    private const string ACTION_TRIGGER = "trigger";
    private const string ACTION_MENU_BUTTON = "menu";
    private const string ACTION_GRIP = "grip";

    private SteamVR_Input_Sources inputSource = default;

    public void Init(Handedness hand)
    {
        if (SteamVR.initializedState == SteamVR.InitializedStates.None)
            SteamVR.Initialize();

        if (hand == Handedness.Left)
            inputSource = SteamVR_Input_Sources.LeftHand;
        else
            inputSource = SteamVR_Input_Sources.RightHand;
    }

#region button_methods

    public override Vector2 GetTrackpadValue()
    {
        return SteamVR_Input.GetVector2(ACTION_PRIMARY_2D_AXIS, inputSource);
    }

    public override Vector2 GetSecondary2DAxisValue()
    {
        return SteamVR_Input.GetVector2(ACTION_SECONDARY_2D_AXIS, inputSource);
    }

    public override float GetTriggerValue()
    {
        return SteamVR_Input.GetFloat(ACTION_TRIGGER, inputSource);
    }

    public override float GetGripValue()
    {
        return SteamVR_Input.GetFloat(ACTION_GRIP, inputSource);
    }

    public override bool GetMenuButton()
    {
        return SteamVR_Input.GetState(ACTION_MENU_BUTTON, inputSource);
    }
    /*
        public virtual bool GetTriggerTouched()
        {
            return Input.GetButton(triggerTouchButton);
        }
    */
    public override bool GetTrackpadClicked()
    {
        return SteamVR_Input.GetState(ACTION_PRIMARY_2D_AXIS_CLICK, inputSource);
    }

    public override bool GetTrackpadTouched()
    {
        return false;
    }

#endregion

#endif
}
