using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : SingletonMonobehaviour<HPBar>
{
    [SerializeField] Animator[] animators;

    protected override void Awake()
    {
        base.Awake();

    }

    public void SetHP(int index, bool val)
    {
        if (val)
        {
            animators[index].Play("HeartAppear");
        }
        else
        {
            animators[index].Play("HeartPop");
        }
    }

    public void ResetHPImages()
    {
        foreach (Animator a in animators)
        {
            a.Play("HeartAppear");
        }
    }

    public void StartHunger()
    {
        if (Player.Instance.health > -1)
        {
            animators[Player.Instance.health].Play("HeartHunger");
        }
    }

    public void RestartHunger()
    {
        animators[Player.Instance.health].Play("HeartFullToHunger");
    }

    public void StopHunger()
    {
        animators[Player.Instance.health].Play("HeartFull");
    }

    public void ToggleAnim(bool val) 
    {
        foreach (Animator a in animators) 
        {
            a.enabled = val;
        }
    }
}
