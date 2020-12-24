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
    [SerializeField] private GameObject nameRestrictionPanel;
    private Bilingual billingualRestrictionsText;

    private void Start()
    {
        ClientConnected.wrongDataEvent += ShowWrongData;
        billingualRestrictionsText = nameRestrictionPanel.GetComponentInChildren<Bilingual>();
        if(!confirmInputField) nameInputField.text = GameManager.username;
    }

    private void OnDestroy()
    {
        ClientConnected.wrongDataEvent -= ShowWrongData;
    }

    private void ShowWrongData(int errorCode)
    {
        Debug.Log("SHOW WRONG DATA: " + errorCode);

        //TODO: Reflejar error en interfaz
        
        nameRestrictionPanel.gameObject.SetActive(true);
        switch (errorCode)
        {
            case 0:
                billingualRestrictionsText.spanishText = "Contraseña incorrecta";
                billingualRestrictionsText.englishText = "Wrong password";
                break;
            case 1:
                billingualRestrictionsText.spanishText = "El usuario no existe";
                billingualRestrictionsText.englishText = "User doesn't exist";
                break;
            case 2:
                billingualRestrictionsText.spanishText = "Esta sesión de usuario ya está iniciada";
                billingualRestrictionsText.englishText = "This user is already logged in";
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
        bool nameOk=CheckValidation(nameInputField);
        bool passwordOk= CheckValidation(passwordInputField);
        bool samePassword = passwordInputField.text.Equals(confirmInputField.text);
        
        //TODO: Comprobar que se cumplan todas las restricciones
        if (nameOk && passwordOk && samePassword)
        {
            Client.user = new User();
            Client.user.id = nameInputField.text.Trim();
            Client.user.password = passwordInputField.text.Trim();
            ClientConnected.SignUp();
        }
        
    }

    private bool CheckValidation(TMP_InputField inputfield)
    {
        string withoutSpaces=inputfield.text.Trim();
        int correctChar = 0;
        char character;
        for (int i = 0; i < inputfield.text.Length; i++)
        {
            character = inputfield.text[i];
            if (Char.IsNumber(character) || Char.IsLetter(character) || character == '-' || character == '_')
                correctChar++;

        }

        return correctChar > 0 && correctChar == inputfield.text.Length && withoutSpaces.Length == inputfield.text.Length;
    }

    public void TrySignIn()
    {
        Client.user = new User();
        Client.user.id = nameInputField.text.Trim();
        Client.user.password = passwordInputField.text.Trim();
        ClientConnected.SignIn();
    }


}
