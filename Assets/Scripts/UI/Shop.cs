using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : SingletonMonobehaviour<Shop>
{
    public Sprite[] GekoSkins;
    public int[] skinsPrices;
    public string[] skinNames;

    public Sprite[] backgrounds;
    public int[] backgroundPrices;
    public string[] backgroundNames;

    GameObject shop;
    Image blockedBackground;
    Image backgroundImage;
    Image setBackgroundImage;
    Image blockedSkin;
    Image setSkinImage;
    Image playerImage;
    TextMeshProUGUI skinName;
    TextMeshProUGUI backgroundName;

    SpriteRenderer background;
    SpriteRenderer playerSkin;

    protected override void Awake()
    {
        base.Awake();

        shop = GameObject.FindGameObjectWithTag("Shop");
        playerSkin = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        blockedSkin = GameObject.FindGameObjectWithTag("BlockedSkin").GetComponent<Image>();
        setSkinImage = GameObject.FindGameObjectWithTag("SetSkinImage").GetComponent<Image>();
        blockedBackground = GameObject.FindGameObjectWithTag("BlockedBackground").GetComponent<Image>();
        setBackgroundImage = GameObject.FindGameObjectWithTag("SetBackgroundImage").GetComponent<Image>();
        backgroundImage = GameObject.FindGameObjectWithTag("BackgroundImage").GetComponent<Image>();
        background = GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>();
        playerImage = GameObject.FindGameObjectWithTag("Skin").GetComponent<Image>();
        skinName = GameObject.FindGameObjectWithTag("SkinName").GetComponent<TextMeshProUGUI>();
        backgroundName = GameObject.FindGameObjectWithTag("BackgroundName").GetComponent<TextMeshProUGUI>();
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
        SaveManager.Instance.SaveDataToFile();
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
            LockSkin(skinsPrices[Settings.GekoSkinIndex] > 0);
        }
    }

    public void RightSlideSkin()
    {
        if (Settings.GekoSkinIndex < GekoSkins.Length - 1)
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
        if (Settings.BackgroundIndex < backgrounds.Length - 1)
        {
            Settings.BackgroundIndex++;
            TestBackground(Settings.BackgroundIndex);
            LockBackground(backgroundPrices[Settings.BackgroundIndex] > 0);
        }
    }

    public void SetSkinButton()
    {
        Settings.SetGekoSkinIndex = Settings.GekoSkinIndex;
        SetSkin(Settings.SetGekoSkinIndex);
    }

    public void BuySkinButton()
    {
        if (Settings.Money >= skinsPrices[Settings.GekoSkinIndex])
        {
            Settings.Money -= skinsPrices[Settings.GekoSkinIndex];
            skinsPrices[Settings.GekoSkinIndex] = 0;
            LockSkin(false);
        }
    }

    public void SetBackgroundButton()
    {
        Settings.SetBackgroundIndex = Settings.BackgroundIndex;
        SetBackground(Settings.SetBackgroundIndex);
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
        setSkinImage.enabled = !val;
    }
    public void LockBackground(bool val)
    {
        blockedBackground.enabled = val;
        setBackgroundImage.enabled = !val;
    }

    public void TestSkin(int i)
    {
        skinName.text = skinNames[i];
        playerImage.sprite = GekoSkins[i];
    }
    public void SetSkin(int i)
    {
        playerSkin.sprite = GekoSkins[i];
    }
    
    public void TestBackground(int i)
    {
        backgroundName.text = backgroundNames[i];
        backgroundImage.sprite = backgrounds[i];
    }
    public void SetBackground(int i)
    {
        background.sprite = backgrounds[i];
    }
}
