using System.Collections;
using UnityEngine;
using UnityEngine.UI;

    public class ProgressBar : MonoBehaviour
    {
        protected Slider slider;


        [Header("Плавное изменение полоски")]
        [SerializeField] private bool lerpingValue = false;

    [Header("Скорость плавного изменения значения")]
    [SerializeField] private float lerpingSpeed = 0;

    public bool LerpingValue { get => lerpingValue; }
    public float LerpingSpeed { get => lerpingSpeed; }

    protected void Ini ()
        {
        if (LerpingValue && LerpingSpeed <= 0)
        {
            throw new ProgressBarException("lerping speed <= 0");
        }


        if (!TryGetComponent(out slider))
            {
                throw new ProgressBarException($"{name} not have component Slider");
            }
        }
    }