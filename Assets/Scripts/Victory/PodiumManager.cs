using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PodiumManager : MonoBehaviour
{
    public List<GameObject> podiumPositions = new List<GameObject>();
    public List<TextMeshProUGUI> playerNames = new List<TextMeshProUGUI>();
    public List<TextMeshProUGUI> playerScores = new List<TextMeshProUGUI>();
    [SerializeField] List<Image> panels=new List<Image>();

    VictoryManager victoryManager;

    private void Start()
    {
        victoryManager = FindObjectOfType<VictoryManager>();

        for (int i=0; i<playerNames.Count;i++)
        {
            playerNames[i].enabled = false;
            playerScores[i].enabled = false;
            panels[i].enabled = false;
        }

        PlaceInPodium();
    }

    void PlaceInPodium()
    {
        //Ordenar en el podio
        //Poner puntuación en UI
        //Ejecutar animación de victoria

        List<Player> orderedPlayers = victoryManager.GetOrderedPlayers();

        for(int i = 0; i < orderedPlayers.Count; i++)
        {
            orderedPlayers[i].transform.position = podiumPositions[i].transform.position;
            orderedPlayers[i].transform.rotation = Quaternion.LookRotation(-Vector3.forward, Vector3.up);

            orderedPlayers[i].PlayVictoryAnimation(i);

            orderedPlayers[i].ForceLookForward();

            panels[i].enabled = true;
            
            playerScores[i].enabled = true;

            playerScores[i].text = orderedPlayers[i].score.ToString();

            playerNames[i].enabled = true;

            if(!GameManager.isLocalGame)
            {
                playerNames[i].text = orderedPlayers[i].userInfo.id;
            }
            else
            {
                if (GameManager.english)
                    playerNames[i].text = "P" + orderedPlayers[i].playerNumber;
                else
                    playerNames[i].text = "P" + orderedPlayers[i].playerNumber;
            }
        }
    }

    public void DestroyPlayers()
    {
        victoryManager.DestroyAllPlayersAndItself();
    }

}