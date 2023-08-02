using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class Combo : MonoBehaviour
{
    Image comboImage;
    float duration;
    Color origColor;
    Color targetColor;
    float time;

    void Start()
    {
        comboImage = GetComponent<Image>();
        duration = 3f;
        origColor = comboImage.color;
        targetColor = new Color(origColor.r, origColor.g, origColor.b, 0f);
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.isGameMenuOpen)
            return;

        float normalizedTime = time / duration;
        comboImage.color = Color.Lerp(origColor, targetColor, normalizedTime);
        time += Time.deltaTime;

        if (time > duration)
        {
            Clear();
        }
    }

    public void Clear()
    {
        Destroy(gameObject);
    }
}
