using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Menu : SingletonMonobehaviour<Menu>
{
    [SerializeField] Sprite[] GekoSkins;
    [SerializeField] int[] skinsPrices;

    Camera camera;
    GameObject menuBottomButtonsBarObject;
    GameObject gameMenuObject;
    GameObject[] cancelledMusicObjects;
    List<Image> cancelledMusicImages = new List<Image>();
    Image blockedSkin;
    Image setButtonGameImage;

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
        setButtonGameImage = GameObject.FindGameObjectWithTag("SetSkinImage").GetComponent<Image>();
    }

    void Start()
    {
        gameMenuObject.SetActive(false);
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

    public void SetButton()
    {
        Settings.SetGekoSkinIndex = Settings.GekoSkinIndex;
        SetSkin(Settings.SetGekoSkinIndex);
    }

    public void BuyButton()
    {
        if (Settings.Money >= skinsPrices[Settings.GekoSkinIndex])
        {
            Settings.Money -= skinsPrices[Settings.GekoSkinIndex];
            skinsPrices[Settings.GekoSkinIndex] = 0;
            LockSkin(false);
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
        setButtonGameImage.enabled = !val;
    }

     public void SetSkin(int i)
    {
        playerSkin.sprite = GekoSkins[i];
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

    void StartGame()
    {
        gameMenuObject.SetActive(true);
        gameObject.SetActive(false);
        GameMenu.Instance.ClearGame();
        GameManager.Instance.StartGame();
    }

    public void ToggleButtonsBar(bool val)
    {
        menuBottomButtonsBarObject.SetActive(val);
    }
}
