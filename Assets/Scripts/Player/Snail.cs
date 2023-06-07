using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : SingletonMonobehaviour<Snail>
{
    [SerializeField] GameObject stickedPartObject;
    TargetJoint2D joint;




    protected override void Awake()
    {
        base.Awake();

        joint = stickedPartObject.GetComponent<TargetJoint2D>();
        joint.enabled = false;

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") && Player.Instance.isElongating)
        {
            Player.Instance.isSticked = true;

            joint.transform.position = collision.contacts[0].point;
            joint.enabled = true;
        }
    }


    public void UnStick()
    {
        Player.Instance.isSticked = false;
        joint.enabled = false;
    }
}
