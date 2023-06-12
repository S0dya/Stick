using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickingPart : MonoBehaviour
{


    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Edges"))
        {
            Player.Instance.isOutOfTrigger = true;
        }
    }
}
