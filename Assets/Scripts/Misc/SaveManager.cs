using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class SaveManager : SingletonMonobehaviour<SaveManager>
{
    public GameSave gameSave;
    public List<ISaveable> iSaveableObjectList;

    protected override void Awake()
    {
        base.Awake();

        iSaveableObjectList = new List<ISaveable>();
    }

    public void LoadDataFromFile()
    {
        string filePath = Application.persistentDataPath + "/data.json";

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            gameSave = JsonConvert.DeserializeObject<GameSave>(json);

            for (int i = iSaveableObjectList.Count - 1; i >= 0; i--)
            {
                if (gameSave.gameObjectData.ContainsKey(iSaveableObjectList[i].ISaveableUniqueID))
                {
                    iSaveableObjectList[i].ISaveableLoad(gameSave);
                }
                else
                {
                    Component component = (Component)iSaveableObjectList[i];
                    Destroy(component.gameObject);
                }
            }
        }

    }

    public void SaveDataToFile()
    {
        gameSave = new GameSave();

        foreach (ISaveable iSaveableObject in iSaveableObjectList)
        {
            gameSave.gameObjectData.Add(iSaveableObject.ISaveableUniqueID, iSaveableObject.ISaveableSave());
        }

        string filePath = Application.persistentDataPath + "/data.json";
        string json = JsonConvert.SerializeObject(gameSave);
        File.WriteAllText(filePath, json);

    }

    public void StoreCurrentSceneData()
    {
        foreach (ISaveable iSaveableObject in iSaveableObjectList)
        {
            iSaveableObject.ISaveableStoreScene(SceneManager.GetActiveScene().name);
        }
    }

    public void RestoreCurrentSceneData()
    {
        foreach (ISaveable iSaveableObject in iSaveableObjectList)
        {
            iSaveableObject.ISaveableRestoreScene(SceneManager.GetActiveScene().name);
        }
    }
}