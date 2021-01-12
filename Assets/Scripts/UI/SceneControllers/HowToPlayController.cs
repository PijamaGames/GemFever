using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HowToPlayController : MonoBehaviour
{
    [SerializeField] Button continueBtn;
    [SerializeField] Button exitBtn;

    [SerializeField] GameObject[] pcTutorial;
    [SerializeField] GameObject[] mobileTutorial;
    GameObject[] tutorial;
    [SerializeField] Button nextBtn;
    [SerializeField] Button previousBtn;
    int index = 0;
    int maxIndex;

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

        foreach (var obj in mobileTutorial) obj.SetActive(false);
        foreach (var obj in pcTutorial) obj.SetActive(false);

        tutorial = GameManager.isHandheld ? mobileTutorial : pcTutorial;
        maxIndex = tutorial.Length;
        UpdateUI();
    }

    private void UpdateUI()
    {
        previousBtn.gameObject.SetActive(index > 0);
        nextBtn.gameObject.SetActive(index < maxIndex - 1);
        for(int i = 0; i < maxIndex; i++)
        {
            tutorial[i].SetActive(i == index);
        }
    }

    public void Next()
    {
        index++;
        if (index >= maxIndex) index = maxIndex - 1;
        UpdateUI();
    }

    public void Previous()
    {
        index--;
        if (index < 0) index = 0;
        UpdateUI();
    }
}
