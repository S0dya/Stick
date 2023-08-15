using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : SingletonMonobehaviour<Shop>
{
    public string[] skinNames;
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

    [SerializeField] GameObject skinPriceObject;
    [SerializeField] GameObject backgroundPriceObject;
    TextMeshProUGUI skinPriceText;
    TextMeshProUGUI backgroundPriceText;

    protected override void Awake()
    {
        base.Awake();

        skinPriceText = skinPriceObject.GetComponentInChildren<TextMeshProUGUI>();
        backgroundPriceText = backgroundPriceObject.GetComponentInChildren<TextMeshProUGUI>();
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
            LockSkin(GameManager.Instance.skinsPrices[Settings.GekoSkinIndex] > 0);
        }
    }

    public void RightSlideSkin()
    {
        if (Settings.GekoSkinIndex < GameManager.Instance.GekoSkins.Length - 1)
        {
            Settings.GekoSkinIndex++;
            TestSkin(Settings.GekoSkinIndex);
            LockSkin(GameManager.Instance.skinsPrices[Settings.GekoSkinIndex] > 0);
        }
    }

    public void LeftSlideBackground()
    {
        if (Settings.BackgroundIndex > 0)
        {
            Settings.BackgroundIndex--;
            TestBackground(Settings.BackgroundIndex);
            LockBackground(GameManager.Instance.backgroundsPrices[Settings.BackgroundIndex] > 0);
        }
    }

    public void RightSlideBackground()
    {
        if (Settings.BackgroundIndex < GameManager.Instance.backgrounds.Length - 1)
        {
            Settings.BackgroundIndex++;
            TestBackground(Settings.BackgroundIndex);
            LockBackground(GameManager.Instance.backgroundsPrices[Settings.BackgroundIndex] > 0);
        }
    }

    public void SetSkinButton()
    {
        Settings.SetGekoSkinIndex = Settings.GekoSkinIndex;
    }

    public void BuySkinButton()
    {
        if (Settings.Money >= GameManager.Instance.skinsPrices[Settings.GekoSkinIndex])
        {
            Settings.Money -= GameManager.Instance.skinsPrices[Settings.GekoSkinIndex];
            GameManager.Instance.skinsPrices[Settings.GekoSkinIndex] = 0;
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
        if (Settings.Money >= GameManager.Instance.backgroundsPrices[Settings.BackgroundIndex])
        {
            Settings.Money -= GameManager.Instance.backgroundsPrices[Settings.BackgroundIndex];
            GameManager.Instance.backgroundsPrices[Settings.BackgroundIndex] = 0;
            LockBackground(false);
        }
    }

    //methods
    public void LockSkin(bool val)
    {
        blockedSkin.enabled = val;
        setSkinImage.SetActive(!val);
        skinPriceObject.SetActive(val);
        if (val)
        {
            skinPriceText.text = GameManager.Instance.skinsPrices[Settings.GekoSkinIndex].ToString();
        }
    }
    public void LockBackground(bool val)
    {
        blockedBackground.enabled = val;
        setBackgroundImage.SetActive(!val);
        backgroundPriceObject.SetActive(val);
        if (val)
        {
            backgroundPriceText.text = GameManager.Instance.backgroundsPrices[Settings.BackgroundIndex].ToString();
        }
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
