using System;
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
    [SerializeField] private Image restrictionImage;
    private TextMeshProUGUI restrictionsText;
    private Bilingual bil;

    private void Start()
    {
        ClientConnected.wrongDataEvent += ShowWrongData;
        restrictionsText=restrictionImage.GetComponentInChildren<TextMeshProUGUI>();
        bil = restrictionImage.GetComponentInChildren<Bilingual>();
    }

    private void OnDestroy()
    {
        ClientConnected.wrongDataEvent -= ShowWrongData;
    }

    private void ShowWrongData(int errorCode)
    {
        Debug.Log("SHOW WRONG DATA: " + errorCode);

        //TODO: Reflejar error en interfaz
        
        restrictionImage.gameObject.SetActive(true);
        switch (errorCode)
        {
            case 0:
                bil.spanishText = "Contraseña incorrecta";
                bil.englishText = "Wrog password";
                break;
            case 1:
                bil.spanishText = "El usuario no existe";
                bil.englishText = "User don't exist";
                break;
            case 2:
                bil.spanishText = "Esta sesión de usuario ya está iniciada";
                bil.englishText = "This user account is already logged in";
                break;
            case 3:
                bil.spanishText = "El nombre de usuario ya existe";
                bil.englishText = "That username already exit";
                break;
        }
        bil.UpdateLanguage();
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
