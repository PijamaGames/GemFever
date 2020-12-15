using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AddVirtualKeyboardDevice : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (InputSystem.GetDevice<VirtualKeyboardDevice>() == null)
        {
            InputSystem.AddDevice<VirtualKeyboardDevice>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
