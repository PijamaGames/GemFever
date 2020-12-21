using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpManager : MonoBehaviour
{
    // Start is called before the first frame update
    [Header("Fields")]
    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TMP_InputField confirmInputField;
    [Header("Restrictions")]
    [SerializeField] private Image passwordImgRestrictions;
    [SerializeField] private Image nameImgRestrictions;
    [SerializeField] private TextMeshProUGUI nameRestrictionsText;
    [SerializeField] private TextMeshProUGUI passwordRestrictionsText;
    private bool visible=false;

    public void TrySignUp()
    {
        //TODO: Comprobar que se cumplan todas las restricciones
        Client.user = new User();
        Client.user.id = nameInputField.text.Trim();
        Client.user.password = passwordInputField.text.Trim();
        ClientConnected.SignUp();
    }

    public void ChangePasswordVisibility(TMP_InputField passInputField)
    {
        visible = !visible;
        passInputField.inputType = visible? TMP_InputField.InputType.Standard: TMP_InputField.InputType.Password;
        passInputField.ActivateInputField();
    }

    public void ShowPasswordRestrictions()
    {
        passwordRestrictionsText.enabled = true;
        passwordImgRestrictions.enabled = true;
    }
    public void QuitPasswordRestrictions()
    {
        passwordRestrictionsText.enabled = false;
        passwordImgRestrictions.enabled = false;
    }

    public void QuitNameRestrictions()
    {
        nameRestrictionsText.enabled = false;
        nameImgRestrictions.enabled = false;
    }

    public void ShowNameRestrictions()
    {
        nameRestrictionsText.enabled = true;
        nameImgRestrictions.enabled = true;
    }
}
