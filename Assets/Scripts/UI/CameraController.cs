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
    float lastDefaultSize;
   
    void Start()
    {
        cam = GetComponent<Camera>();
        lastAspect = 0f;
        cam.orthographicSize = defaultSize;
        lastDefaultSize = defaultSize;
        defaultWidth = defaultSize * aspect;
    }

    private void Update()
    {
        if (cam.aspect != lastAspect || defaultSize != lastDefaultSize)
        {
            defaultWidth = defaultSize * aspect;
            lastAspect = cam.aspect;
            cam.orthographicSize = defaultWidth / cam.aspect;
            lastDefaultSize = defaultSize;
        }
    }
}
