using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : SingletonMonobehaviour<HPBar>
{
    [SerializeField] Image[] hpImages;

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
}
