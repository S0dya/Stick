using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenu : SingletonMonobehaviour<GameMenu>
{
    Player player;
    TextMeshProUGUI scoreText;
    GameObject buttonsBarObject;
    GameObject gameOverBarObject;
    Image backgroundImage;

    int currentScore;


    protected override void Awake()
    {
        base.Awake();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        buttonsBarObject = GameObject.FindGameObjectWithTag("ButtonsBar");
        gameOverBarObject = GameObject.FindGameObjectWithTag("GameOverBar");
        backgroundImage = GetComponent<Image>();
    }

    void Start()
    {
        ClearGame();
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

        GameManager.Instance.CloseMenu();
    }

    public void Music()
    {

    }

    public void Home()
    {

    }

    public void Restart()
    {
        ClearGame();
        GameManager.Instance.isMenuOpen = false;

        player.enabled = true;
    }

    public void Ad()
    {

    }

    //methods
    public void GameOver()
    {
        ToggleGameOverBar(true);

        player.enabled = false;
        GameManager.Instance.OpenMenu();
    }

    public void ClearGame()
    {
        ToggleGameOverBar(false);
        ToggleButtonsBar(false);

        Player.Instance.health = 2;
        HPBar.Instance.ResetHPImages();

        Player.Instance.tongueLength = 0f;

        GameManager.Instance.ClearGame();

        currentScore = 0;
        ChangeScore(0);
    }


    

    public void ChangeScore(float value)
    {
        int endValue = Mathf.FloorToInt(value);
        currentScore += endValue;
        scoreText.text = currentScore.ToString();
    }

    public void ToggleGameOverBar(bool val)
    {
        backgroundImage.enabled = val;
        gameOverBarObject.SetActive(val);
    }

    public void ToggleButtonsBar(bool val)
    {
        backgroundImage.enabled = val;
        buttonsBarObject.SetActive(val);
    }
}
