using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : SingletonMonobehaviour<Menu>
{
    Camera camera;
    GameObject menuBottomButtonsBarObject;
    GameObject gameMenuObject;

    Coroutine moveCamUpCoroutine;


    void Awake()
    {
        base.Awake();

        camera = Camera.main;
        menuBottomButtonsBarObject = GameObject.FindGameObjectWithTag("MenuBottomButtonsBar");
        gameMenuObject = GameObject.FindGameObjectWithTag("GameMenu");
    }

    void Start()
    {
        gameMenuObject.SetActive(false);
    }


    public void Play()
    {
        moveCamUpCoroutine = StartCoroutine(MoveCamUp());
    }

    public void CountMoney(float score)
    {
        Settings.Money += score / 5f;
    }

    IEnumerator MoveCamUp()
    {
        float curY = camera.transform.position.y;
        while (camera.transform.position.y < Settings.posYForCamUp - 0.7f)
        {
            float y = Mathf.Lerp(camera.transform.position.y, Settings.posYForCamUp, 0.05f);

            camera.transform.position = new Vector2(camera.transform.position.x, y);
            yield return null;
        }

        StartGame();
        yield return null;
    }
    void StartGame()
    {
        gameMenuObject.SetActive(true);
        gameObject.SetActive(false);
        GameMenu.Instance.ClearGame();
        GameManager.Instance.StartGame();
    }

    public void ToggleButtonsBar(bool val)
    {
        menuBottomButtonsBarObject.SetActive(val);
    }
}
