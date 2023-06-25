using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text;

public class ComboText : MonoBehaviour
{
    TextMeshProUGUI comboText;
    float duration;
    Color origColor;
    Color targetColor;
    float time;

    void Start()
    {
        comboText = GetComponent<TextMeshProUGUI>();
        duration = 3f;
        origColor = comboText.color;
        targetColor = new Color(origColor.r, origColor.g, origColor.b, 0f);
    }

    void FixedUpdate()
    {
        if (GameManager.Instance.isGameMenuOpen)
            return;

        float normalizedTime = time / duration;
        comboText.color = Color.Lerp(origColor, targetColor, normalizedTime);
        time += Time.deltaTime;

        if (time > duration)
        {
            Destroy(gameObject);
        }
    }
}
