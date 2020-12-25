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
    private Image visibilityImg;

    [SerializeField] Sprite visibleImg;
    [SerializeField] Sprite invisibleImg;

    private void Start()
    {
        passwordInputField = GetComponent<TMP_InputField>();
        visibilityBtn = GetComponentInChildren<Button>();
        visibilityImg = visibilityBtn.GetComponentInChildren<Image>();
        visibilityBtn.onClick.AddListener(ChangePasswordVisibility);
    }

    public void ChangePasswordVisibility()
    {
        visible = !visible;
        visibilityImg.sprite = visible ? visibleImg : invisibleImg;
        passwordInputField.inputType = visible ? TMP_InputField.InputType.Standard : TMP_InputField.InputType.Password;
        passwordInputField.ActivateInputField();
    }
}
