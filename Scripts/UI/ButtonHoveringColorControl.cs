using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonHoveringColorControl : MonoBehaviour
{
    public Color MainHoveringColor;
    public Color OutlineHoveringColor;

    public Text MainText;
    public Text OutlineText;

    private Color mainTextColor;
    private Color outlineTextColor;

    void Start()
    {
        mainTextColor = MainText.color;
        outlineTextColor = OutlineText.color;
    }

   public void OnPointerEnter()
   {
       MainText.color = MainHoveringColor;
       OutlineText.color = OutlineHoveringColor;
   }

   public void OnPointerExit()
   {
       MainText.color = mainTextColor;
       OutlineText.color = outlineTextColor;
   }
}
