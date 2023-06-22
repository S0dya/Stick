using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    [SerializeField] GameObject fireflyPrefab;

    [SerializeField] GameObject pointPrefab;

    [SerializeField] int[] maxEnemies;
    public int[] curEnemies;

    public List<SettingsAI> enemySettingsAIList = new List<SettingsAI>();

    [HideInInspector] public bool isGameMenuOpen;

    Coroutine maxEnemiesIncrease;
    //Logic

    protected override void Awake()
    {
        base.Awake();

        enemyParent = GameObject.FindGameObjectWithTag("EnemyParentTransform").transform;
        pointParent = GameObject.FindGameObjectWithTag("PointParentTransform").transform;
        edgesCollider = GameObject.FindGameObjectWithTag("Edges").GetComponent<BoxCollider2D>();
        edgesColliderForTongue = GameObject.FindGameObjectWithTag("EdgesForTongue").GetComponent<BoxCollider2D>();
        background = GameObject.FindGameObjectWithTag("Background");

        curEnemies = new int[maxEnemies.Length];

        Settings.Initialize();
        background.transform.localScale = new Vector3(Settings.ScreenWidth, Settings.ScreenHeight * 0.8f, 0);
    }

    void Update()//delLtaer
    {
        Debug.Log(isGameMenuOpen);
        if (Input.GetKeyDown(KeyCode.R))
        {
            Menu.Instance.CountMoney(20);
        }
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

        Debug.Log(Player.Instance.health);
        HPBar.Instance.StartHunger();
        maxEnemiesIncrease = StartCoroutine(MaxEnemiesIncrease());
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

        int val = -1;
        if (enemySettingsAIList.Count < maxEnemies.Sum())
        {
            int cur = Random.Range(0, 100);
            if (cur > 100)
            {
                val = 0;
            }
            else if (cur > 100)
            {
                val = 1;
            }
            else
            {
                val = 2;
            }
        }
        

        curEnemies[val]++;
        switch (val)
        {
            case 0:
                GameObject flyGameObject = Instantiate(flyPrefab, spawnPosition, Quaternion.identity, enemyParent);
                settingsAI = flyGameObject.GetComponent<SettingsAI>();
                break;
            case 1:
                GameObject beeGameObject = Instantiate(beePrefab, spawnPosition, Quaternion.identity, enemyParent);
                settingsAI = beeGameObject.GetComponent<SettingsAI>();
                break;
            case 2:
                GameObject fireflyGameObject = Instantiate(fireflyPrefab, spawnPosition, Quaternion.identity, enemyParent);
                settingsAI = fireflyGameObject.GetComponent<SettingsAI>();
                break;
            default:
                Debug.Log("switch at gm");
                break;
        }

        settingsAI.point = pointGameObject;
    }


    public void OpenMenu()
    {
        isGameMenuOpen = true;

        foreach (SettingsAI ai in enemySettingsAIList)
        {
            ai.DisableMovement();
        }
    }
    public void CloseMenu()
    {
        isGameMenuOpen = false;
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
        if (maxEnemiesIncrease != null)
        {
            StopCoroutine(maxEnemiesIncrease);
        }
        HPBar.Instance.StopHunger();
    }


    public void RewardPlayer()
    {
        if (isGameMenuOpen)
        {
            GameMenu.Instance.score *= 2;
        }
        else
        {
            Menu.Instance.CountMoney(100);
        }
    }

    public IEnumerator Timer(float duration)
    {
        while (duration > 0)
        {
            if (isGameMenuOpen)
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

    public IEnumerator MaxEnemiesIncrease()
    {
        while (true)
        {

            StartCoroutine(Timer(10f));
            yield return null;
        }
    }
}
