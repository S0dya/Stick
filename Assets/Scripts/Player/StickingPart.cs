using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickingPart : SingletonMonobehaviour<StickingPart>
{
    Player player;

    [SerializeField] ParticleSystem catchedEffect;
    [SerializeField] Transform ComboTextParent;
    [SerializeField] GameObject[] MultiplayerPrefab;

    [HideInInspector] public bool isScoreMultiplaying;
    float curMultiplayer = 1f;

    Coroutine scoreMultiplayerCoroutine;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponentInParent<Player>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EdgesForTongue"))
        {
            Player.Instance.isOutOfTrigger = false;
        }

        bool isDamage = collision.CompareTag("Damage");
        bool isFood = collision.CompareTag("Food");
        bool isRestoreHp = collision.CompareTag("RestoreHP");

        if (player.isSticked)
        {
            return;
        }
        else if (isDamage || isFood || isRestoreHp)
        {
            SettingsAI settingsAi = collision.gameObject.GetComponent<SettingsAI>();
            if (settingsAi.isDestroying)
                return;

            player.isSticked = true;

            if ((isFood || isRestoreHp))
            {
                if (isFood)
                {
                    AudioManager.Instance.PlayOneShot("FlySound");
                }
                else
                {
                    AudioManager.Instance.PlayOneShot("FireFlySound");
                }

                Instantiate(catchedEffect, collision.transform.position, Quaternion.identity);

                if (isScoreMultiplaying)
                {
                    switch (curMultiplayer)
                    {
                        case 1.5f:
                            Instantiate(MultiplayerPrefab[0], Camera.main.WorldToScreenPoint(collision.transform.position), Quaternion.identity, ComboTextParent);
                            AudioManager.Instance.PlayOneShot("CatchSounds0");
                            break;
                        case 2f:
                            Instantiate(MultiplayerPrefab[1], Camera.main.WorldToScreenPoint(collision.transform.position), Quaternion.identity, ComboTextParent);
                            AudioManager.Instance.PlayOneShot("CatchSounds1");
                            break;
                        case 2.5f:
                            Instantiate(MultiplayerPrefab[2], Camera.main.WorldToScreenPoint(collision.transform.position), Quaternion.identity, ComboTextParent);
                            AudioManager.Instance.PlayOneShot("CatchSounds2");
                            break;
                        case 3f:
                            Instantiate(MultiplayerPrefab[3], Camera.main.WorldToScreenPoint(collision.transform.position), Quaternion.identity, ComboTextParent);
                            AudioManager.Instance.PlayOneShot("CatchSounds3");
                            break;
                        default:
                            break;
                    }
                }

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

                HPBar.Instance.RestartHunger();
                GameMenu.Instance.ChangeScore(settingsAi.score * curMultiplayer);
            }
            else
            {
                GameMenu.Instance.ChangeScore(settingsAi.score * curMultiplayer);
                TurnOffMultyplaing();
                AudioManager.Instance.PlayOneShot("BeeSound");
            }

            EnemyAI enemyAi = collision.gameObject.GetComponent<EnemyAI>();
            enemyAi.StartCoroutine(enemyAi.Attach());
        }
    }

    IEnumerator ScoreMultiplayer()
    {
        yield return new WaitForSeconds(Settings.timeWhileScoreMultiplying);

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


    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("EdgesForTongue"))
        {
            Player.Instance.isOutOfTrigger = true;
        }
    }
}
