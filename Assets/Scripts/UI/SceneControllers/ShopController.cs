﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    private bool selectedFace;
    private bool selectedHat;
    private bool selectedPack;

    private string face = "";
    private string hat = "";
    private int pack = -1;

    [SerializeField] private Button buyBtn;
    [SerializeField] private GameObject confirmPanel;
    [SerializeField] private GameObject cantBuyPanel;
    [SerializeField] private TextMeshProUGUI numGems;
    
    [SerializeField] private Texture[] shopFaces;
    [SerializeField] private int[] facesPrice;
    [SerializeField] private Texture[] shopHats;
    [SerializeField] private int[] hatsPrice;

    private Dictionary<string, int> hats;
    private Dictionary<string, int> faces;


    void Start()
    {
        hats = new Dictionary<string, int>();
        faces = new Dictionary<string, int>();
        for (int i=0; i < facesPrice.Length; i++)
        {
            faces.Add(shopFaces[i].name, facesPrice[i]);
        }
        for (int i = 0; i < hatsPrice.Length; i++)
        {
            hats.Add(shopHats[i].name, hatsPrice[i]);
        }

        selectedPack = false;
        selectedFace = false;
        selectedHat = false;
        buyBtn.gameObject.SetActive(false);
        confirmPanel.SetActive(false);
        cantBuyPanel.SetActive(false);
        UpdateGems();
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
            selectedHat = true;
        }
        hat = id;
    }
    public void OnClickPack(int numGems)
    {
        if (!selectedPack)
        {
            buyBtn.gameObject.SetActive(true);
            selectedPack=true;
        }
        pack = numGems;
    }

    public void OnClickFace(Texture text)
    {
        string id = text.name;
        if (!selectedFace)
        {
            buyBtn.gameObject.SetActive(true);
            selectedFace = true;
        }
        face = id;
    }

    public void OnClickConfirm()
    {
        if (selectedFace)
        {
            if (!Client.user.items_faces.Contains(face) && Client.user.gems >= faces[face])
            {
                List<string> aux = new List<string>(Client.user.items_faces);
                if (face.Equals("faceglasses1"))
                {
                    aux.Add("faceglasses2");
                }
                aux.Add(face);
                Client.user.gems -= faces[face];
                Client.user.items_faces = aux.ToArray();
            }
            
        }
        else if (selectedHat)
        {
            if (!Client.user.items_hats.Contains(hat) && Client.user.gems >= hats[hat])
            {
                List<string> aux = new List<string>(Client.user.items_hats);
                aux.Add(hat);
                Client.user.gems -= hats[hat];
                Client.user.items_hats = aux.ToArray();
            }
            
        }
        else if (selectedPack)
        {
            Client.user.gems += pack;
        }
        UpdateGems();
        ClientSignedIn.SaveInfo();
    }

    private void UpdateGems()
    {
        numGems.SetText("" + Client.user.gems); 
    }

    public void OnClickBuy()
    {
        if (selectedFace)
        {
            if (!Client.user.items_faces.Contains(face))
            {
                confirmPanel.SetActive(true);
            }
            else
            {
                cantBuyPanel.SetActive(true);
            }

        }
        else if (selectedHat)
        {
            if (!Client.user.items_hats.Contains(hat))
            {
                confirmPanel.SetActive(true);
            }
            else
            {
                cantBuyPanel.SetActive(true);
            }

        }
        else if (selectedPack)
        {
            confirmPanel.SetActive(true);
        }
    }

}
