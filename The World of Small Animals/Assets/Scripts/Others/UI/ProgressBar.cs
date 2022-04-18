using System.Collections;
using UnityEngine;
using UnityEngine.UI;

    public class ProgressBar : MonoBehaviour
    {
        protected Slider _slider;


        [Header("Плавное изменение полоски")]
        [SerializeField] private bool _lerpingValue = false;

    [Header("Скорость плавного изменения значения")]
    [SerializeField] private float _lerpingSpeed = 0;

    public bool LerpingValue { get => _lerpingValue; }
    public float LerpingSpeed { get => _lerpingSpeed; }

    public float Value { get => _slider.value; }


    protected void Ini ()
        {
        if (LerpingValue && LerpingSpeed <= 0)
        {
            throw new ProgressBarException("lerping speed <= 0");
        }


        if (!TryGetComponent(out _slider))
            {
                throw new ProgressBarException($"{name} not have component Slider");
            }
        }
    }
