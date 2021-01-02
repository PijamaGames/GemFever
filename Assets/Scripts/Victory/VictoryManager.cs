using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    //Cuando salgas de la escena destruir a los jugadores y a sí mismo.

    public List<Player> players = new List<Player>();


    // Start is called before the first frame update
    void Start()
    {
        FindPlayers();
    }

    void FindPlayers()
    {
        if (players.Count == 0)
        {
            Player[] aux = FindObjectsOfType<Player>();
            foreach (Player player in aux)
                players.Add(player);
        }
    }

    public List<Player> GetOrderedPlayers()
    {
        players.Sort((x, y) => y.score.CompareTo(x.score));

        return players;
    }
    
    public void DestroyAllPlayersAndItself()
    {
        foreach (Player player in players)
            Destroy(player.gameObject);

        Destroy(this.gameObject);
    }
}
