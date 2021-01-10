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
    public int currentGemValue = 1;
    private bool regrowing = false;

    public GameObject gemPrefab;
    GemPool gemPool;

    //Sound
    PersistentAudioSource audioSource;
    [SerializeField] AudioClip mineSound;

    // Start is called before the first frame update
    void Start()
    {
        gemPool = FindObjectOfType<GemPool>();
        audioSource = FindObjectOfType<PersistentAudioSource>();
        gemsLeft = availableGems;
    }

    private IEnumerator Regrow()
    {
        //Debug.Log("Creciendo");

        yield return new WaitForSecondsRealtime(timeToRegrow);
        currentGemValue += valueIncreasePerRegrowth;
        gemsLeft = availableGems;
        regrowing = false;

        //Debug.Log("Crecido");
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
            GameObject gem = gemPool.GetObjectInPool();

            if(gem != null)
            {
                gem.transform.position = this.transform.position;
                gem.transform.rotation = Quaternion.identity;
                gem.GetComponent<Gem>().UpdateGemValue(currentGemValue);
            }
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if(clip != null)
            audioSource.PlayEffect(clip);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!regrowing && other.tag == "Pickaxe")
        {
            if(!gemPool.isEmpty)
            {
                PlaySound(mineSound);
                MineGem();
            }
        }
    }
}
