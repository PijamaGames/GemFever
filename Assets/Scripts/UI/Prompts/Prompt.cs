using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Prompt : MonoBehaviour
{
    public Button btn;
    private RectTransform btnRect;
    private Camera cam;
    RectTransform canvasRect;

    private void Start()
    {
        cam = Camera.main;
        canvasRect = PromptsManager.canvasRect;
    }

    public void SetButton(Button _btn)
    {
        btn = _btn;
        btnRect = btn.GetComponent<RectTransform>();
    }

    private void Update()
    {
        //TODO: Colocar prompt en target point
        if(btn != null)
        {
            Vector2 viewportPos = cam.WorldToViewportPoint(transform.position);
            


            //btn.transform.localPosition = new Vector3(viewportPos.x*1000f, viewportPos.y*200f, 0f);

            Vector2 WorldObject_ScreenPosition = new Vector2(
            ((viewportPos.x * canvasRect.sizeDelta.x) - (canvasRect.sizeDelta.x * 0.5f)),
            ((viewportPos.y * canvasRect.sizeDelta.y) - (canvasRect.sizeDelta.y * 0.5f)));

            //now you can set the position of the ui element
            btnRect.anchoredPosition = WorldObject_ScreenPosition;
            btnRect.localPosition = new Vector3(btnRect.localPosition.x, btnRect.localPosition.y, 0f);
            //Debug.Log(viewportPos);
        }
    }
}
