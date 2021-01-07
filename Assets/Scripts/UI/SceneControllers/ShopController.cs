﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Channels;
using TMPro;
using UnityEditor.UI;
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
    [SerializeField] private TextMeshProUGUI numGems;
    void Start()
    {
        selectedPack = false;
        selectedFace = false;
        selectedHat = false;
        buyBtn.gameObject.SetActive(false);
        confirmPanel.SetActive(false);
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
            List<string> aux = new List<string>(Client.user.items_faces);
            aux.Add(face);
            Client.user.items_faces = aux.ToArray();
        }
        else if (selectedHat)
        {
            List<string> aux = new List<string>(Client.user.items_hats);
            aux.Add(hat);
            Client.user.items_hats = aux.ToArray();
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

}