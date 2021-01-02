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

    VictoryManager victoryManager;

    private void Start()
    {
        victoryManager = FindObjectOfType<VictoryManager>();
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

            //playerScores[i].text = orderedPlayers[i].score.ToString();

            //TODO Nombre de los usuarios

        }
    }

    public void DestroyPlayers()
    {
        victoryManager.DestroyAllPlayersAndItself();
    }

}