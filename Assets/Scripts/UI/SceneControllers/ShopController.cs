using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    private bool selectedFace=false;
    private bool selectedHat =false;
    private bool selectedPack =false;

    private string face = "";
    private string hat = "";
    private int pack = -1;

    [SerializeField] private Button buyBtn;
    [SerializeField] private GameObject confirmPanel;
    void Start()
    {
        buyBtn.gameObject.SetActive(false);
        confirmPanel.SetActive(false);
    }

    public void OnClickChoiceButton()
    {
        selectedFace = false;
        selectedPack =false;
        selectedHat = false;
        buyBtn.gameObject.SetActive(false);
    }

    public void OnClickhat(Texture text)
    {
        string id = text.name;
        if (!selectedHat)
        {
            buyBtn.gameObject.SetActive(true);
        }
        hat = id;
    }
    public void OnClickPack(int id)
    {
        if (!selectedPack)
        {
            buyBtn.gameObject.SetActive(true);
        }
        pack = id;
    }

    public void OnClickFace(Texture text)
    {
        string id = text.name;
        if (!selectedFace)
        {
            buyBtn.gameObject.SetActive(true);
        }
        face = id;
    }

    public void OnClickConfirm()
    {
        //METER LO QUE SE HAYA COMPRADO EN LA VARIABLE DE PEDRO
        
    }
   
}
