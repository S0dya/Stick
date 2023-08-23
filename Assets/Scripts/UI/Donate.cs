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
            donateTexts[i].text = $"<color=yellow>{donateAmounts[i]}</color>";
        }
    }

    public void OpenDonate()
    {
        Menu.Instance.inDonate = true;
        donate.SetActive(true);
    }

    public void CloseDonate()
    {
        Menu.Instance.inDonate = false;
        donate.SetActive(false);
    }
    //Buttons

    public void RewardedAdButton()
    {
        AdsManager.Instance.ShowRewardedAd();
        SaveManager.Instance.SaveDataToFile();
    }

    public void DonateButton(int i)
    {
        IAPManager.instance.OnBuyDonate(i);
    }

    public void ShopButton()
    {
        CloseDonate();
        Shop.Instance.OpenShop();
    }


}
