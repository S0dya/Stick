using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tongue : SingletonMonobehaviour<Tongue>
{
    Player player;
    public GameObject stickingPartObject;
    StickingPart stickingPart;
    LineRenderer tongue;

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
        tongue = GetComponent<LineRenderer>();

        ComboTextParent = GameObject.FindGameObjectWithTag("ComboTextParentTransform").transform;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (player.isSticked)
        {
            return;
        }
        else if (collision.CompareTag("Damage") || collision.CompareTag("Food") || collision.CompareTag("RestoreHP"))
        {
            player.isSticked = true;

            if ((collision.CompareTag("Food") || collision.CompareTag("RestoreHP")))
            {
                stickingPart.PlayCatchedEffect();

                if (PlayerTrigger.Instance.isScoreMultiplaying)
                {
                    GameObject multiplayerObj = new GameObject();

                    switch (PlayerTrigger.Instance.curMultiplayer)
                    {
                        case 1.5f:
                            multiplayerObj = Instantiate(x2MultiplayerPrefab, ComboTextParent);
                            break;
                        case 2f:
                            multiplayerObj = Instantiate(x3MultiplayerPrefab, ComboTextParent);
                            break;
                        case 2.5f:
                            multiplayerObj = Instantiate(x4MultiplayerPrefab, ComboTextParent);
                            break;
                        case 3f:
                            multiplayerObj = Instantiate(x5MultiplayerPrefab, ComboTextParent);
                            break;
                        default:
                            break;
                    }

                    multiplayerObj.transform.position = Camera.main.WorldToScreenPoint(stickingPartObject.transform.position);
                }
            }

            EnemyAI enemyAi = collision.gameObject.GetComponent<EnemyAI>();
            enemyAi.canMove = false;

            collision.transform.SetParent(stickingPartObject.transform);
        }
    }

    public void SetColor(Color startColor, Color endColor)
    {
        tongue.startColor = startColor;
        tongue.endColor = endColor;
    }
}
