using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    public void HeartPopped()
    {
        Player.Instance.MinusHp(true);
    }

    public void OnHeartPopped()
    {
        if (Player.Instance.health < 0)
        {
            GameMenu.Instance.GameOver();
        }
    }
}
