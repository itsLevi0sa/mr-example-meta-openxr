using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class UpdateValue : MonoBehaviour
{
    public GameObject target;
    public Text axisValueToText;
    public Text targetText;

    private float axisValue;

    public float AxisValue
    {
        get { return axisValue; }
        set
        {
            // Check if the value has changed
            if (value != axisValue)
            {
                axisValue = value;
                OnFloatValueChanged();
            }
        }
    }

    public enum Axis
    {
        X,
        Y,
        Z
    }

    public Axis axis;

    private void Start()
    {
        if (axis == Axis.X)
        {
            axisValue = target.transform.position.x;
            axisValueToText.text = axisValue.ToString();
        }
        if (axis == Axis.Y)
        {
            axisValue = target.transform.position.y;
            axisValueToText.text = axisValue.ToString();
        }
        if (axis == Axis.Z)
        {
            axisValue = target.transform.position.z;
            axisValueToText.text = axisValue.ToString();
        }
        targetText.text = axisValueToText.text;
    }
    private void OnFloatValueChanged()
    {
        if (axis == Axis.X)
        {
            target.transform.position = new Vector3(axisValue, target.transform.position.y, target.transform.position.z);
        }
        if (axis == Axis.Y)
        {
            target.transform.position = new Vector3(target.transform.position.x, axisValue, target.transform.position.z);
        }
        if (axis == Axis.Z)
        {
            target.transform.position = new Vector3(target.transform.position.x, target.transform.position.y, axisValue);
        }
    }

    private void Update()
    {
        if (float.TryParse(axisValueToText.text, out float floatValue))
        {
            axisValue = floatValue;
        }
    }
}
