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
    [SerializeField] private Bilingual bilingualRestrictions;

    private void Start()
    {
        ClientConnected.wrongDataEvent += ShowWrongData;
        if(!confirmInputField) nameInputField.text = GameManager.username;
    }

    private void OnDestroy()
    {
        ClientConnected.wrongDataEvent -= ShowWrongData;
    }

    public void ShowNameRestrictions()
    {
        bilingualRestrictions.spanishText = "El nombre puede contener letras, números, guion y barra baja";
        bilingualRestrictions.englishText = "The username can contain letters, numbers, hyphen and underscore";
        bilingualRestrictions.UpdateLanguage();
    }

    public void ShowPasswordRestrictions()
    {
        bilingualRestrictions.spanishText = "La contraseña puede contener letras, números, guion y barra baja";
        bilingualRestrictions.englishText = "The password can contain letters, numbers, hyphen and underscore";
        bilingualRestrictions.UpdateLanguage();
    }

    private void ShowWrongData(int errorCode)
    {
        Debug.Log("SHOW WRONG DATA: " + errorCode);

        GameManager.instance.ReleaseUI();

        
        switch (errorCode)
        {
            case 0:
                bilingualRestrictions.spanishText = "Contraseña incorrecta";
                bilingualRestrictions.englishText = "Wrong password";
                break;
            case 1:
                bilingualRestrictions.spanishText = "El usuario no existe";
                bilingualRestrictions.englishText = "User doesn't exist";
                break;
            case 2:
                bilingualRestrictions.spanishText = "Esta sesión de usuario ya está iniciada";
                bilingualRestrictions.englishText = "This user is already logged in";
                break;
            case 3:
                bilingualRestrictions.spanishText = "Ese nombre de usuario ya existe...";
                bilingualRestrictions.englishText = "That username already exists...";
                break;
            case 100:
                bilingualRestrictions.spanishText = "El nombre contiene caracteres no permitidos o está vacío";
                bilingualRestrictions.englishText = "The username contains restricted characters or it's empty";
                break;
            case 101:
                bilingualRestrictions.spanishText = "La contraseña contiene caracteres no permitidos o está vacía";
                bilingualRestrictions.englishText = "The password contains restricted characters or it's empty";
                break;
            case 102:
                bilingualRestrictions.spanishText = "Las contraseñas no coinciden";
                bilingualRestrictions.englishText = "The password fields don't match";
                break;
        }
        bilingualRestrictions.UpdateLanguage();
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
            GameManager.instance.BlockUI();
        } else if (!nameOk)
        {
            ShowWrongData(100);
        } else if (!passwordOk)
        {
            ShowWrongData(101);
        } else if (!samePassword)
        {
            ShowWrongData(102);
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
        GameManager.instance.BlockUI();
    }
}
