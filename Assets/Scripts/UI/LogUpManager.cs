using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class LogUpManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TMP_InputField passwordInputField;
    //[SerializeField] private TMP_InputField confirmPasswordInputField;
    [SerializeField] private Image passwordImgRestrictions;
    [SerializeField] private Image nameImgRestrictions;
    [SerializeField] private TextMeshProUGUI nameRestrictionsText;
    [SerializeField] private TextMeshProUGUI passwordRestrictionsText;
    private bool visible=false;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
