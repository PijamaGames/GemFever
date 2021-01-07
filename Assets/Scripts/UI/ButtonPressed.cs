using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class ButtonPressed : MonoBehaviour
{

    private Button button;
    private Image image;
    private Image image1;
    private Image image2;
    private Vector3 originalPos;
    private TextMeshProUGUI text;
    private TextMeshProUGUI text1;
    private TextMeshProUGUI text2;

    [SerializeField] private Sprite selectedImage;
    [SerializeField] private Sprite unSelectedImage;
    [SerializeField] private Button otherButton;
    [SerializeField] private Button otherButton2;
    [SerializeField] float moveTextOnPressed = -2.5f;

    void Start()
    {
        button = GetComponent<Button>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        text1 = otherButton.GetComponentInChildren<TextMeshProUGUI>();
        text2 = otherButton2.GetComponentInChildren<TextMeshProUGUI>();

        originalPos = text.rectTransform.localPosition;
        image = GetComponent<Image>();
        image1 = otherButton.GetComponent<Image>();
        image2 = otherButton2.GetComponent<Image>();

        EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
        var pointerClick = new EventTrigger.Entry();
        pointerClick.eventID = EventTriggerType.PointerClick;

        pointerClick.callback.AddListener((e) => ChangeButton());
        trigger.triggers.Add(pointerClick);

        if (text.name == "Body" || text.name=="ShopFace")
        {
            ChangeButton();
        }

    }

    private void ChangeButton()
    {
        image.sprite = selectedImage;
        image1.sprite = unSelectedImage;
        image2.sprite = unSelectedImage;

        Vector3 pos = originalPos;
        pos.y += moveTextOnPressed;
        text.rectTransform.localPosition = pos;

        pos.y -= moveTextOnPressed;
        text2.rectTransform.localPosition = pos;
        text1.rectTransform.localPosition = pos;
    }
}
