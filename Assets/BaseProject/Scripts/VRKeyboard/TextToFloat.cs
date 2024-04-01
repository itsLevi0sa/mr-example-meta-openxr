using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextToFloat : MonoBehaviour
{
    public float displayValue;
    public Text displayText;
    public UpdateValue updateValue;

    // Update is called once per frame
    void Update()
    {
       
        if (float.TryParse(displayText.text, out float floatValue))
        {
            displayValue = floatValue;
        }
        if (displayText.text == "")
        {
            displayValue = 0;
        }
        updateValue.AxisValue = displayValue;
        updateValue.axisValueToText.text = displayText.text;
    }
}
