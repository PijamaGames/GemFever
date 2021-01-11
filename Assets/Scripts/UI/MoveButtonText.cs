using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MoveButtonText : MonoBehaviour
{
    [SerializeField] float moveYOnPressed = -4.5f;

    private Vector3 []originalPos;
    private Button btn;
    private TextMeshProUGUI []text;

    private void Start()
    {
        btn = GetComponent<Button>();
        text = GetComponentsInChildren<TextMeshProUGUI>();
        originalPos = new Vector3[text.Length];
        for (int i=0;i<text.Length; i++)
        {
            originalPos[i] = text[i].rectTransform.localPosition;
        }

        EventTrigger trigger = btn.gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => Pressed());
        trigger.triggers.Add(pointerDown);

        var pointerUp = new EventTrigger.Entry();
        pointerUp.eventID = EventTriggerType.PointerUp;
        pointerUp.callback.AddListener((e) => Released());
        trigger.triggers.Add(pointerUp);
    }

    public void Pressed()
    {
        Vector3 []pos=new Vector3[originalPos.Length];
        
        for (int i = 0; i < text.Length; i++)
        {
            pos[i] = originalPos[i];
            pos[i].y+= moveYOnPressed;
            text[i].rectTransform.localPosition = pos[i];
        }
    }

    public void Released()
    {
        Vector3[] pos = new Vector3[originalPos.Length];

        for (int i = 0; i < text.Length; i++)
        {
            pos[i] = originalPos[i];
            text[i].rectTransform.localPosition = pos[i];
        }

    }
}
