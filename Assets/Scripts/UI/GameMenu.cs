using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;


public class GameMenu : SingletonMonobehaviour<GameMenu>
{
    GameObject playerObject;
    Player player;
    TextMeshProUGUI scoreText;
    TextMeshProUGUI scoreInGameMenuText;
    TextMeshProUGUI rewardedAdText;
    GameObject buttonsBarObject;
    GameObject gameOverBarObject;
    GameObject inGameUI;
    Image backgroundImage;
    Camera camera;
    Image lockedAd;

    //Coroutine moveCamDownCoroutine;


    [HideInInspector] public int score;
    bool canShowRewardedAd;


    protected override void Awake()
    {
        base.Awake();

        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        scoreInGameMenuText = GameObject.FindGameObjectWithTag("ScoreInGameMenu").GetComponent<TextMeshProUGUI>();
        rewardedAdText = GameObject.FindGameObjectWithTag("RewardedAdText").GetComponent<TextMeshProUGUI>();
        buttonsBarObject = GameObject.FindGameObjectWithTag("ButtonsBar");
        gameOverBarObject = GameObject.FindGameObjectWithTag("GameOverBar");
        inGameUI = GameObject.FindGameObjectWithTag("InGameUI");
        backgroundImage = GameObject.FindGameObjectWithTag("GameOverAndPause").GetComponent<Image>();
        camera = Camera.main;
        lockedAd = GameObject.FindGameObjectWithTag("LockedAd").GetComponent<Image>();
    }

    void Start()
    {
    }

    void OnEnable()
    {
        player.enabled = true;
        Player.Instance.playerSprite.enabled = true;
    }
    void OnDisable()
    {
        player.enabled = false;
        Player.Instance.playerSprite.enabled = false;
    }



    //buttonsMethods
    public void Stop()
    {
        ToggleButtonsBar(true);
        player.enabled = false;

        GameManager.Instance.OpenMenu();
    }

    public void Play()
    {
        ToggleButtonsBar(false);
        player.enabled = true;
        inGameUI.SetActive(true);

        GameManager.Instance.CloseMenu();
    }

    public void Music()
    {
        Menu.Instance.EnableMusic(!Settings.IsMusicEnabled);
    }

    public void Home()
    {
        ClearGame();
        SetPlayer();
        Menu.Instance.OpenMenu();
        //moveCamDownCoroutine = StartCoroutine(MoveCamDown());
        SaveManager.Instance.SaveDataToFile();
    }

    public void Restart()
    {
        ClearGame();
        SetPlayer();
        GameManager.Instance.StartGame();
        player.enabled = true;
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

        GameManager.Instance.ClearGame();

        Menu.Instance.CountMoney(score);
        score = 0;
        ChangeScore(0);
    }


    /*
    IEnumerator MoveCamDown()
    {
        float curY = camera.transform.position.y;
        while (camera.transform.position.y > Settings.posYForCamDown + 0.7f)
        {
            float y = Mathf.Lerp(camera.transform.position.y, Settings.posYForCamDown, 0.05f);

            camera.transform.position = new Vector2(camera.transform.position.x, y);
            yield return null;
        }

        yield return null;
    }
    */

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
