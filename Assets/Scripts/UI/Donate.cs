using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Donate : SingletonMonobehaviour<Donate>
{
    [SerializeField] float rewardAdAmount;
    [SerializeField] float[] donatePrices;
    [SerializeField] float[] donateAmounts;

    [SerializeField] GameObject donate;

    [SerializeField] TextMeshProUGUI rewardText;
    [SerializeField] TextMeshProUGUI[] donateTexts;

    protected override void Awake()
    {
        base.Awake();

        rewardText.text = $"<color=yellow>{rewardAdAmount}</color> for watching <color=green>ad</color>";

        for (int i = 0; i < donateTexts.Length; i++)
        {
            donateTexts[i].text = $"<color=yellow>{donateAmounts[i]}</color> for <color=green>{donatePrices[i]}</color>$";
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

    public void RewardedAdButton()
    {
        AdsManager.Instance.ShowRewardedAd();
    }

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
