using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdjustCamAspect : MonoBehaviour
{
    Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("aspect: " + cam.aspect + " fov: " + cam.fieldOfView);
    }
}
