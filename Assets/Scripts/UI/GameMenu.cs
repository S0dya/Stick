using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;
using UnityEngine.Rendering.Universal;


public class GameMenu : SingletonMonobehaviour<GameMenu>
{
    [SerializeField] GameObject playerObject;
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI scoreInGameMenuText;
    [SerializeField] TextMeshProUGUI rewardedAdText;
    [SerializeField] GameObject buttonsBarObject;
    [SerializeField] GameObject gameOverBarObject;
    [SerializeField] GameObject inGameUI;
    [SerializeField] Image backgroundImage;
    [SerializeField] Image lockedAd;
    [SerializeField] Image cancelledMusicImage;
    [SerializeField] Light2D globalLight;
    Player player;

    bool isDay;
    bool isNight;
    int scoreNededForNightChange;
    int scoreNededForDayChange;
    Coroutine changeToDay;
    Coroutine changeToNight;

    [HideInInspector] public int score;
    bool canShowRewardedAd;

    protected override void Awake()
    {
        base.Awake();

        player = playerObject.GetComponent<Player>();
    }

    void Start()
    {
        CheckSound();
        ToggleInGameMenu(true);
        ClearGame();
        player.SetSkin(Settings.SetGekoSkinIndex);
        player.SetBackground(Settings.SetBackgroundIndex);
        GameManager.Instance.StartGame();
        AudioManager.Instance.ToggleMusic(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            ChangeScore(50);
        }
    }

    //buttonsMethods
    public void Stop()
    {
        ToggleButtonsBar(true);
        player.enabled = false;
        AudioManager.Instance.ChangeMusic();

        GameManager.Instance.OpenMenu();
    }

    public void Play()
    {
        ToggleButtonsBar(false);
        player.enabled = true;
        inGameUI.SetActive(true);
        AudioManager.Instance.ChangeMusic();

        GameManager.Instance.CloseMenu();
    }

    public void Music()
    {
        EnableMusic(!Settings.IsMusicEnabled);
    }

    public void Home()
    {
        LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadSceneAsync(1, 2));
        ClearGame();
        SetPlayer();
        SaveManager.Instance.SaveDataToFile();
        AudioManager.Instance.ToggleMusic(false);

    }

    public void Restart()
    {
        ClearGame();
        SetPlayer();
        GameManager.Instance.StartGame();
        player.enabled = true;
        AudioManager.Instance.EventInstancesDict["Music"].start();
    }

    public void RewardedAdButton()
    {
        if (canShowRewardedAd)
        {
            AdsManager.Instance.ShowRewardedAd();

            ToggleRewardedAd(false);
        }
    }

    //gameMethods
    void EnableMusic(bool val)
    {
        cancelledMusicImage.enabled = !val;
        AudioManager.Instance.ToggleSound(val);
        Settings.IsMusicEnabled = val;
    }
    void CheckSound()
    {
        if (!Settings.IsMusicEnabled)
        {
            cancelledMusicImage.enabled = true;
        }
    }

    public void GameOver()
    {
        ToggleGameOverBar(true);
        ToggleInGameMenu(false);

        ShowScoreInGameMenu();
        AudioManager.Instance.PlayOneShot("GameOverSound");
        AudioManager.Instance.ToggleMusic(false);

        player.enabled = false;
        GameManager.Instance.OpenMenu();
    }

    public void ClearGame()
    {
        ToggleGameOverBar(false);
        ToggleButtonsBar(false);
        ToggleRewardedAd(true);
        ToggleInGameMenu(true);

        Player.Instance.health = 2;
        HPBar.Instance.ResetHPImages();
        Player.Instance.tongueLength = 0f;
        Tongue.Instance.TurnOffMultyplaing();

        GameManager.Instance.ClearGame();

        Menu.Instance.CountMoney(score);

        globalLight.intensity = 1;
        scoreNededForNightChange = Settings.scoreNeededForNightChange;
        scoreNededForDayChange = Settings.scoreNeededForDayChange;
        isDay = true;
        isNight = false;

        score = 0;
        ChangeScore(0);
    }

    //inGameMethods
    public void ChangeScore(float value)
    {
        int endValue = Mathf.FloorToInt(value);
        score += endValue;
        score = Mathf.Max(score, 0);
        CheckDayNightChange();
        scoreText.text = score.ToString();
    }

    void CheckDayNightChange()
    {
        if (isDay && score > scoreNededForNightChange)
        {
            scoreNededForNightChange += 100;
            isDay = false;
            isNight = true;
            if (changeToDay != null)
            {
                StopCoroutine(changeToDay);
            }
            changeToNight = StartCoroutine(ChangeToNight());
        }
        else if (isNight && score > scoreNededForDayChange)
        {
            scoreNededForDayChange += 100;
            isDay = true;
            isNight = false;
            if (changeToNight != null)
            {
                StopCoroutine(changeToNight);
            }
            changeToDay = StartCoroutine(ChangeToDay());
        }
    }

    public void DoubleScore()
    {
        if (score > 0)
        {
            score *= 2;
            scoreText.text = score.ToString();
        }
    }

    public void ShowScoreInGameMenu()
    {
        StringBuilder score = new StringBuilder();
        score.Append("Score:");
        score.AppendLine();
        score.Append(scoreText.text.ToString());
        scoreInGameMenuText.text = score.ToString();
    }

    public void ToggleGameOverBar(bool val)
    {
        GameManager.Instance.isGameMenuOpen = val;
        backgroundImage.enabled = val;
        gameOverBarObject.SetActive(val);
    }

    public void ToggleButtonsBar(bool val)
    {
        GameManager.Instance.isGameMenuOpen = val;
        backgroundImage.enabled = val;
        buttonsBarObject.SetActive(val);
    }

    public void ToggleInGameMenu(bool val)
    {
        inGameUI.SetActive(val);
    }

    public void ToggleRewardedAd(bool val)
    {
        canShowRewardedAd = false;
        rewardedAdText.enabled = val;
        lockedAd.enabled = !val;
        canShowRewardedAd = val;
    }

    public void SetPlayer()
    {
        playerObject.transform.rotation = Quaternion.AngleAxis(90f, Vector3.forward);
        player.tongueLine.SetPosition(0, new Vector3(player.tongueLength, 0f, 0f));
        player.tongueCollider.offset = new Vector2(player.tongueLength / 2 - 0.01f, 0f);
        player.tongueCollider.size = new Vector2(player.tongueLength - 0.01f, 0.14f);
        player.nearTongueCollider.offset = new Vector2(player.tongueLength / 2 - 0.01f, 0f);
        player.nearTongueCollider.size = new Vector2(player.tongueLength - 0.01f, 0.14f);
        player.canElongate = true;
        player.isOutOfTrigger = false;
        player.isSticked = false;
    }

    IEnumerator ChangeToNight()
    {
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(7f));
        while (globalLight.intensity > 0.3f)
        {
            if (GameManager.Instance.isGameMenuOpen)
            {
                yield return null;
            }
            globalLight.intensity = Mathf.Lerp(globalLight.intensity, 0.1f, Time.deltaTime * 0.05f);

            yield return null;
        }
    }
    IEnumerator ChangeToDay()
    {
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(7f));
        while (globalLight.intensity < 1)
        {
            if (GameManager.Instance.isGameMenuOpen)
            {
                yield return null;
            }
            globalLight.intensity = Mathf.Lerp(globalLight.intensity, 1.1f, Time.deltaTime * 0.7f);

            yield return null;
        }
    }
}
