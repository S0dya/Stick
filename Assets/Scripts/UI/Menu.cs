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

    Coroutine moveCamUpCoroutine;


    void Awake()
    {   
        base.Awake();

        camera = Camera.main;
        menuBottomButtonsBarObject = GameObject.FindGameObjectWithTag("MenuBottomButtonsBar");
        gameMenuObject = GameObject.FindGameObjectWithTag("GameMenu");
        menuObject = GameObject.FindGameObjectWithTag("Menu");
        
        GameObject[] cancelledMusicObjects = GameObject.FindGameObjectsWithTag("CancelledMusicObject");
        cancelledMusicImages = new Image[cancelledMusicObjects.Length];
        for (int i = 0; i < cancelledMusicImages.Length; i++)
            cancelledMusicImages[i] = (cancelledMusicObjects[i].GetComponent<Image>());

    }

    void Start()
    {
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
        moveCamUpCoroutine = StartCoroutine(MoveCamUp());
        Shop.Instance.SetSkin(Settings.SetGekoSkinIndex);
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
        Settings.Money += score / 5f;
    }

    
    IEnumerator MoveCamUp()
    {
        CloseMenu();
        float curY = camera.transform.position.y;
        while (camera.transform.position.y < Settings.posYForCamUp - 0.7f)
        {
            float y = Mathf.Lerp(camera.transform.position.y, Settings.posYForCamUp, 0.05f);

            camera.transform.position = new Vector2(camera.transform.position.x, y);
            yield return null;
        }

        
        StartGame();
        yield return null;
    }

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
    }

    public void ToggleButtonsBar(bool val)
    {
        menuBottomButtonsBarObject.SetActive(val);
    }
}
