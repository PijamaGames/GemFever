using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignController : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Fields")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField confirmInputField;


    private void Start()
    {
        ClientConnected.wrongDataEvent += ShowWrongData;
    }

    private void OnDestroy()
    {
        ClientConnected.wrongDataEvent -= ShowWrongData;
    }

    private void ShowWrongData(int errorCode)
    {
        Debug.Log("SHOW WRONG DATA: " + errorCode);

        //TODO: Reflejar error en interfaz
    }

    public void TrySignUp()
    {
        //TODO: Comprobar que se cumplan todas las restricciones
        Client.user = new User();
        Client.user.id = nameInputField.text.Trim();
        Client.user.password = passwordInputField.text.Trim();
        ClientConnected.SignUp();
    }

    public void TrySignIn()
    {
        Client.user = new User();
        Client.user.id = nameInputField.text.Trim();
        Client.user.password = passwordInputField.text.Trim();
        ClientConnected.SignIn();
    }


}
