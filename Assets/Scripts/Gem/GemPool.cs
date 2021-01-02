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
        for(int i = 0; i < objectsInPool; i++)
        {
            GameObject gameObject = Instantiate(prefab, transform.position, Quaternion.identity);
            gameObject.transform.SetParent(this.gameObject.transform);
            pool.Push(gameObject);
            gameObject.SetActive(false);
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
