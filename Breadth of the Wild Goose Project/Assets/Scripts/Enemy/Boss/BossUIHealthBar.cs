using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUIHealthBar : MonoBehaviour
{
    public Image mask;
    float originalSize;

    public static BossUIHealthBar instance { get; private set; }
    // Start is called before the first frame update
    void Awake() 
    {
        instance = this;
    }

    // Update is called once per frame
    void Start()
    {
        originalSize = mask.rectTransform.rect.height;
    }

    public void SetValue(float value)
    {
        mask.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, originalSize * value);
    }
}
