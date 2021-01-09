using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonOutline : MonoBehaviour
{
    private Outline outline;
    private Button[] buttons;
    private Color selected=new Color(0.3843138f, 0.2313726f, 0.1058824f, 1f);
    private Color unselected=new Color(1f,1f,1f,0f);
    private Color[] outlineColors;

    void Start()
    {
        buttons = GetComponentsInChildren<Button>();
        outlineColors=new Color[buttons.Length];
        int i = 0;
        foreach (var button in buttons)
        {
            EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
            var pointerClick = new EventTrigger.Entry();
            pointerClick.eventID = EventTriggerType.PointerClick;
            pointerClick.callback.AddListener((e) => ChangeOutline(button));
            trigger.triggers.Add(pointerClick);

            outline = button.GetComponent<Outline>();
            outlineColors[i] = outline.effectColor;
            outline.effectColor = unselected;

            i++;
        }
        
    }

    private void ChangeOutline(Button button)
    {
        outline = button.GetComponent<Outline>();
        outline.effectColor=new Color(0f,0f,0f,0f);
        outline.effectColor=outlineColors[0];
        int i = 0;
        foreach (var b in  buttons)
        {
            if (b != button)
            {
                outline = b.GetComponent<Outline>();
                outline.effectColor = unselected;
            }

            i++;

        }
    }

}
