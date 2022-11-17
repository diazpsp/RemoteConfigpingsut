using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthBar : MonoBehaviour
{
    public Image image;
    public void UpdateBar(float filledAmount)
    {
        image.DOFillAmount(filledAmount, 0.5f);
        if (filledAmount > 0.6f)
        {
            image.DOColor(Color.green, 0.5f);
        }
        else if (filledAmount > 0.3f)
        {
            image.DOColor(Color.yellow, 0.5f);
        }
        else
        {
            image.DOColor(Color.red, 0.5f);
        }
    }
}
