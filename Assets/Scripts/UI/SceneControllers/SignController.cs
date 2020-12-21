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
    private Bilingual billingualRestrictionsText;

    private void Start()
    {
        ClientConnected.wrongDataEvent += ShowWrongData;
        restrictionsText=restrictionImage.GetComponentInChildren<TextMeshProUGUI>();
        billingualRestrictionsText = restrictionImage.GetComponentInChildren<Bilingual>();
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
                billingualRestrictionsText.spanishText = "Contraseña incorrecta";
                billingualRestrictionsText.englishText = "Wrog password";
                break;
            case 1:
                billingualRestrictionsText.spanishText = "El usuario no existe";
                billingualRestrictionsText.englishText = "User don't exist";
                break;
            case 2:
                billingualRestrictionsText.spanishText = "Esta sesión de usuario ya está iniciada";
                billingualRestrictionsText.englishText = "This user account is already logged in";
                break;
            case 3:
                billingualRestrictionsText.spanishText = "El nombre de usuario ya existe";
                billingualRestrictionsText.englishText = "That username already exists";
                break;
        }
        billingualRestrictionsText.UpdateLanguage();
    }

    public void TrySignUp()
    {
        int correctChar=0;
        char character;
        for (int i = 0; i < nameInputField.text.Length; i++)
        {
            character = nameInputField.text[i];
            if (Char.IsNumber(character) || Char.IsLetter(character) || character=='-' || character=='_')
                correctChar++;

        }
        
        //TODO: Comprobar que se cumplan todas las restricciones
        if (correctChar>0 && correctChar == nameInputField.text.Length)
        {
            Client.user = new User();
            Client.user.id = nameInputField.text.Trim();
            Client.user.password = passwordInputField.text.Trim();
            ClientConnected.SignUp();
        }
        
    }

    public void TrySignIn()
    {
        Client.user = new User();
        Client.user.id = nameInputField.text.Trim();
        Client.user.password = passwordInputField.text.Trim();
        ClientConnected.SignIn();
    }


}
