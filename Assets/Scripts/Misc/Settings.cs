using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Settings
{
    public static float ScreenWidth { get; private set; }
    public static float ScreenHeight { get; private set; }

    public static void Initialize()
    {
        ScreenWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0f, 0f)).x - Camera.main.ScreenToWorldPoint(Vector3.zero).x;
        ScreenHeight = Camera.main.ScreenToWorldPoint(new Vector3(0f, Screen.height, 0f)).y - Camera.main.ScreenToWorldPoint(Vector3.zero).y;
    }
}
