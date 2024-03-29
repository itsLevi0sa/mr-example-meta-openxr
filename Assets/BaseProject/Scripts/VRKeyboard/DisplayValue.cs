using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DisplayValue : MonoBehaviour
{
    public Text displayText;
    public Text textValue;

    public void UpdateDisplay()
    {
        if (displayText.text.Length < 4)
        {
            displayText.text += textValue.text;
        }    
    }

    public void DeleteCharacter()
    {
        // Check if the text has characters to delete
        if (displayText.text.Length > 0)
        {
            // Remove the last character from the current text
            displayText.text = displayText.text.Substring(0, displayText.text.Length - 1);       
        }
    }

    public void ClearText()
    {
        displayText.text = "";
    }
}
