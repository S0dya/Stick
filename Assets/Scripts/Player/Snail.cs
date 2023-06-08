using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : SingletonMonobehaviour<Snail>
{

    Player player;


    protected override void Awake()
    {
        base.Awake();

        player = GetComponentInParent<Player>();

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && player.isElongating)
        {

            if (player.snailLength < 1.5f)
            {
                player.isPushing = true;
            }
            else
            {
                player.isSticked = true;

            }
            
        }
    }


    public void UnStick()
    {
        player.isSticked = false;
    }
}
