using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonobehaviour<Player>
{
    [HideInInspector] public LineRenderer tongueLine;
    Rigidbody2D rigidbody;
    [HideInInspector] public BoxCollider2D tongueCollider;
    [HideInInspector] public BoxCollider2D nearTongueCollider;
    GameObject tongueObject;
    Vector2 mousePos;
    Camera camera;

    Coroutine elongateCoroutine;
    Coroutine shortenCoroutine;

    [HideInInspector] public bool isSticked;
    [HideInInspector] public bool isOutOfTrigger;
    [HideInInspector] public bool canElongate = true;

    [HideInInspector] public float tongueLength;
    [HideInInspector] public int health;

    protected override void Awake()
    {
        base.Awake();

        nearTongueCollider = GameObject.FindGameObjectWithTag("NearTongue").GetComponent<BoxCollider2D>();
        tongueObject = GameObject.FindGameObjectWithTag("Tongue");
        tongueLine = tongueObject.GetComponent<LineRenderer>();
        tongueCollider = tongueObject.GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();
        camera = Camera.main;

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
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    Shorten();
                }
            }
        }


        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            if (Input.mousePosition.y / Settings.heightForInput > Settings.blindZoneOfY)
            {
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
            tongueLine.SetPosition(0, new Vector3(tongueLength, 0f, 0f));

            tongueCollider.offset = new Vector2(tongueLength /2 -0.01f, 0f);
            tongueCollider.size = new Vector2(tongueLength - 0.01f, 0.2f);
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
        tongueCollider.enabled = false;
        if (elongateCoroutine != null)
        {
            StopCoroutine(elongateCoroutine);
        }

        nearTongueCollider.offset = new Vector2(0 / 2 - 0.01f, 0f);
        nearTongueCollider.size = new Vector2(0 , 0.14f);

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

    void OnDisable()
    {
        StopAllCoroutines();
    }
}