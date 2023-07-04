using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : SingletonMonobehaviour<LoadingScene>
{
    public GameObject LoadingScreen;
    public Image LoadingBarFill;

    protected override void Awake()
    {
        base.Awake();

    }

    public IEnumerator LoadSceneAsync(int sceneId, int sceneToClose)
    {
        if (sceneToClose > 0)
        {
            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(sceneToClose);
        }
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId, LoadSceneMode.Additive);

        LoadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progression = Mathf.Clamp01(operation.progress / 0.9f);

            LoadingBarFill.fillAmount = progression;

            yield return null;
        }

        LoadingScreen.SetActive(false);
    }
}
