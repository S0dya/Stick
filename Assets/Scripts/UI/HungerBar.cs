using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerBar : SingletonMonobehaviour<HungerBar>
{
    CanvasGroup canvasGroup;
    [SerializeField] Image mask;
    [HideInInspector] public float originalSize;
    float alpha;


    protected override void Awake()
    {
        base.Awake ();

        originalSize = mask.rectTransform.rect.width;
    }

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void Toggle(bool toggle)
    {
        gameObject.SetActive(toggle);
    }

    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize * value);
    }
}
