using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
    public class RadialProgress : ProgressBar, IProgressBar
    {
    [Header("Fill прогресса")]
    [SerializeField]
    private Image _fillImage;

    public float Value { get => _fillImage.fillAmount; }


      
        void Start()
        {
        
        if (_fillImage == null)
        {
            throw new RadialProgressException("fill image not seted");
        }

        if (LerpingValue && LerpingSpeed <= 0)
        {
            throw new RadialProgressException("lerping speed <= 0");
        }

        _fillImage.type = Image.Type.Filled;

        }
    public void UpdateProgress (float value)
    {
        if (LerpingValue)
        {
            StopAllCoroutines();
            StartCoroutine(UpdateProgresAsync(value));
        }

        else
        {
            _fillImage.fillAmount = value;
        }
    }


    public IEnumerator UpdateProgresAsync(float value)
    {
        float lerpValue = 0f;

        value = Mathf.Clamp(value, 0f, 1.0f);

        while (true)
        {
            float time = LerpingSpeed * Time.deltaTime;

            yield return new WaitForSeconds(time);

            lerpValue += time;

            _fillImage.fillAmount = Mathf.Lerp(_fillImage.fillAmount, value, _lerpValue);

            if (_lerpValue >= 1.0f)
            {
                yield break;
            }
        }
    }


}
