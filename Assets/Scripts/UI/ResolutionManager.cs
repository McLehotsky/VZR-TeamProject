using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class ResolutionManager : MonoBehaviour
{
    public List<TMP_Text> allTexts;

    public Color pickedColor = Color.yellow;
    public Color normalColor = Color.white;

    void Start()
    {
        if (allTexts.Count >= 2) 
        {
            ChangeColor(allTexts[1]);
        }
    }

    public void ClickedButton(TMP_Text textToChange)
    {
        ChangeColor(textToChange);
    }

    void ChangeColor(TMP_Text activeText)
    {
        foreach (TMP_Text text in allTexts)
        {
            if (text == activeText)
            {
                text.color = pickedColor;
            }
            else
            {
                text.color = normalColor;
            }
        }
    }
}