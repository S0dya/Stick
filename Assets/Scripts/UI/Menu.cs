using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Menu : SingletonMonobehaviour<Menu>
{
    Camera camera;
    GameObject menuBottomButtonsBarObject;
    GameObject gameMenuObject;
    GameObject menuObject;
    GameObject shop;
    Image[] cancelledMusicImages;
    [HideInInspector] public TextMeshProUGUI moneyAmount;

    //Coroutine moveCamUpCoroutine;


    void Awake()
    {   
        base.Awake();

        camera = Camera.main;
        menuBottomButtonsBarObject = GameObject.FindGameObjectWithTag("MenuBottomButtonsBar");
        gameMenuObject = GameObject.FindGameObjectWithTag("GameMenu");
        menuObject = GameObject.FindGameObjectWithTag("Menu");
        moneyAmount = GameObject.FindGameObjectWithTag("MoneyAmount").GetComponent<TextMeshProUGUI>();

        GameObject[] cancelledMusicObjects = GameObject.FindGameObjectsWithTag("CancelledMusicObject");
        cancelledMusicImages = new Image[cancelledMusicObjects.Length];
        for (int i = 0; i < cancelledMusicImages.Length; i++)
            cancelledMusicImages[i] = (cancelledMusicObjects[i].GetComponent<Image>());

    }

    void Start()
    {
        SaveManager.Instance.SaveDataToFile();
        OpenMenu();

    }

    public void OpenMenu()
    {
        menuObject.SetActive(true);
        gameMenuObject.SetActive(false);
        Shop.Instance.CloseShop();
        Donate.Instance.CloseDonate();
    }

    //ButtonsMethods
    public void Play()
    {
        //moveCamUpCoroutine = StartCoroutine(MoveCamUp());
        CloseMenu();
        StartGame();
        Shop.Instance.SetSkin(Settings.SetGekoSkinIndex);
        Shop.Instance.SetBackground(Settings.SetBackgroundIndex);
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
            im.enabled = val;
        }
        Settings.IsMusicEnabled = val;
    }

    public void CountMoney(float score)
    {
        Settings.Money += Mathf.FloorToInt(score / 5f);
        moneyAmount.text = Settings.Money.ToString();
    }

    /*
    IEnumerator MoveCamUp()
    {
        float curY = camera.transform.position.y;
        while (camera.transform.position.y < Settings.posYForCamUp - 0.7f)
        {
            float y = Mathf.Lerp(camera.transform.position.y, Settings.posYForCamUp, 0.05f);

            camera.transform.position = new Vector2(camera.transform.position.x, y);
            yield return null;
        }

        
        yield return null;
    }
    */

    void CloseMenu()
    {
        menuObject.SetActive(false);
    }

    void StartGame()
    {
        Shop.Instance.CloseShop();
        gameMenuObject.SetActive(true);
        GameMenu.Instance.ClearGame();
        GameManager.Instance.StartGame();
        GameMenu.Instance.ToggleInGameMenu(true);

    }

    public void ToggleButtonsBar(bool val)
    {
        menuBottomButtonsBarObject.SetActive(val);
    }
}
