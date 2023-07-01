using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : SingletonMonobehaviour<Menu>
{
    [SerializeField] GameObject menu;
    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject shop;
    [SerializeField] Image[] cancelledMusicImages;
    public TextMeshProUGUI moneyAmount;

    protected override void Awake()
    {   
        base.Awake();

    }

    void Start()
    {
        //SaveManager.Instance.SaveDataToFile();
        OpenMenu();

    }

    public void OpenMenu()
    {
        menu.SetActive(true);
        gameMenu.SetActive(false);
        Shop.Instance.CloseShop();
        Donate.Instance.CloseDonate();
    }

    //ButtonsMethods
    public void Play()
    {
        CloseMenu();
        StartGame();
        Shop.Instance.SetSkin(Settings.SetGekoSkinIndex);
        Shop.Instance.SetBackground(Settings.SetBackgroundIndex);
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


    //OtherMethods
    public void EnableMusic(bool val)
    {
        foreach(Image im in cancelledMusicImages)
        {
            im.enabled = !val;
        }
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
        menu.SetActive(false);
    }

    void StartGame()
    {
        Shop.Instance.CloseShop();
        gameMenu.SetActive(true);
        GameMenu.Instance.ClearGame();
        GameManager.Instance.StartGame();
        GameMenu.Instance.ToggleInGameMenu(true);
        AudioManager.Instance.ChangeMusic();
        AudioManager.Instance.EventInstancesDict["FliesAmbience"].start();
    }
}
