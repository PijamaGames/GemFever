using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ore : MonoBehaviour
{
    [SerializeField] int availableGems = 6;
    [SerializeField] int gemsPerPickaxeHit = 2;

    [SerializeField] float timeToRegrow = 3f;
    [SerializeField] int valueIncreasePerRegrowth = 1;

    private int gemsLeft;
    private int currentGemValue = 1;
    private bool regrowing = false;

    public GameObject gemPrefab;

    // Start is called before the first frame update
    void Start()
    {
        gemsLeft = availableGems;
    }

    private IEnumerator Regrow()
    {
        Debug.Log("Creciendo");

        yield return new WaitForSecondsRealtime(timeToRegrow);
        currentGemValue += valueIncreasePerRegrowth;
        gemsLeft = availableGems;
        regrowing = false;

        Debug.Log("Crecido");
    }

    private void MineGem()
    {
        int extractedGems = 0;
        if(gemsLeft - gemsPerPickaxeHit < 0)
        {
            for(int i = 0; gemsLeft > 0; i++)
            {
                extractedGems++;
                gemsLeft--;
            }
        }
        else
        {
            gemsLeft -= gemsPerPickaxeHit;
            extractedGems = gemsPerPickaxeHit;
        }

        SpawnGems(extractedGems);

        //Debug.Log("Suelto " + extractedGems + " gemas");

        if (!regrowing && gemsLeft <= 0)
        {
            regrowing = true;
            StartCoroutine(Regrow());
        }
            
    }

    private void SpawnGems(int gemsToSpawn)
    {
        for(int i = 0; i < gemsToSpawn; i++)
        {
            Instantiate(gemPrefab, this.transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!regrowing && other.tag == "Pickaxe")
        {
            Debug.Log("Picado");
            MineGem();
        }
    }
}
