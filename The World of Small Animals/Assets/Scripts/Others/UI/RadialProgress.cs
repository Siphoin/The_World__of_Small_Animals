using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
    public class RadialProgress : ProgressBar, IProgressBar
    {
    [Header("Fill прогресса")]
    [SerializeField]
    private Image fillImage;


        // Use this for initialization
        void Start()
        {
        if (fillImage == null)
        {
            throw new RadialProgressException("fill image not seted");
        }

        if (LerpingValue && LerpingSpeed <= 0)
        {
            throw new RadialProgressException("lerping speed <= 0");
        }

        fillImage.type = Image.Type.Filled;

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
            fillImage.fillAmount = value;
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

            fillImage.fillAmount = Mathf.Lerp(fillImage.fillAmount, value, lerpValue);

            if (lerpValue >= 1.0f)
            {
                yield break;
            }
        }
    }


}