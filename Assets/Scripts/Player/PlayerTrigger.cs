using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : SingletonMonobehaviour<PlayerTrigger>
{
    Player player;
   
    protected override void Awake()
    {
        base.Awake();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Food") || collision.CompareTag("RestoreHP") || collision.CompareTag("Damage"))
        {
            SettingsAI settingsAi = collision.gameObject.GetComponent<SettingsAI>();
            if (settingsAi.isDestroying)
                return;

            settingsAi.Die();

            if (collision.CompareTag("Damage"))
            {
                player.MinusHp(true);
            }
            else if (collision.CompareTag("RestoreHP"))
            {
                player.MinusHp(false);
            }
        }
    }
}
