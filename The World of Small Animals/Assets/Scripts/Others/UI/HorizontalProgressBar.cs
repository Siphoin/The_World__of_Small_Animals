using System.Collections;
using UnityEngine;

public class HorizontalProgressBar : ProgressBar, IProgressBar
    {
    
    public IEnumerator UpdateProgresAsync(float value)
    {
        float lerpValue = 0f;

        while (true)
        {
            float time = LerpingSpeed * Time.deltaTime;

            yield return new WaitForSeconds(time);

            lerpValue += time;

            _slider.value = Mathf.Lerp(_slider.value, value, lerpValue);

            if (lerpValue >= 1.0f)
            {
                yield break;
            }
        }
    }

    public void UpdateProgress(float value)
    {

        Ini();


        if (LerpingValue)
        {
            StopAllCoroutines();
            StartCoroutine(UpdateProgresAsync(value));
        }

        else
        {
            _slider.value = value;
        }
    }

    public void SetMaxValue (long value)
    {
        Ini();

        slider.maxValue = value;
    }
}
