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
    public int enemyType;
    public float defaultSpeed;
    public float speedOnNearTheTongue;

    //temp
    float currentSpeed;
    Coroutine SpawnCoroutine;
    [HideInInspector] public bool isDestroying;

    void Awake()
    {
        enemyAi = GetComponent<EnemyAI>();
    }

    void Start()
    {
        GameManager.Instance.enemySettingsAIList.Add(this);
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
        GameManager.Instance.curEnemies[enemyType]--;
        SpawnCoroutine = GameManager.Instance.StartCoroutine(GameManager.Instance.Spawn());
        GameManager.Instance.enemySettingsAIList.Remove(this);
        Destroy(gameObject);
    }

    public void Clear()
    {
        GameManager.Instance.curEnemies[enemyType]--;
        isDestroying = true;
        GameManager.Instance.enemySettingsAIList.Remove(this);
        Destroy(gameObject);
    }

    
}
