using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : SingletonMonobehaviour<PlayerTrigger>
{
    Coroutine scoreMultiplayerCoroutine;
    Player player;

    [HideInInspector] public bool isScoreMultiplaying;
    [HideInInspector] public float curMultiplayer;

    protected override void Awake()
    {
        base.Awake();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Start()
    {
        curMultiplayer = 1.5f;
        isScoreMultiplaying = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food") || collision.CompareTag("RestoreHP"))
        {
            SettingsAI settingsAi = collision.gameObject.GetComponent<SettingsAI>();
            if (settingsAi.isDestroying)
                return;

            if (scoreMultiplayerCoroutine != null)
            {
                if (curMultiplayer < 3)
                {
                    curMultiplayer += Settings.scoreMultiplyer;
                    Settings.curTongueMultiplyer++;
                }
                StopCoroutine(scoreMultiplayerCoroutine);
            }

            scoreMultiplayerCoroutine = StartCoroutine(ScoreMultiplayer());
            isScoreMultiplaying = true;

            HPBar.Instance.StopHunger();
            HPBar.Instance.StartHunger();
            GameMenu.Instance.ChangeScore(settingsAi.score * curMultiplayer);
            settingsAi.Die();

            if (collision.CompareTag("RestoreHP"))
            {
                player.MinusHp(false);
            }
        }
        else if (collision.CompareTag("Damage"))
        {
            TurnOffMultyplaing();
            SettingsAI settingsAi = collision.gameObject.GetComponent<SettingsAI>();
            GameMenu.Instance.ChangeScore(settingsAi.score * curMultiplayer);
            settingsAi.Die();
            player.MinusHp(true);
        }
    }
    IEnumerator ScoreMultiplayer()
    {
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(Settings.timeWhileScoreMultiplying));

        TurnOffMultyplaing();
    }

    public void TurnOffMultyplaing()
    {
        if (scoreMultiplayerCoroutine != null)
        {
            StopCoroutine(scoreMultiplayerCoroutine);
        }
        curMultiplayer = 1;
        isScoreMultiplaying = false;
        Settings.curTongueMultiplyer = Settings.tongueMultiplyer;
    }
}
