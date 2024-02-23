using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputShimBase
{
    #region button_methods

    public virtual Vector2 GetTrackpadValue()
    {
        return default;
    }

    public virtual Vector2 GetSecondary2DAxisValue()
    {
        return default;
    }

    public virtual float GetTriggerValue()
    {
        return default;
    }

    public virtual float GetGripValue()
    {
        return default;
    }

    public virtual bool GetMenuButton()
    {
        return default;
    }
    /*
        public virtual bool GetTriggerTouched()
        {
            return Input.GetButton(triggerTouchButton);
        }
    */
    public virtual bool GetTrackpadClicked()
    {
        return false;
    }

    public virtual bool GetTrackpadTouched()
    {
        return false;
    }

    #endregion
}
