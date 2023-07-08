using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : SingletonMonobehaviour<Player>
{
    [SerializeField] GameObject tongueObject;
    [HideInInspector] public LineRenderer tongueLine;
    Light2D lightForStickingPart;

    Vector2 mousePos;
    Camera camera;
    Rigidbody2D rigidbody;
    Rigidbody2D rigidbodyOfStickingObject;
    BoxCollider2D stickingPartCollider;

    Coroutine elongateCoroutine;
    Coroutine shortenCoroutine;

    public GameObject stickingPartObject;

    [HideInInspector] public bool isSticked;
    [HideInInspector] public bool isOutOfTrigger;
    [HideInInspector] public bool canElongate = true;

    [HideInInspector] public float tongueLength;
    [HideInInspector] public int health;

    [SerializeField] SpriteRenderer background;
    SpriteRenderer playerSkin;

    bool touchEnded;

    protected override void Awake()
    {
        base.Awake();

        playerSkin = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbodyOfStickingObject = stickingPartObject.GetComponent<Rigidbody2D>();
        stickingPartCollider = stickingPartObject.GetComponent<BoxCollider2D>();
        lightForStickingPart = stickingPartObject.GetComponentInChildren<Light2D>();
        camera = Camera.main;

        tongueLine = tongueObject.GetComponent<LineRenderer>();

        stickingPartCollider.enabled = false;
    }

    void Start()
    {
        Shorten();
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
                    touchEnded = false;
                    mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
                    if (touch.position.y / Settings.heightForInput > Settings.blindZoneOfY)
                    {
                        Elongate();
                    }
                }
                else if (touch.phase == TouchPhase.Ended)
                {
                    touchEnded = true;
                }
            }
        }
    }

    void Elongate()
    {
        if (!canElongate)
        {
            return;
        }

        canElongate = false;
        stickingPartCollider.enabled = true;

        elongateCoroutine = StartCoroutine(ElongateTongue());
    }
    IEnumerator ElongateTongue()
    {
        Vector2 direction = mousePos - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        while (!isOutOfTrigger && !isSticked && !touchEnded)
        {
            if (GameManager.Instance.isGameMenuOpen)
            {
                yield return null;
            }

            float lengthForStickingObject = tongueLength * 0.7f;
            Vector3 position = new Vector3(lengthForStickingObject, lengthForStickingObject, 0) * direction;
            tongueLine.SetPosition(0, new Vector2(tongueLength, 0));
            rigidbodyOfStickingObject.MovePosition(transform.position + position);
            tongueLength += Settings.tongueMultiplyer * Time.deltaTime;

            yield return null;
        }

        Shorten();
    }


    void Shorten()
    {
        stickingPartCollider.enabled = false;
        if (elongateCoroutine != null)
        {
            StopCoroutine(elongateCoroutine);
        }

        shortenCoroutine = StartCoroutine(ShortenTongue());
    }
    IEnumerator ShortenTongue()
    {
        Vector2 direction = (Vector2)transform.position - (Vector2)stickingPartObject.transform.position;
        direction.Normalize();
        float distance = direction.magnitude;
        
        while (tongueLength > 0.1f)
        {
            if (GameManager.Instance.isGameMenuOpen)
            {
                yield return null;
            }

            tongueLength -= Settings.tongueMultiplyer * 1.2f * Time.deltaTime;
            float lengthForStickingObject = tongueLength * 0.7f;
            Vector3 position = new Vector3(lengthForStickingObject, lengthForStickingObject, 0) * direction;
            tongueLine.SetPosition(0, new Vector2(tongueLength, 0));
            rigidbodyOfStickingObject.MovePosition(transform.position - position);

            yield return null;
        }

        isOutOfTrigger = false;
        isSticked = false;
        canElongate = true;
    }


    public void MinusHp(bool minus)
    {
        HPBar.Instance.StopHunger();
        if (minus)
        {
            if (health >= 0)
            {
                HPBar.Instance.SetHPImage(health, false);

                health--;

                if (health < 0)
                {
                    GameMenu.Instance.GameOver();
                }
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
        HPBar.Instance.StartHunger();
    }

    public void SetSkin(int i)
    {
        playerSkin.sprite = GameManager.Instance.GekoSkins[i];
        SetColor(GameManager.Instance.tongueColorsStart[i], GameManager.Instance.tongueColorsEnd[i]);
    }
    public void SetBackground(int i)
    {
        background.sprite = GameManager.Instance.backgrounds[i];
        
        float screenHeight = camera.orthographicSize * 1.58f;
        background.transform.localScale = new Vector3(screenHeight * camera.aspect * 1.98f, screenHeight, 0);
    }

    public void SetColor(Color startColor, Color endColor)
    {
        tongueLine.startColor = startColor;
        tongueLine.endColor = endColor;

        lightForStickingPart.color = endColor;
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }
}