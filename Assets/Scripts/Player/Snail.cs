using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : SingletonMonobehaviour<Snail>
{

    

    
    protected override void Awake()
    {
        base.Awake();


    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Debug.Log("DAS");
            Player.Instance.isSticked = true;

            Player.Instance.OnSticked();
        }
    }
}
