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
        if (collision.CompareTag("Damage") || collision.CompareTag("Food") || collision.CompareTag("RestoreHP"))
        {
            player.isSticked = true;

            EnemyAI enemyAi = collision.gameObject.GetComponent<EnemyAI>();
            enemyAi.canMove = false;

            collision.transform.SetParent(StickingPart.transform);
        }
    }
    

}
