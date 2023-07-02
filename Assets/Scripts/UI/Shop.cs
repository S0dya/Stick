using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : SingletonMonobehaviour<Shop>
{
    public int[] skinsPrices;
    public string[] skinNames;

    public int[] backgroundPrices;
    public string[] backgroundNames;

    [SerializeField] GameObject shop;
    [SerializeField] Image blockedBackground;
    [SerializeField] Image backgroundImage;
    [SerializeField] GameObject setBackgroundImage;
    [SerializeField] Image blockedSkin;
    [SerializeField] GameObject setSkinImage;
    [SerializeField] Image skin;
    [SerializeField] TextMeshProUGUI skinName;
    [SerializeField] TextMeshProUGUI backgroundName;

    protected override void Awake()
    {
        base.Awake();

    }

    public void OpenShop()
    {
        LockSkin(false);
        Settings.GekoSkinIndex = Settings.SetGekoSkinIndex;
        TestSkin(Settings.GekoSkinIndex);

        LockBackground(false);
        Settings.BackgroundIndex = Settings.SetBackgroundIndex;
        TestBackground(Settings.BackgroundIndex);

        shop.SetActive(true);
    }

    public void CloseShop()
    {
        shop.SetActive(false);
    }

    //Buttons
    public void Home()
    {
        CloseShop();
        SaveManager.Instance.SaveDataToFile();
        Menu.Instance.OpenMenu();
    }

    public void DonateButton()
    {
        CloseShop();
        Donate.Instance.OpenDonate();
    }

    public void LeftSlideSkin()
    {
        if (Settings.GekoSkinIndex > 0)
        {
            Settings.GekoSkinIndex--;
            TestSkin(Settings.GekoSkinIndex);
            LockSkin(skinsPrices[Settings.GekoSkinIndex] > 0);
        }
    }

    public void RightSlideSkin()
    {
        if (Settings.GekoSkinIndex < GameManager.Instance.GekoSkins.Length - 1)
        {
            Settings.GekoSkinIndex++;
            TestSkin(Settings.GekoSkinIndex);
            LockSkin(skinsPrices[Settings.GekoSkinIndex] > 0);
        }
    }

    public void LeftSlideBackground()
    {
        if (Settings.BackgroundIndex > 0)
        {
            Settings.BackgroundIndex--;
            TestBackground(Settings.BackgroundIndex);
            LockBackground(backgroundPrices[Settings.BackgroundIndex] > 0);
        }
    }

    public void RightSlideBackground()
    {
        if (Settings.BackgroundIndex < GameManager.Instance.backgrounds.Length - 1)
        {
            Settings.BackgroundIndex++;
            TestBackground(Settings.BackgroundIndex);
            LockBackground(backgroundPrices[Settings.BackgroundIndex] > 0);
        }
    }

    public void SetSkinButton()
    {
        Settings.SetGekoSkinIndex = Settings.GekoSkinIndex;
    }

    public void BuySkinButton()
    {
        if (Settings.Money >= skinsPrices[Settings.GekoSkinIndex])
        {
            Settings.Money -= skinsPrices[Settings.GekoSkinIndex];
            skinsPrices[Settings.GekoSkinIndex] = 0;
            Menu.Instance.moneyAmount.text = Settings.Money.ToString();
            LockSkin(false);
        }
    }

    public void SetBackgroundButton()
    {
        Settings.SetBackgroundIndex = Settings.BackgroundIndex;
    }

    public void BuyBackgroundButton()
    {
        if (Settings.Money >= backgroundPrices[Settings.BackgroundIndex])
        {
            Settings.Money -= backgroundPrices[Settings.BackgroundIndex];
            backgroundPrices[Settings.BackgroundIndex] = 0;
            LockBackground(false);
        }
    }

    //methods
    public void LockSkin(bool val)
    {
        blockedSkin.enabled = val;
        setSkinImage.SetActive(!val);
    }
    public void LockBackground(bool val)
    {
        blockedBackground.enabled = val;
        setBackgroundImage.SetActive(!val);
    }

    public void TestSkin(int i)
    {
        skinName.text = skinNames[i];
        skin.sprite = GameManager.Instance.GekoSkins[i];
    }
    
    public void TestBackground(int i)
    {
        backgroundName.text = backgroundNames[i];
        backgroundImage.sprite = GameManager.Instance.backgrounds[i];
    }
}
