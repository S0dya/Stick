using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsAI : MonoBehaviour
{
    EnemyAI enemyAi;

    //point
    public GameObject point;
    public int amountOfPointsToVisit;

    //coroutine
    public float timeForTakingAnotherPoint;
    public float timeForChangingSpeed;

    //ai
    public float defaultSpeed;
    public float speedOnNearTheTongue;

    //temp
    float currentSpeed;


    public void DisableMovement()
    {
        currentSpeed = enemyAi.maxSpeed;
        enemyAi.maxSpeed = 0;
    }
    public void EnableMovement()
    {
        enemyAi.maxSpeed = currentSpeed;
    }
}
