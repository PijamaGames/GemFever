using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateKeyboard : MonoBehaviour
{
    [SerializeField] WebGLKeyboard.KeyboardController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller.enabled = GameManager.isHandheld;
    }
}
