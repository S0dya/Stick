using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : SingletonMonobehaviour<Tongue>
{
    Player player;
    public GameObject stickingPartObject;
    StickingPart stickingPart;

    Transform ComboTextParent;
    [SerializeField] GameObject x2MultiplayerPrefab;
    [SerializeField] GameObject x3MultiplayerPrefab;
    [SerializeField] GameObject x4MultiplayerPrefab;
    [SerializeField] GameObject x5MultiplayerPrefab;

    protected override void Awake()
    {
        base.Awake();

        player = GetComponentInParent<Player>();
        stickingPart = stickingPartObject.GetComponent<StickingPart>();

        ComboTextParent = GameObject.FindGameObjectWithTag("ComboTextParentTransform").transform;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Damage") || collision.CompareTag("Food") || collision.CompareTag("RestoreHP"))
        {
            player.isSticked = true;


            if ((collision.CompareTag("Food") || collision.CompareTag("RestoreHP")))
            {
                stickingPart.PlayCatchedEffect();

                if (PlayerTrigger.Instance.isScoreMultiplaying)
                {
                    switch (PlayerTrigger.Instance.curMultiplayer)
                    {
                        case 1.5f:
                            Debug.Log(1);
                            Instantiate(x2MultiplayerPrefab, stickingPartObject.transform.position, Quaternion.identity, ComboTextParent);
                            break;
                        case 2f:
                            Debug.Log(2);
                            Instantiate(x3MultiplayerPrefab, stickingPartObject.transform.position, Quaternion.identity, ComboTextParent);
                            break;
                        case 2.5f:
                            Debug.Log(3);
                            Instantiate(x4MultiplayerPrefab, stickingPartObject.transform.position, Quaternion.identity, ComboTextParent);
                            break;
                        case 3f:
                            Debug.Log(4);
                            Instantiate(x5MultiplayerPrefab, stickingPartObject.transform.position, Quaternion.identity, ComboTextParent);
                            break;
                        default:
                            Debug.Log("def");
                            break;
                    }
                }
            }

            EnemyAI enemyAi = collision.gameObject.GetComponent<EnemyAI>();
            enemyAi.canMove = false;

            collision.transform.SetParent(stickingPartObject.transform);
        }
    }
}
