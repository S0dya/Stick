using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : SingletonMonobehaviour<Menu>
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject shop;
    [SerializeField] Image cancelledMusicImage;
    public TextMeshProUGUI moneyAmount;

    [HideInInspector] public bool inMenu;
    [HideInInspector] public bool inShop;
    [HideInInspector] public bool inDonate;

    protected override void Awake()
    {   
        base.Awake();
        //SaveManager.Instance.SaveDataToFile();

    }

    void Start()
    {
        CheckSound();
        OpenMenu();
    }

    public void OpenMenu()
    {
        inMenu = true;
        menu.SetActive(true);
        Shop.Instance.CloseShop();
        Donate.Instance.CloseDonate();
    }

    //ButtonsMethods
    public void Play()
    {
        inMenu = false;
        LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadSceneAsync(2, 1));
        AudioManager.Instance.PlayOneShot("PlaySound");
    }

    public void Music()
    {
        EnableMusic(!Settings.IsMusicEnabled);
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ShopButton()
    {
        CloseMenu();
        Shop.Instance.OpenShop();
        moneyAmount.text = Settings.Money.ToString();
    }

    public void PlayButtonSound()
    {
        AudioManager.Instance.PlayOneShot("ButtonPress");
    }



    //OtherMethods
    void CheckSound()
    {
        if (!Settings.IsMusicEnabled)
        {
            cancelledMusicImage.enabled = true;
        }
    }

    void EnableMusic(bool val)
    {
        cancelledMusicImage.enabled = !val;
        AudioManager.Instance.ToggleSound(val);
        Settings.IsMusicEnabled = val;
    }

    public void CountMoney(float score)
    {
        Settings.Money += Mathf.FloorToInt(score / 5f);
        moneyAmount.text = Settings.Money.ToString();
    }

    void CloseMenu()
    {
        inMenu = false;
        menu.SetActive(false);
    }
}
