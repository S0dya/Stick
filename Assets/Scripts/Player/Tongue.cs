using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : SingletonMonobehaviour<Tongue>
{
    Player player;
    public GameObject stickingPartObject;
    StickingPart stickingPart;
    LineRenderer tongue;

    [SerializeField] ParticleSystem catchedEffect;

    Transform ComboTextParent;
    [SerializeField] GameObject x2MultiplayerPrefab;
    [SerializeField] GameObject x3MultiplayerPrefab;
    [SerializeField] GameObject x4MultiplayerPrefab;
    [SerializeField] GameObject x5MultiplayerPrefab;

    Coroutine scoreMultiplayerCoroutine;

    [HideInInspector] public bool isScoreMultiplaying;
    [HideInInspector] public float curMultiplayer;


    protected override void Awake()
    {
        base.Awake();

        player = GetComponentInParent<Player>();
        stickingPart = stickingPartObject.GetComponent<StickingPart>();
        tongue = GetComponent<LineRenderer>();

        ComboTextParent = GameObject.FindGameObjectWithTag("ComboTextParentTransform").transform;
    }

    void Start()
    {
        curMultiplayer = 1f;
        isScoreMultiplaying = false;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
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
                    AudioManager.Instance.PlayOneShot(FMODManager.Instance.FlySound);
                }
                else
                {
                    AudioManager.Instance.PlayOneShot(FMODManager.Instance.FireFlySound);
                }

                Instantiate(catchedEffect, collision.transform.position, Quaternion.identity);

                if (isScoreMultiplaying)
                {
                    GameObject multiplayerObj = new GameObject();

                    switch (curMultiplayer)
                    {
                        case 1.5f:
                            multiplayerObj = Instantiate(x2MultiplayerPrefab, ComboTextParent);
                            AudioManager.Instance.PlayOneShot(FMODManager.Instance.CatchSounds[0]);
                            break;
                        case 2f:
                            multiplayerObj = Instantiate(x3MultiplayerPrefab, ComboTextParent);
                            AudioManager.Instance.PlayOneShot(FMODManager.Instance.CatchSounds[1]);
                            break;
                        case 2.5f:
                            multiplayerObj = Instantiate(x4MultiplayerPrefab, ComboTextParent);
                            AudioManager.Instance.PlayOneShot(FMODManager.Instance.CatchSounds[2]);
                            break;
                        case 3f:
                            multiplayerObj = Instantiate(x5MultiplayerPrefab, ComboTextParent);
                            AudioManager.Instance.PlayOneShot(FMODManager.Instance.CatchSounds[3]);
                            break;
                        default:
                            break;
                    }

                    multiplayerObj.transform.position = Camera.main.WorldToScreenPoint(collision.transform.position);
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
                
                HPBar.Instance.StopHunger();
                HPBar.Instance.StartHunger();
                GameMenu.Instance.ChangeScore(settingsAi.score * curMultiplayer);
            }
            else
            {
                GameMenu.Instance.ChangeScore(settingsAi.score * curMultiplayer);
                TurnOffMultyplaing();
                AudioManager.Instance.PlayOneShot(FMODManager.Instance.BeeSound);
            }

            EnemyAI enemyAi = collision.gameObject.GetComponent<EnemyAI>();
            enemyAi.canMove = false;

            collision.transform.SetParent(stickingPartObject.transform);
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
    
    
    public void SetColor(Color startColor, Color endColor)
    {
        tongue.startColor = startColor;
        tongue.endColor = endColor;
    }
}
