using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : SingletonMonobehaviour<Player>
{
    [SerializeField] BoxCollider2D collider;

    LineRenderer snail;
    Rigidbody2D rigidbody;


    [SerializeField] float snailMultiplyer; //ToSettingsLater
    float snailLength;
    float colliderLength;
    public bool isSticked;

    Coroutine elongateCoroutine;
    Coroutine shortenCoroutine;


    protected override void Awake()
    {
        base.Awake();

        snail = GetComponentInChildren<LineRenderer>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        snail.SetPosition(0, new Vector3(0, 0f, 0f));
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
                    if (shortenCoroutine != null)
                    {
                        StopCoroutine(shortenCoroutine);
                    }
                    elongateCoroutine = StartCoroutine(ElongateSnail());

                }
                else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    if (elongateCoroutine != null)
                    {
                        StopCoroutine(elongateCoroutine);
                    }
                    shortenCoroutine = StartCoroutine(ShortenSnail());


                }
            }
        }
    }
    IEnumerator ElongateSnail()
    {
        
        while (snailLength < 6 && !isSticked)
        {
            Vector2 forceDirection = transform.TransformDirection(Vector2.right);
            rigidbody.AddForce(forceDirection * 0.1f, ForceMode2D.Impulse);
            snail.SetPosition(0, new Vector3(snailLength, 0f, 0f));

            snailLength += snailMultiplyer * Time.deltaTime;
            collider.offset= new Vector2(snailLength, 0f);
            collider.size = new Vector2(snailLength /2, 0.14f);
            yield return null;
        }

    }

    IEnumerator ShortenSnail()
    {
        isSticked = false;

        while (snailLength > 0)
        {
            snail.SetPosition(0, new Vector3(snailLength, 0f, 0f));

            snailLength -= (snailMultiplyer + snailMultiplyer / 3) * Time.deltaTime;
            collider.offset = new Vector2(snailLength, 0f);
            collider.size = new Vector2(snailLength / 2, 0.14f);
            yield return null;
        }

    }

    

    public void OnSticked()
    {

    }
}