using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayController : MonoBehaviour
{
    [SerializeField] Button continueBtn;
    [SerializeField] Button exitBtn;

    private void Start()
    {
        bool isSigningUp = Client.IsCurrentState(Client.signedUpState);
        continueBtn.gameObject.SetActive(isSigningUp);

        if (isSigningUp)
        {
            continueBtn.onClick.AddListener(() =>
            {
                ClientSignedUp.SignIn();
            });
            exitBtn.gameObject.SetActive(false);
        }
    }
}
