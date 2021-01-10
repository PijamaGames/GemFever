using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    //Cuando salgas de la escena destruir a los jugadores y a sí mismo.

    public List<Player> players = new List<Player>();
    [SerializeField] int gemsEarnedInLocal = 10;


    // Start is called before the first frame update
    void Start()
    {
        FindPlayers();
        if (!GameManager.isLocalGame)
        {
            foreach (var p in players)
            {
                if (p.userInfo.id == Client.user.id)
                {
                    Client.user.gems += p.score;
                }
            }
            ClientInRoom.SaveGems();
        } else
        {
            Client.user.gems += gemsEarnedInLocal;
            ClientSignedIn.SaveInfo();
        }
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
