using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class RoomInfoView : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public Bilingual numPlayers;
    private Button btn;
    string player;

    float moveYOnPressed = -4.5f;

    private Vector3 originalPosName;
    private Vector3 originalPosNumPlayers;
    private TextMeshProUGUI[] text;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            Debug.Log("PRESS JOIN: " + player);
            ClientSignedIn.JoinRoom(player);
        });

        originalPosName = new Vector3();
        originalPosName = playerName.rectTransform.localPosition;

        originalPosNumPlayers = new Vector3();
        originalPosNumPlayers = numPlayers.GetComponent<TextMeshProUGUI>().rectTransform.localPosition;

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

    public void UpdateVisuals(PlayerRoomInfo info)
    {
        if (info != null)
        {
            player = info.player;
            playerName.text = info.player;
            numPlayers.spanishText = "Jugadores: " + info.count + "/" + PlayerRoomInfo.maxPlayers;
            numPlayers.englishText = "Players: " + info.count + "/" + PlayerRoomInfo.maxPlayers;
            numPlayers.UpdateLanguage();
        }
        gameObject.SetActive(info != null);
    }

    public void Pressed()
    {
        Vector3 posName =originalPosName;
        Vector3 posNumPlayers =originalPosNumPlayers;

        posName.y += moveYOnPressed;
        posNumPlayers.y += moveYOnPressed;

        playerName.rectTransform.localPosition = posName;
        numPlayers.GetComponent<TextMeshProUGUI>().rectTransform.localPosition = posNumPlayers;

    }

    public void Released()
    {
        playerName.rectTransform.localPosition = originalPosName;
        numPlayers.GetComponent<TextMeshProUGUI>().rectTransform.localPosition = originalPosNumPlayers;
        
    }

}
