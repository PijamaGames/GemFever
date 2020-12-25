using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MoveButtonText : MonoBehaviour
{
    [SerializeField] float moveYOnPressed = -4.5f;

    private Vector3 originalPos;
    private Button btn;
    private TextMeshProUGUI text;

    private void Start()
    {
        btn = GetComponent<Button>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        originalPos = text.rectTransform.localPosition;

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
        Vector3 pos = originalPos;
        pos.y += moveYOnPressed;
        text.rectTransform.localPosition = pos;
    }

    public void Released()
    {
        Vector3 pos = originalPos;
        pos.y -= moveYOnPressed;
        text.rectTransform.localPosition = pos;
    }
}
