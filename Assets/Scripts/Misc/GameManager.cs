using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    BoxCollider2D edgesCollider;

    //Gameplay
    Transform enemyParent;
    Transform pointParent;

    [SerializeField] GameObject flyPrefab;
    [SerializeField] GameObject beePrefab;
    [SerializeField] GameObject butterflyPrefab;

    [SerializeField] GameObject pointPrefab;

    [SerializeField] float maxEnemies;
    [SerializeField] float maxFlies;
    [SerializeField] float maxBees;
    [SerializeField] float maxButterflies;

    SettingsAI[] enemySettingsAIArray;

    bool isMenuOpen;

    //Logic

    protected override void Awake()
    {
        base.Awake();
        Settings.Initialize();
        AstarPath.active.Scan();

        enemyParent = GameObject.FindGameObjectWithTag("EnemyParentTransform").transform;
        pointParent = GameObject.FindGameObjectWithTag("PointParentTransform").transform;

        edgesCollider = GameObject.FindGameObjectWithTag("Edges").GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        DefineEdgesOfScreen();

        for (int i = 0; i < 15; i++)
        {
            StartCoroutine(Spawn());
        }

    }

    void DefineEdgesOfScreen()
    {
        edgesCollider.size = new Vector2(Settings.ScreenWidth * 1.6f, Settings.ScreenHeight * 1.6f);
    }

    public IEnumerator Spawn()
    {
        yield return StartCoroutine(Timer(Random.Range(0.1f, 1.5f)));

        float randomX = Random.Range(0, 2) == 0 ? -Settings.ScreenWidth * 0.7f : Settings.ScreenWidth * 0.7f;
        float randomY = Random.Range(0f, Settings.maxY);
        Vector3 spawnPosition = new Vector3(randomX, randomY, 1f);
        GameObject pointGameObject = Instantiate(pointPrefab, spawnPosition, Quaternion.identity, pointParent);
        SettingsAI settingsAI = new SettingsAI();

        int randomVal = Random.Range(0, 2);
        switch (randomVal)
        {
            case 0:
                GameObject flyGameObject = Instantiate(flyPrefab, spawnPosition, Quaternion.identity, enemyParent);
                settingsAI = flyGameObject.GetComponent<SettingsAI>();
                break;
            case 1:
                GameObject beeGameObject = Instantiate(beePrefab, spawnPosition, Quaternion.identity, enemyParent);
                settingsAI = beeGameObject.GetComponent<SettingsAI>();
                break;
            default:
                Debug.Log("switch");
                break;
        }

        settingsAI.point = pointGameObject;
        settingsAI.amountOfPointsToVisit = Random.Range(2, 5);
    }


    public void GameOver()
    {
        Debug.Log("GameOver");
    }

    public void OpenMenu()
    {
        enemySettingsAIArray = GameObject.FindObjectsOfType<SettingsAI>();

        foreach (SettingsAI ai in enemySettingsAIArray)
        {
            ai.DisableMovement();
        }
    }
    public void CloseMenu()
    {
        foreach (SettingsAI ai in enemySettingsAIArray)
        {
            ai.EnableMovement();
        }
    }

    public IEnumerator Timer(float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            if (isMenuOpen)
            {
                yield return null; // Skip frame update if menu is open
            }
            else
            {
                elapsedTime += Time.deltaTime;
                yield return null;
            }
        }
    }   
}
