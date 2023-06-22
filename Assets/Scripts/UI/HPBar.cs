using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : SingletonMonobehaviour<HPBar>
{
    [SerializeField] Image[] hpImages;

    Coroutine hunger;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SetHPImage(int index, bool val)
    {
        hpImages[index].enabled = val;
    }

    public void ResetHPImages()
    {
        foreach (Image i in hpImages)
        {
            i.enabled = true;
        }
    }

    public void StartHunger()
    {
        if (Player.Instance.health > -1)
            hunger = StartCoroutine(Hunger(Player.Instance.health));
    }
    public void StopHunger()
    {
        if (hunger != null)
        {
            StopCoroutine(hunger);
        }
        hpImages[Player.Instance.health].enabled = true;
    }

    IEnumerator Hunger(int index)
    {
        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(7f));
        hpImages[index].enabled = false;

        yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(0.5f));
        hpImages[index].enabled = true;

        for (int i = 0; i < 2; i++)
        {
            yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(4f));
        
            hpImages[index].enabled = false;

            yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(0.5f));
            hpImages[index].enabled = true;
        }
        for (int i = 0; i < 3; i++)
        {
            yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(2f));

            hpImages[index].enabled = false;

            yield return GameManager.Instance.StartCoroutine(GameManager.Instance.Timer(0.2f));
            hpImages[index].enabled = true;
        }

        Player.Instance.MinusHp(true);
        StartHunger();
        yield return null;
    }
}
