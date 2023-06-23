using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Donate : SingletonMonobehaviour<Donate>
{
    [SerializeField] float[] donatePrices;
    [SerializeField] float[] donateAmounts;

    GameObject donate;

    TextMeshProUGUI[] donateTexts;

    protected override void Awake()
    {
        base.Awake();

        donate = GameObject.FindGameObjectWithTag("Donate");

        GameObject[] donateObjects = GameObject.FindGameObjectsWithTag("DonateText");
        donateTexts = new TextMeshProUGUI[donateObjects.Length];
        for (int i = 0; i < donateTexts.Length; i++)
        {
            donateTexts[i] = (donateObjects[i].GetComponent<TextMeshProUGUI>());
            donateTexts[i].text = $"{donateAmounts[i]} for {donatePrices[i]}$";
        }
    }

    public void OpenDonate()
    {
        donate.SetActive(true);
    }

    public void CloseDonate()
    {
        donate.SetActive(false);
        SaveManager.Instance.SaveDataToFile();
    }
    //Buttons

    public void DonateButton(int i)
    {
        Debug.Log("don " + i);
    }

    public void ShopButton()
    {
        CloseDonate();
        Shop.Instance.OpenShop();
    }

    //Methods


}
