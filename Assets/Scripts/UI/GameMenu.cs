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
    Camera camera;
    GameObject menuObject;
    Coroutine moveCamDownCoroutine;

    int currentScore;


    protected override void Awake()
    {
        base.Awake();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        buttonsBarObject = GameObject.FindGameObjectWithTag("ButtonsBar");
        gameOverBarObject = GameObject.FindGameObjectWithTag("GameOverBar");
        backgroundImage = GameObject.FindGameObjectWithTag("GameOverAndPause").GetComponent<Image>();
        camera = Camera.main;
        menuObject = GameObject.FindGameObjectWithTag("Menu");
    }

    void Start()
    {
    }

    void OnEnable()
    {
        player.enabled = true;
    }
    void OnDisable()
    {
        player.enabled = false;
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
        ClearGame();
        moveCamDownCoroutine = StartCoroutine(MoveCamDown());//Del Coroueine Later

    }

    public void Restart()
    {
        ClearGame();
        GameManager.Instance.StartGame();
        player.enabled = true;
    }

    public void Ad()
    {

    }

    //gameMethods
    public void GameOver()
    {
        ToggleGameOverBar(true);

        Menu.Instance.CountMoney(currentScore);

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


    IEnumerator MoveCamDown()
    {
        float curY = camera.transform.position.y;
        while (camera.transform.position.y > Settings.posYForCamDown + 0.7f)
        {
            float y = Mathf.Lerp(camera.transform.position.y, Settings.posYForCamDown, 0.05f);

            camera.transform.position = new Vector2(camera.transform.position.x, y);
            yield return null;
        }

        menuObject.SetActive(true);
        gameObject.SetActive(false);
        yield return null;
    }

    //inGameMethods
    public void ChangeScore(float value)
    {
        int endValue = Mathf.FloorToInt(value);
        currentScore += endValue;
        currentScore = Mathf.Max(currentScore, 0);
        scoreText.text = currentScore.ToString();
    }

    public void ToggleGameOverBar(bool val)
    {
        GameManager.Instance.isMenuOpen = val;
        backgroundImage.enabled = val;
        gameOverBarObject.SetActive(val);
    }

    public void ToggleButtonsBar(bool val)
    {
        GameManager.Instance.isMenuOpen = val;
        backgroundImage.enabled = val;
        buttonsBarObject.SetActive(val);
    }
}
