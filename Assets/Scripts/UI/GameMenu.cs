using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;


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
    Player player;

    [HideInInspector] public int score;
    bool canShowRewardedAd;

    protected override void Awake()
    {
        base.Awake();

        player = playerObject.GetComponent<Player>();
    }

    void Start()
    {
        ToggleInGameMenu(true);
        ClearGame();
        player.SetSkin(Settings.SetGekoSkinIndex);
        player.SetBackground(Settings.SetBackgroundIndex);
        GameManager.Instance.StartGame();
    }

    void OnEnable()
    {
        player.enabled = true;
        player.playerSprite.enabled = true;
    }
    void OnDisable()
    {
        player.enabled = false;
        player.playerSprite.enabled = false;
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
        Menu.Instance.EnableMusic(!Settings.IsMusicEnabled);
    }

    public void Home()
    {
        LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadSceneAsync(0, 1));
        ClearGame();
        SetPlayer();
        SaveManager.Instance.SaveDataToFile();
        AudioManager.Instance.ChangeMusic();
        AudioManager.Instance.EventInstancesDict["FliesAmbience"].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
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
    public void GameOver()
    {
        ToggleGameOverBar(true);
        ToggleInGameMenu(false);

        ShowScoreInGameMenu();
        AudioManager.Instance.PlayOneShot("GameOverSound");
        AudioManager.Instance.EventInstancesDict["Music"].stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

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
        score = 0;
        ChangeScore(0);
    }

    //inGameMethods
    public void ChangeScore(float value)
    {
        int endValue = Mathf.FloorToInt(value);
        score += endValue;
        score = Mathf.Max(score, 0);
        scoreText.text = score.ToString();
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
}
