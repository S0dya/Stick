using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    //Game 
    public static string GameScene = "SampleScene";

    public static int Money;
    public static bool IsMusicEnabled;

    public static int GekoSkinIndex;
    public static int SetGekoSkinIndex;

    public static int BackgroundIndex;
    public static int SetBackgroundIndex;

    //Screen
    public static float ScreenWidth { get; private set; }
    public static float ScreenHeight { get; private set; }
    public static float heightForInput { get; private set; }


    public static float minX { get; private set; }
    public static float minY { get; private set; }
    public static float maxX { get; private set; }
    public static float maxY { get; private set; }

    public static float blindZoneOfY { get; private set; }

    public static float posYForCamDown { get; private set; }
    public static float posYForCamUp { get; private set; }

    //Player
    public static int maxHealth { get; private set; }
    public static float tongueMultiplyer { get; private set; }
    public static float scoreMultiplyer { get; private set; }
    public static float timeWhileScoreMultiplying { get; private set; }


    public static void Initialize()
    {
        ScreenWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x - Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        ScreenHeight = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y - Camera.main.ScreenToWorldPoint(Vector3.zero).y;
        heightForInput = Screen.height;

        minX = -ScreenWidth / 2f;
        maxX = ScreenWidth / 2f;
        maxY = ScreenHeight / 2f;
        minY = -ScreenHeight / 5f;

        blindZoneOfY = 0.1f;

        posYForCamDown = -4.7f;
        posYForCamUp = 0.7f;

        maxHealth = 2;
        tongueMultiplyer = 30f;
        scoreMultiplyer = 0.2f;
        timeWhileScoreMultiplying = 1f;

        GekoSkinIndex = 0;//DelLater
        BackgroundIndex = 0;
    }
}
