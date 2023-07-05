using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SettingsAI : MonoBehaviour
{
    EnemyAI enemyAi;
    Light2D light;

    //point
    [HideInInspector] public int amountOfPointsToVisit;
    public int minAOPTV;
    public int maxAOPTV;

    //coroutine
    public float timeForTakingAnotherPoint;
    public float timeForChangingSpeed;

    //UI
    public float score;

    //ai
    [HideInInspector] public float speed;
    public float rotationSpeed;
    public float defaultSpeed;
    public int enemyType;
    public float speedOnNearTheTongue;

    //temp
    Vector2 curTarget;
    float currentSpeed;
    Coroutine SpawnCoroutine;
    [HideInInspector] public bool isDestroying;

    void Awake()
    {
        enemyAi = GetComponent<EnemyAI>();
        light = GetComponentInChildren<Light2D>();
        speed = defaultSpeed;
        ToggleLight(GameMenu.Instance.isCurNight);
    }

    void Start()
    {
        GameManager.Instance.enemySettingsAIList.Add(this);

        amountOfPointsToVisit = Random.Range(minAOPTV, maxAOPTV);
    }

    public void DisableMovement()
    {
        enemyAi.StopMoving();
    }
    public void EnableMovement()
    {
        enemyAi.StartMoving();
    }

    public void ToggleLight(bool val)
    {
        light.enabled = val;
    }

    public void Die()
    {
        SpawnCoroutine = GameManager.Instance.StartCoroutine(GameManager.Instance.Spawn());
        Clear();
    }

    public void Clear()
    {
        isDestroying = true;
        GameManager.Instance.enemySettingsAIList.Remove(this);
        Destroy(gameObject);
    }
}
