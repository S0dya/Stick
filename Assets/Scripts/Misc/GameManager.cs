using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    BoxCollider2D edgesCollider;
    BoxCollider2D edgesColliderForTongue;
    GameObject background; 

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

    public List<SettingsAI> enemySettingsAIList = new List<SettingsAI>();

    [HideInInspector] public bool isMenuOpen;

    //Logic

    protected override void Awake()
    {
        base.Awake();

        enemyParent = GameObject.FindGameObjectWithTag("EnemyParentTransform").transform;
        pointParent = GameObject.FindGameObjectWithTag("PointParentTransform").transform;
        edgesCollider = GameObject.FindGameObjectWithTag("Edges").GetComponent<BoxCollider2D>();
        edgesColliderForTongue = GameObject.FindGameObjectWithTag("EdgesForTongue").GetComponent<BoxCollider2D>();
        background = GameObject.FindGameObjectWithTag("Background");

        Settings.Initialize();
        background.transform.localScale = new Vector3(Settings.ScreenWidth, Settings.ScreenHeight * 0.8f, 0);
    }

    void Update()//delLtaer
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Settings.Money += 5;
        }
        Debug.Log(Settings.Money);
    }

    public void StartGame()
    {
        AstarPath.active.Scan();

        DefineEdgesOfScreen();
        enemySettingsAIList = new List<SettingsAI>();

        for (int i = 0; i < 15; i++)
        {
            StartCoroutine(Spawn());
        }
    }

    void DefineEdgesOfScreen()
    {
        edgesCollider.size = new Vector2(Settings.ScreenWidth * 1.6f, Settings.ScreenHeight * 1.6f);
        edgesColliderForTongue.size = new Vector2(Settings.ScreenWidth * 1.2f, Settings.ScreenHeight * 1.3f);
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
                Debug.Log("switch at gm");
                break;
        }

        settingsAI.point = pointGameObject;
        settingsAI.amountOfPointsToVisit = Random.Range(2, 5);
    }


    public void OpenMenu()
    {
        isMenuOpen = true;

        foreach (SettingsAI ai in enemySettingsAIList)
        {
            ai.DisableMovement();
        }
    }
    public void CloseMenu()
    {
        isMenuOpen = false;
        foreach (SettingsAI ai in enemySettingsAIList)
        {
            ai.EnableMovement();
        }
    }

    public void ClearGame()
    {
        StopAllCoroutines();
        while (enemySettingsAIList.Count > 0)
        {
            enemySettingsAIList[0].Clear();
        }
    }


    public IEnumerator Timer(float duration)
    {
        while (duration > 0)
        {
            if (isMenuOpen)
            {
                yield return null;
            }
            else
            {
                duration -= Time.deltaTime;
                yield return null;
            }
        }
    }   


}
