using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
{
    [SerializeField] float requestRoomsInterval = 4f;

    [SerializeField] Button createRoomBtn;
    [SerializeField] Button joinRandomBtn;

    [SerializeField] GameObject roomInfoPrefab;
    [SerializeField] Transform roomInfoContainer;
    [SerializeField] int maxRoomsDisplay = 20;
    RoomInfoView[] roomInfoViews;

    private void Start()
    {
        if (Client.state != Client.signedInState) return;
        roomInfoViews = new RoomInfoView[maxRoomsDisplay];
        for(int i = 0; i < maxRoomsDisplay; i++)
        {
            roomInfoViews[i] = Instantiate(roomInfoPrefab, roomInfoContainer).GetComponent<RoomInfoView>();
            roomInfoViews[i].UpdateVisuals(null);
        }

        createRoomBtn.onClick.AddListener(() =>
        {
            ClientSignedIn.CreateRoom();
        });
        joinRandomBtn.onClick.AddListener(() =>
        {
            ClientSignedIn.JoinRoom("");
        });

        ClientSignedIn.getRoomsEvent += UpdateUI;
        StartCoroutine(UpdateRoomInfosCoroutine());
    }

    private void OnDestroy()
    {
        ClientSignedIn.getRoomsEvent -= UpdateUI;
    }

    private void UpdateUI(List<PlayerRoomInfo> infos)
    {
        int count = infos.Count;
        Debug.Log("getting info " + count);
        PlayerRoomInfo info;
        for(int i = 0; i < maxRoomsDisplay; i++)
        {
            info = i < count ? infos[i] : null;
            roomInfoViews[i].UpdateVisuals(info);
        }
    }

    IEnumerator UpdateRoomInfosCoroutine()
    {
        while (true)
        {
            ClientSignedIn.RequestRoomInfos();
            yield return new WaitForSeconds(requestRoomsInterval);

        }
    }
}
