using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>, ISaveable
{
    [SerializeField] BoxCollider2D edgesCollider;
    [SerializeField] BoxCollider2D edgesColliderForTongue;
    
    //Gameplay
    Transform enemyParent;

    [SerializeField] GameObject flyPrefab;
    [SerializeField] GameObject beePrefab;
    [SerializeField] GameObject fireflyPrefab;

    [SerializeField] GameObject playerPrefab;

    public Sprite[] GekoSkins;
    public Sprite[] backgrounds;
    public Color[] tongueColorsStart;
    public Color[] tongueColorsEnd;
    public int[] skinsPrices;
    public int[] backgroundsPrices;

    public List<SettingsAI> enemySettingsAIList = new List<SettingsAI>();
    public int maxEnemies;

    [HideInInspector] public bool isGameMenuOpen;

    Coroutine maxEnemiesIncrease;

    GameObjectSave _gameObjectSave;
    public GameObjectSave GameObjectSave { get { return _gameObjectSave; } set { _gameObjectSave = value; } }

    float fireFlySpawnChance;
    float beeSpawnChance;

    protected override void Awake()
    {
        base.Awake();
        Settings.Initialize();
        GameObjectSave = new GameObjectSave();

        float scaleFactor = Screen.width * 0.0012f;
        flyPrefab.transform.localScale = new Vector2(scaleFactor, scaleFactor);
        beePrefab.transform.localScale = new Vector2(scaleFactor, scaleFactor);
        fireflyPrefab.transform.localScale = new Vector2(scaleFactor, scaleFactor);
        playerPrefab.transform.localScale = new Vector2(scaleFactor, scaleFactor);
    }

    void Start()
    {
        SaveManager.Instance.LoadDataFromFile();
        LoadingScene.Instance.StartCoroutine(LoadingScene.Instance.LoadSceneAsync(1, -1));
    }

    void Update()//delLtaer
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Menu.Instance.CountMoney(1000);
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //SaveManager.Instance.SaveDataFromFile();
            AudioManager.Instance.ChangeMusic();
        }
    }

    public void StartGame()
    {
        enemyParent = GameObject.FindGameObjectWithTag("EnemyParentTransform").transform;

        DefineEdgesOfScreen();
        enemySettingsAIList = new List<SettingsAI>();

        maxEnemies = 5;
        for (int i = 0; i < maxEnemies; i++)
        {
            StartCoroutine(Spawn());
        }

        HPBar.Instance.StartHunger();
        beeSpawnChance = 30;
        fireFlySpawnChance = 3;
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

        int val = -1;
        if (enemySettingsAIList.Count < maxEnemies)
        {
            int cur = Random.Range(0, 100);
            if (cur > beeSpawnChance)
            {
                val = 0;
            }
            else if (cur > fireFlySpawnChance)
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
                Instantiate(flyPrefab, spawnPosition, Quaternion.identity, enemyParent);
                break;
            case 1:
                Instantiate(beePrefab, spawnPosition, Quaternion.identity, enemyParent);
                break;
            case 2:
                Instantiate(fireflyPrefab, spawnPosition, Quaternion.identity, enemyParent);
                break;
            default:
                break;
        }
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
        GameObject[] comboTextObjs = GameObject.FindGameObjectsWithTag("ComboText");
        List<Combo> comboTexts = new List<Combo>();
        foreach (var cto in comboTextObjs)
        {
            comboTexts.Add(cto.GetComponent<Combo>());
        }

        StopAllCoroutines();
        while (enemySettingsAIList.Count > 0)
        {
            enemySettingsAIList[0].Clear();
        }
        if (maxEnemiesIncrease != null)
        {
            StopCoroutine(maxEnemiesIncrease);
        }

        while (comboTexts.Count > 0)
        {
            comboTexts[0].Clear();
            comboTexts.RemoveAt(0);
        }

        HPBar.Instance.StopHunger();
    }


    public void RewardPlayer()
    {
        if (isGameMenuOpen)
        {
            GameMenu.Instance.DoubleScore();
            GameMenu.Instance.ShowScoreInGameMenu();
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
            if (isGameMenuOpen)
            {
                yield return null;
            }
            if (maxEnemies < 17)
            {
                yield return StartCoroutine(Timer(10f));
                
                maxEnemies++;
            }

            yield return StartCoroutine(Timer(7f));
            if (beeSpawnChance > 50)
            {
                break;
            }
            beeSpawnChance++;
            fireFlySpawnChance += 0.5f;
            StartCoroutine(Spawn());
            yield return null;
        }
    }
    
    public void ToggleEnemiesToNight(bool var)
    {
        foreach (var ai in enemySettingsAIList)
        {
            ai.ToggleLight(var);
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
        sceneSave.boolDictionary = new Dictionary<string, bool>();

        sceneSave.intDictionary.Add("money", Settings.Money);
        sceneSave.intDictionary.Add("setGekoSkinIndex", Settings.SetGekoSkinIndex);
        sceneSave.intDictionary.Add("setBackgroundIndex", Settings.SetBackgroundIndex);
        sceneSave.boolDictionary.Add("isMusicEnabled", Settings.IsMusicEnabled);

        int[] skinPrices = new int[skinsPrices.Length];
        int[] backgroundPrices = new int[backgroundsPrices.Length];
        for (int i = 0; i < skinPrices.Length; i++)
        {
            skinPrices[i] = skinsPrices[i];
        }
        for (int i = 0; i < backgroundPrices.Length; i++)
        {
            backgroundPrices[i] = backgroundsPrices[i];
        }
        sceneSave.intArrayDictionary.Add("skinPrices", skinPrices);
        sceneSave.intArrayDictionary.Add("backgroundPrices", backgroundPrices);

        GameObjectSave.sceneData.Add(Settings.GameScene, sceneSave);
        return GameObjectSave;
    }


    public void ISaveableLoad(GameObjectSave gameObjectSave)
    {
        if (gameObjectSave.sceneData.TryGetValue(Settings.GameScene, out SceneSave sceneSave))
        {
            if (sceneSave.intDictionary != null)
            {
                if (sceneSave.intDictionary.TryGetValue("money", out int money))
                {
                    Settings.Money = money;
                }
                if (sceneSave.intDictionary.TryGetValue("setGekoSkinIndex", out int setGekoSkinIndex))
                {
                    Settings.SetGekoSkinIndex = setGekoSkinIndex;
                }
                if (sceneSave.intDictionary.TryGetValue("setBackgroundIndex", out int setBackgroundIndex))
                {
                    Settings.SetBackgroundIndex = setBackgroundIndex;
                }
            }
            if (sceneSave.intArrayDictionary != null)
            {
                if (sceneSave.intArrayDictionary.TryGetValue("skinPrices", out int[] skinPrices))
                {
                    for (int i = 0; i < skinsPrices.Length; i++)
                    {
                        skinsPrices[i] = skinPrices[i];
                    }
                }
                if (sceneSave.intArrayDictionary.TryGetValue("backgroundPrices", out int[] backgroundPrices))
                {
                    for (int i = 0; i < backgroundsPrices.Length; i++)
                    {
                        backgroundsPrices[i] = backgroundPrices[i];
                    }
                }
            }
            if (sceneSave.boolDictionary != null)
            {
                if (sceneSave.boolDictionary.TryGetValue("isMusicEnabled", out bool isMusicEnabled))
                {
                    Settings.IsMusicEnabled = isMusicEnabled;
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
