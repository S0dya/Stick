using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : SingletonMonobehaviour<Tongue>
{

    Player player;
    public GameObject StickingPart;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponentInParent<Player>();

    }


    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food"))
        {
            player.isSticked = true;

            collision.transform.SetParent(StickingPart.transform);
        }
        else if (collision.CompareTag("Edge"))
        {
            player.isOnTrigger = true;
        }
    }


    public void UnStick()
    {
        player.isSticked = false;
    }
}
