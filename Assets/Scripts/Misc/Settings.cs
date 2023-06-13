using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    //Screen
    public static float ScreenWidth { get; private set; }
    public static float ScreenHeight { get; private set; }

    public static float minX { get; private set; }
    public static float minY { get; private set; }
    public static float maxX { get; private set; }
    public static float maxY { get; private set; }


    //Player
    public static int maxHealth { get; private set; }
    public static float tongueMultiplyer { get; private set; }
    public static float scoreMultiplyer { get; private set; }
    public static float timeWhileScoreMultiplying { get; private set; }


    public static void Initialize()
    {
        ScreenWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x - Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        ScreenHeight = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y - Camera.main.ScreenToWorldPoint(Vector3.zero).y;

        minX = -ScreenWidth / 2f;
        maxX = ScreenWidth / 2f;
        maxY = ScreenHeight / 2f;
        minY = -ScreenHeight / 3f;

        maxHealth = 2;
        tongueMultiplyer = 30f;
        scoreMultiplyer = 0.2f;
        timeWhileScoreMultiplying = 1f;
    }
}
