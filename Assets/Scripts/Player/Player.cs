using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Player : SingletonMonobehaviour<Player>
{
    [SerializeField] GameObject tongueObject;
    [HideInInspector] public LineRenderer tongueLine;
    Light2D lightForStickingPart;
    [HideInInspector] public BoxCollider2D tongueCollider;

    public BoxCollider2D nearTongueCollider;
    
    Vector2 mousePos;
    Camera camera;
    Rigidbody2D rigidbody;
    Rigidbody2D rigidbodyOfStickingObject;

    Coroutine elongateCoroutine;
    Coroutine shortenCoroutine;

    [SerializeField] GameObject stickingPartObject;

    [HideInInspector] public bool isSticked;
    [HideInInspector] public bool isOutOfTrigger;
    [HideInInspector] public bool canElongate = true;

    [HideInInspector] public float tongueLength;
    [HideInInspector] public int health;

    [SerializeField] SpriteRenderer background;
    SpriteRenderer playerSkin;

    protected override void Awake()
    {
        base.Awake();

        tongueLine = tongueObject.GetComponent<LineRenderer>();
        lightForStickingPart = tongueObject.GetComponent<Light2D>();
        tongueCollider = tongueObject.GetComponent<BoxCollider2D>();

        playerSkin = GetComponent<SpriteRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbodyOfStickingObject = stickingPartObject.GetComponent<Rigidbody2D>();
        camera = Camera.main;

        background.transform.localScale = new Vector3(Settings.ScreenWidth, Settings.ScreenHeight * 0.8f, 0);
        tongueCollider.enabled = false;
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
                    mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
                    if (touch.position.y / Settings.heightForInput > Settings.blindZoneOfY)
                    {
                        Elongate();
                    }
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Input.mousePosition.y / Settings.heightForInput > Settings.blindZoneOfY)
            {
                mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
                Elongate();
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

        while (!isOutOfTrigger && !isSticked && !Input.GetKeyUp(KeyCode.Mouse0))
        {
            if (GameManager.Instance.isGameMenuOpen)
            {
                yield return null;
            }

            rigidbodyOfStickingObject.MovePosition(rigidbodyOfStickingObject.position + direction * Settings.tongueLengthenSpeed * Time.deltaTime);

            yield return null;
        }

        Shorten();
    }


    void Shorten()
    {
        tongueCollider.enabled = false;
        nearTongueCollider.enabled = false;
        if (elongateCoroutine != null)
        {
            StopCoroutine(elongateCoroutine);
        }

        shortenCoroutine = StartCoroutine(ShortenTongue());
    }
    IEnumerator ShortenTongue()
    {
        Vector2 direction = transform.position - rigidbodyOfStickingObject.position;
        float distance = direction.magnitude;

        while (distance > 0.1)
        {
            rigidbodyOfStickingObject.velocity = transform.up * -Settings.tongueMultiplyer * Time.deltaTime;

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