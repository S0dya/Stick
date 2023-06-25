using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickingPart : MonoBehaviour
{
    [SerializeField] ParticleSystem catchedEffect;

    public void PlayCatchedEffect()
    {
        Instantiate(catchedEffect, transform.position, Quaternion.identity);
    }



    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("EdgesForTongue"))
        {
            Player.Instance.isOutOfTrigger = true;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EdgesForTongue"))
        {
            Player.Instance.isOutOfTrigger = false;
        }
    }
}
