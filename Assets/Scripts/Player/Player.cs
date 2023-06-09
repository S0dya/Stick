using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonobehaviour<Player>
{
    [SerializeField] GameObject snailObject;

    [HideInInspector] public LineRenderer tongueLine;
    Rigidbody2D rigidbody;
    BoxCollider2D tongueCollider;

    public float maxSnailLength;

    [SerializeField] float snailMultiplyer; //ToSettingsLater
    //[HideInInspector]
    public float snailLength;
    float colliderLength;

    Coroutine elongateCoroutine;
    Coroutine shortenCoroutine;

    Vector2 mousePos;

    [HideInInspector] public bool isSticked;
    [HideInInspector] public bool isOnTrigger;
    [HideInInspector] public bool isElongating;

    protected override void Awake()
    {
        base.Awake();

        tongueLine = snailObject.GetComponent<LineRenderer>();
        tongueCollider = snailObject.GetComponent<BoxCollider2D>();
        rigidbody = GetComponent<Rigidbody2D>();

        tongueCollider.enabled = false;
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
        tongueCollider.enabled = true;

        elongateCoroutine = StartCoroutine(ElongateTongue());
    }
    IEnumerator ElongateTongue()
    {
        Vector3 direction = mousePos - (Vector2)transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        while (!isOnTrigger && !isSticked)
        {
            tongueLine.SetPosition(0, new Vector3(snailLength, 0f ,0f));
            tongueCollider.offset = new Vector2(snailLength /2 -0.01f, 0f);
            tongueCollider.size = new Vector2(snailLength - 0.01f, 0.14f);

            Tongue.Instance.StickingPart.transform.position = transform.TransformPoint(tongueLine.GetPosition(0));

            snailLength += snailMultiplyer * Time.deltaTime;
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

        isOnTrigger = false;
        isSticked = false;
        isElongating = false;

        tongueCollider.enabled = false;

        shortenCoroutine = StartCoroutine(ShortenTongue());
    }
    IEnumerator ShortenTongue()
    {
        while (snailLength > -0.1)
        {
            tongueLine.SetPosition(0, new Vector3(snailLength, 0f, 0f));

            Tongue.Instance.StickingPart.transform.position = transform.TransformPoint(tongueLine.GetPosition(0));

            snailLength -= (snailMultiplyer + snailMultiplyer / 3) * Time.deltaTime;
            yield return null;
        }

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {


            GameObject.Destroy(collision.gameObject);
        }
    }

}