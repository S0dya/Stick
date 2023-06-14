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

    //UI
    public float score;

    //ai
    public float defaultSpeed;
    public float speedOnNearTheTongue;

    //temp
    float currentSpeed;
    Coroutine SpawnCoroutine;

    void Awake()
    {
        enemyAi = GetComponent<EnemyAI>();
    }

    public void DisableMovement()
    {
        currentSpeed = enemyAi.maxSpeed;
        enemyAi.canMove= false;
    }
    public void EnableMovement()
    {
        enemyAi.canMove = true;
    }

    public void Die()
    {
        SpawnCoroutine = GameManager.Instance.StartCoroutine(GameManager.Instance.Spawn());
        Destroy(gameObject);
    }

    public void Clear()
    {
        Destroy(gameObject);
    }
}
