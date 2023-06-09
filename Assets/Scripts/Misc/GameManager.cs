using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonobehaviour<GameManager>
{
    bool isMenuOpen;

    protected override void Awake()
    {
        base.Awake();

        Settings.Initialize();
    }

    void Start()
    {


        AstarPath.active.Scan();
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
