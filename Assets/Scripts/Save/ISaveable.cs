public interface ISaveable
{
    GameObjectSave GameObjectSave { get; set; }

    void ISaveableRegister();

    void ISaveableDeregister();

    GameObjectSave ISaveableSave();

    void ISaveableLoad(GameObjectSave gameObjectSave);


    void ISaveableStoreScene(string sceneName);

    void ISaveableRestoreScene(string sceneName);

}
