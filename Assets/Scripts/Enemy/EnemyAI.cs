using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Pathfinding;

public class EnemyAI : AIPath
{
    SettingsAI settingsAI;



    float minX;
    float minY;
    float maxX;
    float maxY;


    void Start()
    {
        settingsAI = GetComponent<SettingsAI>();
        settingsAI.point.transform.position = GetRandomPosition();


    }

    public void OnEndReached()
    {
        settingsAI.amountOfPointsToVisit--;

        settingsAI.point.transform.position = GetRandomPosition();
    }

    Vector2 GetRandomPosition()
    {
        if (settingsAI.amountOfPointsToVisit > 0)
        {
            minX = -Settings.ScreenWidth / 2f;
            maxX = Settings.ScreenWidth / 2f;
            maxY = Settings.ScreenHeight / 2f;
        }
        else
        {
            minX = -Settings.ScreenWidth;
            maxX = Settings.ScreenWidth;
            maxY = Settings.ScreenHeight;
        }
        minY = -Settings.ScreenHeight / 3f;

        float randomX = Random.Range(minX, maxX);
        float randomY = Random.Range(minY, maxY);

        return new Vector2(randomX, randomY);
    }

    void OnDrawGizmos()//Test
    {
        Gizmos.color = Color.red;
        Vector3 minPoint = new Vector3(minX, minY, transform.position.z);
        Vector3 maxPoint = new Vector3(maxX, maxY, transform.position.z);
        Gizmos.DrawLine(minPoint, new Vector3(minX, maxY, transform.position.z));
        Gizmos.DrawLine(minPoint, new Vector3(maxX, minY, transform.position.z));
        Gizmos.DrawLine(maxPoint, new Vector3(minX, maxY, transform.position.z));
        Gizmos.DrawLine(maxPoint, new Vector3(maxX, minY, transform.position.z));
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Edge"))
        {
            Destroy(gameObject);
        }
    }
}
