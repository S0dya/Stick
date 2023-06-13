using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonobehaviour<Player>
{
    LineRenderer tongueLine;
    Rigidbody2D rigidbody;
    BoxCollider2D tongueCollider;
    BoxCollider2D nearTongueCollider;
    GameObject tongueObject;
    Vector2 mousePos;

    Coroutine elongateCoroutine;
    Coroutine shortenCoroutine;
    Coroutine scoreMultiplayerCoroutine;

    [HideInInspector] public bool isSticked;
    [HideInInspector] public bool isOutOfTrigger;
    [HideInInspector] public bool isElongating;
    [HideInInspector] public bool isScoreMultiplaying;
    
    [HideInInspector] public float tongueLength;
    int health;
    bool canElongate;
    float curMultiplayer;

    protected override void Awake()
    {
        base.Awake();

        nearTongueCollider = GameObject.FindGameObjectWithTag("NearTongue").GetComponent<BoxCollider2D>();
        tongueObject = GameObject.FindGameObjectWithTag("Tongue");
        tongueLine = tongueObject.GetComponent<LineRenderer>();
        tongueCollider = tongueObject.GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();


        tongueCollider.enabled = false;
    }

    void Start()
    {
        health = 2;
        curMultiplayer = 1f;
    }

    void Update()
    {

        TouchInput();


    }

    void TouchInput()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.touches[i];

                if (touch.phase == TouchPhase.Began)
                {
                    Elongate();
                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    Shorten();
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Elongate();
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Shorten();
        }

    }

    void Elongate()
    {
        if (!canElongate)
        {
            return;
        }

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        canElongate = false;
        isElongating = true;
        tongueCollider.enabled = true;
        nearTongueCollider.enabled = true;

        elongateCoroutine = StartCoroutine(ElongateTongue());
    }
    IEnumerator ElongateTongue()
    {
        Vector3 direction = mousePos - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        while (!isOutOfTrigger && !isSticked)
        {
            tongueLine.SetPosition(0, new Vector3(tongueLength, 0f, 0f));

            tongueCollider.offset = new Vector2(tongueLength /2 -0.01f, 0f);
            tongueCollider.size = new Vector2(tongueLength - 0.01f, 0.14f);
            nearTongueCollider.offset = new Vector2(tongueLength /2 -0.01f, 0f);
            nearTongueCollider.size = new Vector2(tongueLength -0.01f, 0.14f);

            Tongue.Instance.StickingPart.transform.position = transform.TransformPoint(tongueLine.GetPosition(0));

            tongueLength += Settings.tongueMultiplyer * Time.deltaTime;
            yield return null;
        }

        Shorten();
    }


    void Shorten()
    {
        if (elongateCoroutine != null)
        {
            StopCoroutine(elongateCoroutine);
        }

        isOutOfTrigger = false;
        isSticked = false;
        isElongating = false;

        tongueCollider.enabled = false;
        nearTongueCollider.enabled = false;

        shortenCoroutine = StartCoroutine(ShortenTongue());
    }
    IEnumerator ShortenTongue()
    {
        while (tongueLength > -0.1)
        {
            tongueLine.SetPosition(0, new Vector3(tongueLength, 0f, 0f));

            Tongue.Instance.StickingPart.transform.position = transform.TransformPoint(tongueLine.GetPosition(0));

            tongueLength -= (Settings.tongueMultiplyer * 1.3f) * Time.deltaTime;
            yield return null;
        }

        canElongate = true;
    }

    public void MinusHp(bool minus)
    {
        if (minus)
        {
            //check if < 0 if needed 
            HPBar.Instance.SetHPImage(health, false);

            health--;

            if (health < 0)
            {
                GameManager.Instance.GameOver();
            }
        }
        else
        {
            if (health < Settings.maxHealth)
            {
                health++;

                HPBar.Instance.SetHPImage(health, true);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            if (scoreMultiplayerCoroutine != null)
            {
                curMultiplayer += Settings.scoreMultiplyer;
                StopCoroutine(scoreMultiplayerCoroutine);
            }
            scoreMultiplayerCoroutine = StartCoroutine(ScoreMultiplayer());
            SettingsAI settingsAi = collision.gameObject.GetComponent<SettingsAI>();
            GameMenu.Instance.ChangeScore(settingsAi.score * curMultiplayer);
            settingsAi.Die();
            isScoreMultiplaying = true;
        }
    }
    IEnumerator ScoreMultiplayer()
    {
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(Settings.timeWhileScoreMultiplying));

        curMultiplayer = 1f;
        isScoreMultiplaying = false;
    }

}