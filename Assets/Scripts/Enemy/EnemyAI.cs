using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class EnemyAI : AIPath
{
    SettingsAI settingsAI;
    AIDestinationSetter destinationSetter;

    float randomX;
    float randomY;

    Coroutine changingSpeedCoroutine;

    void Awake()
    {
        settingsAI = GetComponent<SettingsAI>();
        destinationSetter = GetComponent<AIDestinationSetter>();
    }

    void Start()
    {
        maxSpeed = settingsAI.defaultSpeed;

        GetRandomPosition();
        destinationSetter.target = settingsAI.point.transform;
    }

    public void OnEndReached()
    {
        settingsAI.amountOfPointsToVisit--;

        StartCoroutine(TakeAnotherPoint());

    }
    IEnumerator TakeAnotherPoint()
    {
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(settingsAI.timeForTakingAnotherPoint));

        GetRandomPosition();
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

        settingsAI.point.transform.position = new Vector2(randomX, randomY);
    }

    void OnDrawGizmos()//Test del later
    {
        Gizmos.color = Color.red;
        Vector3 minPoint = new Vector3(Settings.minX, Settings.minY, transform.position.z);
        Vector3 maxPoint = new Vector3(Settings.maxX, Settings.maxY, transform.position.z);
        Gizmos.DrawLine(minPoint, new Vector3(Settings.minX, Settings.maxY, transform.position.z));
        Gizmos.DrawLine(minPoint, new Vector3(Settings.maxX, Settings.minY, transform.position.z));
        Gizmos.DrawLine(maxPoint, new Vector3(Settings.minX, Settings.maxY, transform.position.z));
        Gizmos.DrawLine(maxPoint, new Vector3(Settings.maxX, Settings.minY, transform.position.z));
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
        maxSpeed = settingsAI.speedOnNearTheTongue; //change Later

        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(settingsAI.timeForChangingSpeed));

        maxSpeed = settingsAI.defaultSpeed;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Edges") && !settingsAI.isDestroying)
        {
            settingsAI.Die();
        }
    }

}
