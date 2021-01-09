using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GemPool : MonoBehaviour
{
    [SerializeField] int objectsInPool = 35;
    [SerializeField] GameObject prefab;
    Stack<GameObject> pool = new Stack<GameObject>();
    public int availableObjects = 35;
    public bool isEmpty;

    private void Awake()
    {
        NetworkGem networkGem;
        for(int i = 0; i < objectsInPool; i++)
        {
            GameObject obj = Instantiate(prefab, transform.position, Quaternion.identity);
            obj.name = obj.name + i.ToString();
            obj.transform.SetParent(this.gameObject.transform);
            pool.Push(obj);
            obj.SetActive(false);
            networkGem = obj.GetComponent<NetworkGem>();
            networkGem.Init();
        }
    }

    public GameObject GetObjectInPool()
    {
        if (isEmpty) return null;

        availableObjects--;

        GameObject gameObject = pool.Pop();
        gameObject.SetActive(true);

        isEmpty = pool.Count == 0;

        return gameObject;
    }

    public void ReturnObjectToPool(GameObject gameObject)
    {
        availableObjects++;

        pool.Push(gameObject);
        gameObject.SetActive(false);

        isEmpty = pool.Count == 0;
    }
}
