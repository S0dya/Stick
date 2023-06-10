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

    [SerializeField] List<GameObject> enemyList = new List<GameObject>();

    [SerializeField] float maxEnemies;
    [SerializeField] float maxFlies;
    [SerializeField] float maxBees;
    [SerializeField] float maxButterflies;


    bool isMenuOpen;

    

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
            Spawn();
        }


    }

    void DefineEdgesOfScreen()
    {
        edgesCollider.size = new Vector2(Settings.ScreenWidth * 1.6f, Settings.ScreenHeight * 1.6f);
    }
    

    public void Spawn()
    {


        float randomX = Random.Range(0, 2) == 0 ? -Settings.ScreenWidth *0.7f : Settings.ScreenWidth *0.7f;
        float randomY = Random.Range(0f, Settings.maxY);

        Vector3 spawnPosition = new Vector3(randomX, randomY, 1f);

        GameObject pointGameObject = Instantiate(pointPrefab, spawnPosition, Quaternion.identity, pointParent);
        GameObject flyGameObject = Instantiate(flyPrefab, spawnPosition, Quaternion.identity, enemyParent);

        SettingsAI settingsAI = flyGameObject.GetComponent<SettingsAI>();
        settingsAI.point = pointGameObject;
        settingsAI.amountOfPointsToVisit = Random.Range(5, 10);
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
