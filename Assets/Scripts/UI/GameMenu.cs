using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenu : SingletonMonobehaviour<GameMenu>
{
    GameObject playerObject;
    Player player;
    TextMeshProUGUI scoreText;
    GameObject buttonsBarObject;
    GameObject gameOverBarObject;
    Image backgroundImage;
    Camera camera;
    
    //Coroutine moveCamDownCoroutine;


    int currentScore;


    protected override void Awake()
    {
        base.Awake();

        playerObject = GameObject.FindGameObjectWithTag("Player");
        player = playerObject.GetComponent<Player>();
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
        buttonsBarObject = GameObject.FindGameObjectWithTag("ButtonsBar");
        gameOverBarObject = GameObject.FindGameObjectWithTag("GameOverBar");
        backgroundImage = GameObject.FindGameObjectWithTag("GameOverAndPause").GetComponent<Image>();
        camera = Camera.main;
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
        Menu.Instance.EnableMusic(!Settings.IsMusicEnabled);
    }

    public void Home()
    {
        ClearGame();
        SetPlayer();
        Menu.Instance.OpenMenu();
        //moveCamDownCoroutine = StartCoroutine(MoveCamDown());
    }

    public void Restart()
    {
        ClearGame();
        SetPlayer();
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

        Menu.Instance.CountMoney(currentScore);
        currentScore = 0;
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
