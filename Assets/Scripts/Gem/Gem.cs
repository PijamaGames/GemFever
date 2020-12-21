using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int value = 1;

    [SerializeField] float horizontalSpawnForce = 6f;

    [SerializeField] float verticalSpawnForce = 6f;

    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        SpawnnForce();
    }

    public void SpawnnForce()
    {
        Vector3 force = Vector3.zero;

        force.x = Random.Range(-horizontalSpawnForce, horizontalSpawnForce);
        force.y = Random.Range(-verticalSpawnForce, verticalSpawnForce);


        rb.AddForce(force, ForceMode.Impulse);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (collision.gameObject.GetComponent<Player>().TryAddGemToPouch(this))
                gameObject.SetActive(false);
        }
    }
}
