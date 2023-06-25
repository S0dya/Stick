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
        curMultiplayer = 1f;

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
                curMultiplayer += Settings.scoreMultiplyer;
                StopCoroutine(scoreMultiplayerCoroutine);
            }

            HPBar.Instance.StopHunger();
            HPBar.Instance.StartHunger();

            scoreMultiplayerCoroutine = StartCoroutine(ScoreMultiplayer());
            GameMenu.Instance.ChangeScore(settingsAi.score * curMultiplayer);
            settingsAi.Die();
            isScoreMultiplaying = true;

            if (collision.CompareTag("RestoreHP"))
            {
                player.MinusHp(false);
            }
        }
        else if (collision.CompareTag("Damage"))
        {
            if (scoreMultiplayerCoroutine != null)
            {
                StopCoroutine(scoreMultiplayerCoroutine);
            }
            SettingsAI settingsAi = collision.gameObject.GetComponent<SettingsAI>();
            GameMenu.Instance.ChangeScore(settingsAi.score * curMultiplayer);
            settingsAi.Die();
            player.MinusHp(true);
        }
    }
    IEnumerator ScoreMultiplayer()
    {
        if (curMultiplayer < 3)
        {
            curMultiplayer += 0.5f;
        }
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(Settings.timeWhileScoreMultiplying));

        curMultiplayer = 1;
        isScoreMultiplaying = false;
    }
}
