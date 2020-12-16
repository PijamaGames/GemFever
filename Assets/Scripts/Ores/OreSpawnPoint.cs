using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class OreSpawnPoint : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Vector4(0f, 76f, 255f, 100f);
        Gizmos.DrawCube(gameObject.transform.position, gameObject.transform.localScale);
    }
}
