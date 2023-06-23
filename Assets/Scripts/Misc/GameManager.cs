using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class GameManager : SingletonMonobehaviour<GameManager>, ISaveable
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

    public int maxEnemies;

    public List<SettingsAI> enemySettingsAIList = new List<SettingsAI>();

    [HideInInspector] public bool isGameMenuOpen;

    Coroutine maxEnemiesIncrease;

    string _iSaveableUniqueID;
    public string ISaveableUniqueID { get { return _iSaveableUniqueID; } set { _iSaveableUniqueID = value; } }
    GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }
    

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

        ISaveableUniqueID = GetComponent<GenerateGUID>().GUID;
        GameObjectSave = new GameObjectSave();

    }

    void Start()
    {
        SaveManager.Instance.LoadDataFromFile();
    }

    void Update()//delLtaer
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Menu.Instance.CountMoney(20);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            SaveManager.Instance.LoadDataFromFile();
        }
    }

    public void StartGame()
    {
        AstarPath.active.Scan();

        DefineEdgesOfScreen();
        enemySettingsAIList = new List<SettingsAI>();

        maxEnemies = 5;
        for (int i = 0; i < maxEnemies; i++)
        {
            StartCoroutine(Spawn());
        }

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
        yield return StartCoroutine(Timer(UnityEngine.Random.Range(0.1f, 1.5f)));

        float randomX = UnityEngine.Random.Range(0, 2) == 0 ? -Settings.ScreenWidth * 0.7f : Settings.ScreenWidth * 0.7f;
        float randomY = UnityEngine.Random.Range(0f, Settings.maxY);
        Vector3 spawnPosition = new Vector3(randomX, randomY, 1f);
        GameObject pointGameObject = Instantiate(pointPrefab, spawnPosition, Quaternion.identity, pointParent);
        SettingsAI settingsAI = new SettingsAI();

        int val = -1;
        if (enemySettingsAIList.Count < maxEnemies)
        {
            int cur = UnityEngine.Random.Range(0, 100);
            if (cur > 30)
            {
                val = 0;
            }
            else if (cur > 10)
            {
                val = 1;
            }
            else
            {
                val = 2;
            }
        }
        


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
            yield return StartCoroutine(Timer(10f));
            maxEnemies++;
            StartCoroutine(Spawn());
            yield return null;
        }
    }


    void OnEnable()
    {
        ISaveableRegister();
    }
    void OnDisable()
    {
        ISaveableDeregister();
    }

    public void ISaveableRegister()
    {
        SaveManager.Instance.iSaveableObjectList.Add(this);
    }

    public void ISaveableDeregister()
    {
        SaveManager.Instance.iSaveableObjectList.Remove(this);
    }

    public GameObjectSave ISaveableSave()
    {
        GameObjectSave.sceneData.Remove(Settings.GameScene);

        SceneSave sceneSave = new SceneSave();

        sceneSave.intDictionary = new Dictionary<string, int>();
        sceneSave.intArrayDictionary = new Dictionary<string, int[]>();


        sceneSave.intDictionary.Add("money", Settings.Money);
        sceneSave.intDictionary.Add("setGekoSkinIndex", Settings.SetGekoSkinIndex);
        sceneSave.intDictionary.Add("setBackgroundIndex", Settings.SetBackgroundIndex);

        int[] skinPrices = new int[Shop.Instance.skinsPrices.Length];
        int[] backgroundPrices = new int[Shop.Instance.backgroundPrices.Length];
        Array.Copy(Shop.Instance.skinsPrices, skinPrices, Shop.Instance.skinsPrices.Length);
        Array.Copy(Shop.Instance.backgroundPrices, backgroundPrices, Shop.Instance.backgroundPrices.Length);
        sceneSave.intArrayDictionary.Add("skinPrices", skinPrices);
        sceneSave.intArrayDictionary.Add("backgroundPrices", backgroundPrices);

        GameObjectSave.sceneData.Add(Settings.GameScene, sceneSave);
        return GameObjectSave;
    }


    public void ISaveableLoad(GameSave gameSave)
    {
        if (gameSave.gameObjectData.TryGetValue(ISaveableUniqueID, out GameObjectSave gameObjectSave))
        {
            if (gameObjectSave.sceneData.TryGetValue(Settings.GameScene, out SceneSave sceneSave))
            {
                Debug.Log("D");
                if (sceneSave.intDictionary != null)
                {
                    Debug.Log("D");
                    if (sceneSave.intDictionary.TryGetValue("money", out int money))
                    {
                        Debug.Log(money);
                        Settings.Money = money;
                    }
                    else if (sceneSave.intDictionary.TryGetValue("setGekoSkinIndex", out int setGekoSkinIndex))
                    {
                        Settings.SetGekoSkinIndex = setGekoSkinIndex;
                    }
                    else if (sceneSave.intDictionary.TryGetValue("setBackgroundIndex", out int setBackgroundIndex))
                    {
                        Settings.SetBackgroundIndex = setBackgroundIndex;
                    }
                }
                if (sceneSave.intArrayDictionary != null)
                {
                    if (sceneSave.intArrayDictionary.TryGetValue("skinPrices", out int[] skinPrices))
                    {
                        Shop.Instance.skinsPrices = skinPrices;
                    }
                    else if (sceneSave.intArrayDictionary.TryGetValue("backgroundPrices", out int[] backgroundPrices))
                    {
                        Shop.Instance.backgroundPrices = backgroundPrices;
                    }
                }
            }
        }
    }

    public void ISaveableStoreScene(string sceneName)
    {
    }


    public void ISaveableRestoreScene(string sceneName)
    {
    }
}
