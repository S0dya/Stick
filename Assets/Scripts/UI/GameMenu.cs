using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameMenu : SingletonMonobehaviour<GameMenu>
{
    Player player;
    TextMeshProUGUI scoreText;

    int currentScore;

    protected override void Awake()
    {
        base.Awake();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        scoreText = GameObject.FindGameObjectWithTag("Score").GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        currentScore = 0;
        ChangeScore(0);

        Toggle(false);
    }

    //buttonsMethods
    public void Stop()
    {
        Toggle(true);
        player.enabled = false;

        GameManager.Instance.OpenMenu();
    }

    public void Play()
    {
        Toggle(false);
        player.enabled = true;

        GameManager.Instance.CloseMenu();
    }

    public void Music()
    {

    }

    public void Home()
    {

    }

    


    public void ChangeScore(float value)
    {
        int endValue = Mathf.FloorToInt(value);
        currentScore += endValue;
        scoreText.text = currentScore.ToString();
    }

    public void Toggle(bool val)
    {
        gameObject.SetActive(val);
    }
}
