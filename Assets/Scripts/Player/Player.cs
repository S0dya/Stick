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
    float snailLength;
    float colliderLength;

    Coroutine elongateCoroutine;
    Coroutine shortenCoroutine;

    [HideInInspector] public bool isSticked;
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


        if (Input.GetKeyDown(KeyCode.Space))
        {
            Elongate();
        }
        else if (Input.GetKeyUp(KeyCode.Space))
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

        isElongating = true;
        snailCollider.enabled = true;

        elongateCoroutine = StartCoroutine(ElongateSnail());
    }
    IEnumerator ElongateSnail()
    {
        while (snailLength < maxSnailLength && !isSticked)
        {
            Vector2 forceDirection = transform.TransformDirection(Vector2.right);
            rigidbody.AddForce(forceDirection * 0.1f, ForceMode2D.Impulse);

            snailLine.SetPosition(0, new Vector3(snailLength, 0f ,0f));
            snailCollider.offset = new Vector2(snailLength /2, 0f);
            snailCollider.size = new Vector2(snailLength, 0.01f);

            snailLength += snailMultiplyer * Time.deltaTime;
            yield return null;
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