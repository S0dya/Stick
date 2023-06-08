using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonobehaviour<Player>
{
    [SerializeField] GameObject snailObject;

    LineRenderer snailLine;
    Rigidbody2D rigidbody;
    BoxCollider2D snailCollider;


    public float maxSnailLength;

    [SerializeField] float snailMultiplyer; //ToSettingsLater
    //[HideInInspector]
    public float snailLength;
    float colliderLength;

    Coroutine elongateCoroutine;
    Coroutine shortenCoroutine;

    Vector2 mousePos;

    [HideInInspector] public bool isSticked;
    [HideInInspector] public bool isPushing;
    [HideInInspector] public bool isElongating;

    protected override void Awake()
    {
        base.Awake();

        snailLine = snailObject.GetComponent<LineRenderer>();
        snailCollider = snailObject.GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();


        snailCollider.enabled = false;
    }

    void Start()
    {
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
        if (shortenCoroutine != null)
        {
            StopCoroutine(shortenCoroutine);
        }

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        isElongating = true;
        snailCollider.enabled = true;

        elongateCoroutine = StartCoroutine(ElongateSnail());
    }
    IEnumerator ElongateSnail()
    {
        Vector3 direction = mousePos - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        while (snailLength < maxSnailLength && !isSticked)
        {
            snailLine.SetPosition(0, new Vector3(snailLength, 0f ,0f));
            snailCollider.offset = new Vector2(snailLength /2 -0.01f, 0f);
            snailCollider.size = new Vector2(snailLength - 0.01f, 0.14f);

            snailLength += snailMultiplyer * Time.deltaTime;
            yield return null;
        }

        if (!isSticked)
        {
            Shorten();
        }
    }


    void Shorten()
    {
        if (elongateCoroutine != null)
        {
            StopCoroutine(elongateCoroutine);
        }
        if (isSticked)
        {
            Snail.Instance.UnStick();
        }

        isPushing = false;
        isElongating = false;

        snailCollider.enabled = false;

        shortenCoroutine = StartCoroutine(ShortenSnail());
    }
    IEnumerator ShortenSnail()
    {
        while (snailLength > -0.1)
        {
            snailLine.SetPosition(0, new Vector3(snailLength, 0f, 0f));

            snailLength -= (snailMultiplyer + snailMultiplyer / 3) * Time.deltaTime;
            yield return null;
        }

    }


}