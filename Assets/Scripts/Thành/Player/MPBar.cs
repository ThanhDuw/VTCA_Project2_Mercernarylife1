using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MPBar : MonoBehaviour
{
    public Slider slider;
    public void SetMaxMP(float mp)
    {
        slider.maxValue = mp;
        slider.value = mp;
    }
    public void SetMP(float mp)
    {
        slider.value = mp;
    }
}
