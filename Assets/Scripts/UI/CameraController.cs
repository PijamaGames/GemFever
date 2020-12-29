using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class CameraController : MonoBehaviour
{
    [SerializeField] float defaultSize = 3.91f;
    [SerializeField] float aspect = 2f;
    private Camera cam;
    float defaultWidth;
    float lastAspect;

   
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.orthographicSize = defaultSize;
        defaultWidth = defaultSize * aspect;
        lastAspect = 0f;
    }

    private void Update()
    {
        if(cam.aspect != lastAspect)
        {
            lastAspect = cam.aspect;
            cam.orthographicSize = defaultWidth / cam.aspect;
        }
    }
}
