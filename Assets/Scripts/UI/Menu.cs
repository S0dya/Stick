using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Menu : SingletonMonobehaviour<Menu>
{
    [SerializeField] Sprite[] GekoSkins;
    [SerializeField] int[] skinsPrices;

    [SerializeField] Sprite[] backgrounds;
    [SerializeField] int[] backgroundPrices;

    Camera camera;
    GameObject menuBottomButtonsBarObject;
    GameObject gameMenuObject;
    GameObject[] cancelledMusicObjects;
    List<Image> cancelledMusicImages = new List<Image>();
    Image blockedSkin;
    Image setSkinImage;
    GameObject menuObject;
    Image blockedBackground;
    Image backgroundImage;
    Image setBackgroundImage;

    SpriteRenderer background;
    SpriteRenderer playerSkin;

    Coroutine moveCamUpCoroutine;


    void Awake()
    {   
        base.Awake();

        camera = Camera.main;
        menuBottomButtonsBarObject = GameObject.FindGameObjectWithTag("MenuBottomButtonsBar");
        gameMenuObject = GameObject.FindGameObjectWithTag("GameMenu");
        cancelledMusicObjects = GameObject.FindGameObjectsWithTag("CancelledMusicObject");
        foreach(GameObject go in cancelledMusicObjects)
            cancelledMusicImages.Add(go.GetComponent<Image>());
        playerSkin = GameObject.FindGameObjectWithTag("Player").GetComponent<SpriteRenderer>();
        blockedSkin = GameObject.FindGameObjectWithTag("BlockedSkin").GetComponent<Image>();
        setSkinImage = GameObject.FindGameObjectWithTag("SetSkinImage").GetComponent<Image>();
        menuObject = GameObject.FindGameObjectWithTag("Menu");
        blockedBackground = GameObject.FindGameObjectWithTag("BlockedBackground").GetComponent<Image>();
        setBackgroundImage = GameObject.FindGameObjectWithTag("SetBackgroundImage").GetComponent<Image>();
        backgroundImage = GameObject.FindGameObjectWithTag("BackgroundImage").GetComponent<Image>();
        background = GameObject.FindGameObjectWithTag("Background").GetComponent<SpriteRenderer>();

    }

    void Start()
    {
        OpenMenu();
        
    }

    public void OpenMenu()
    {
        menuObject.SetActive(true);
        gameMenuObject.SetActive(false);
        LockSkin(false);
        Settings.GekoSkinIndex = Settings.SetGekoSkinIndex;
        SetSkin(Settings.GekoSkinIndex);
    }

    //ButtonsMethods
    public void LeftSlideSkin()
    {
        if (Settings.GekoSkinIndex > 0)
        {
            Settings.GekoSkinIndex--;
            SetSkin(Settings.GekoSkinIndex);
            LockSkin(skinsPrices[Settings.GekoSkinIndex] > 0);
        }
    }

    public void RightSlideSkin()
    {
        if (Settings.GekoSkinIndex < GekoSkins.Length-1)
        {
            Settings.GekoSkinIndex++;
            SetSkin(Settings.GekoSkinIndex);
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

    public void Music()
    {
        EnableMusic(!Settings.IsMusicEnabled);
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

    public void SetSkin(int i)
    {
        playerSkin.sprite = GekoSkins[i];
    }
    public void TestBackground(int i)
    {
        backgroundImage.sprite = backgrounds[i];
    }
    public void SetBackground(int i)
    {
        background.sprite = backgrounds[i];
    }






    public void CountMoney(float score)
    {
        Settings.Money += score / 5f;
    }

    public void Play()
    {
        moveCamUpCoroutine = StartCoroutine(MoveCamUp());
        SetSkin(Settings.SetGekoSkinIndex);
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
        gameMenuObject.SetActive(true);
        GameMenu.Instance.ClearGame();
        GameManager.Instance.StartGame();
    }

    public void ToggleButtonsBar(bool val)
    {
        menuBottomButtonsBarObject.SetActive(val);
    }
}
