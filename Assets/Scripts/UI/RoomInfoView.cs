using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RoomInfoView : MonoBehaviour
{
    public TextMeshProUGUI playerName;
    public Bilingual numPlayers;
    private Button btn;
    string player;

    private void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(() =>
        {
            Debug.Log("PRESS JOIN: " + player);
            ClientSignedIn.JoinRoom(player);
        });
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


}
