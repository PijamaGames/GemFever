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

    [SerializeField] List<MeshRenderer> gemMeshes = new List<MeshRenderer>();
    [SerializeField] List<GemOreTiers> gemOreTiers = new List<GemOreTiers>();

    //Sound
    PersistentAudioSource audioSource;
    [SerializeField] AudioClip mineSound;

    // Start is called before the first frame update
    void Start()
    {
        gemPool = FindObjectOfType<GemPool>();
        audioSource = FindObjectOfType<PersistentAudioSource>();
        gemsLeft = availableGems;

        UpdateGemColorInOre();
    }

    private IEnumerator Regrow()
    {
        ShowGemMeshes(false);

        yield return new WaitForSecondsRealtime(timeToRegrow);
        currentGemValue += valueIncreasePerRegrowth;
        gemsLeft = availableGems;
        regrowing = false;

        UpdateGemColorInOre();
        ShowGemMeshes(true);
    }

    private void ShowGemMeshes(bool active)
    {
        foreach(MeshRenderer meshRenderer in gemMeshes)
        {
            meshRenderer.enabled = active;
        }
    }

    private void UpdateGemColorInOre()
    {
        bool currentTierFound = false;

        GemOreTiers nextTier;
        GemOreTiers currentTier = gemOreTiers[0];

        for (int i = 0; i < gemOreTiers.Count; i++)
        {
            if (!currentTierFound)
            {
                if (i == gemOreTiers.Count - 1)
                {
                    currentTierFound = true;
                }
                else
                {
                    nextTier = gemOreTiers[i + 1];
                    if (currentGemValue >= nextTier.minValue)
                    {
                        currentTier = nextTier;
                    }
                }
            }
        }

        foreach(MeshRenderer gemMesh in gemMeshes)
        {
            gemMesh.material = currentTier.tierMaterial;
        }

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

[System.Serializable]
public class GemOreTiers
{
    public Material tierMaterial;
    public int minValue;
}