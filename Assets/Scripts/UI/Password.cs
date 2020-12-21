using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Password : MonoBehaviour
{
    private bool visible = false;

    private TMP_InputField passwordInputField;
    private Button visibilityBtn;

    private void Start()
    {
        passwordInputField = GetComponent<TMP_InputField>();
        visibilityBtn = GetComponentInChildren<Button>();
        visibilityBtn.onClick.AddListener(ChangePasswordVisibility);
    }

    public void ChangePasswordVisibility()
    {
        visible = !visible;
        passwordInputField.inputType = visible ? TMP_InputField.InputType.Standard : TMP_InputField.InputType.Password;
        passwordInputField.ActivateInputField();
    }
}
