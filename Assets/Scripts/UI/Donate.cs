using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Donate : SingletonMonobehaviour<Donate>
{
    [SerializeField] float[] donatePrices;
    [SerializeField] float[] donateAmounts;

    [SerializeField] GameObject donate;

    [SerializeField] TextMeshProUGUI[] donateTexts;

    protected override void Awake()
    {
        base.Awake();

        for (int i = 0; i < donateTexts.Length; i++)
        {
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
    }
    //Buttons

    public void DonateButton(int i)
    {
        Debug.Log("don " + i);
    }

    public void ShopButton()
    {
        CloseDonate();
        SaveManager.Instance.SaveDataToFile();
        Shop.Instance.OpenShop();
    }
}
