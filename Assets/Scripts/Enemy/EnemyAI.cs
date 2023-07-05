using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    SettingsAI settingsAI;
    Rigidbody2D rigidbody;

    float randomX;
    float randomY;
    Vector2 target;

    Coroutine changingSpeedCoroutine;

    void Awake()
    {
        settingsAI = GetComponent<SettingsAI>();
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GetRandomPosition();
        StartCoroutine(MoveToTarget());
    }

    public IEnumerator MoveToTarget()
    {
        while (true)
        {
            Vector2 direction = target - rigidbody.position;
            float distance = direction.magnitude;

            if (distance > 0.2f)
            {
                direction.Normalize();

                float dotValue = Vector2.Dot(transform.up, direction);
                float targetRotation = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
                rigidbody.MoveRotation(Mathf.LerpAngle(rigidbody.rotation, targetRotation, settingsAI.rotationSpeed * Time.deltaTime));

                rigidbody.velocity = transform.up * settingsAI.speed * Mathf.Max(dotValue, 0.1f);
            }
            else
            {
                rigidbody.velocity = Vector2.zero;
                StartCoroutine(WaitBeforNewTarget());
                break;
            }
            
            yield return null;
        }
    }

    IEnumerator WaitBeforNewTarget()
    {
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(settingsAI.timeForTakingAnotherPoint));

        settingsAI.amountOfPointsToVisit--;
        GetRandomPosition();
        StartCoroutine(MoveToTarget());
    }

    void GetRandomPosition()
    {
        if (settingsAI.amountOfPointsToVisit > 0)
        {
            randomX = Random.Range(Settings.minX, Settings.maxX);
            randomY = Random.Range(Settings.minY, Settings.maxY);
        }
        else
        {
            randomX = Random.Range(0, 2) == 0 ? -Settings.ScreenWidth: Settings.ScreenWidth;
            randomY = Random.Range(Settings.minY * 1.2f, Settings.ScreenHeight * 1.5f);
        }

        target = new Vector2(randomX, randomY);
    }

    public void StopMoving()
    {
        rigidbody.velocity = Vector2.zero;
        StopAllCoroutines();
    }

    public void StartMoving()
    {
        StartCoroutine(MoveToTarget());
    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("NearTongue"))
        {

            if (changingSpeedCoroutine != null)
            {
                StopCoroutine(changingSpeedCoroutine);
            }
            changingSpeedCoroutine = StartCoroutine(ChangeSpeed());
        }
    }
    IEnumerator ChangeSpeed()
    { 
        float curRotationSpeed = settingsAI.rotationSpeed;
        settingsAI.speed = settingsAI.speedOnNearTheTongue;
        settingsAI.rotationSpeed = curRotationSpeed * 1.5f;

        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(settingsAI.timeForChangingSpeed));

        settingsAI.speed = settingsAI.defaultSpeed;
        settingsAI.rotationSpeed = curRotationSpeed;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Edges") && !settingsAI.isDestroying)
        {
            settingsAI.Die();
        }
    }

}
