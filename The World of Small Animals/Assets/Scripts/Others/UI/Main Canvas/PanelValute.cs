using System.Collections;
using UnityEngine;
using TMPro;
    public class PanelValute : GeterDataCharacter
    {
     [Header("Скорость анимации набирания счета у текста валюты")]
     [SerializeField] private float speedAnimNewValute = 0.5f;

    [Header("Текст значения аникоинов")]
    [SerializeField] private TextMeshProUGUI textAnicoins;

    [Header("Текст значения самоцветов")]
    [SerializeField] private TextMeshProUGUI textGems;

    private int oldValueAnicoins;
    private int oldValueGems;
    // Use this for initialization
    void Start()
        {
        Ini();


        if (speedAnimNewValute <= 0f)
        {
            throw new PanelValuteException("speed anim new value invalid");
        }
        if (textGems == null)
        {
            throw new PanelValuteException("text gems not seted");
        }

        if (textAnicoins == null)
        {
            throw new PanelValuteException("text anicoins not seted");
        }

        RefreshValuteData();
        }

    private void RefreshValuteData ()
    {
        StopAllCoroutines();

        if (CheckingValuteData(oldValueAnicoins, CharacterData.anicoins, textAnicoins))
        {
            oldValueAnicoins = CharacterData.anicoins;
        }

        if (CheckingValuteData(oldValueAnicoins, CharacterData.gems, textGems))
        {
            oldValueGems = CharacterData.gems;
        }

    }

    private bool CheckingValuteData (int oldValue, int currentValue, TextMeshProUGUI text)
    {

        bool equalsValues = oldValue != currentValue;


        if (equalsValues)
        {
            StartCoroutine(AnimationSetingValueValute(text, oldValue, currentValue));
        }

        return equalsValues;

        
    }

    private IEnumerator AnimationSetingValueValute (TextMeshProUGUI text, int oldValue, int newValue)
    {

        float lerpValue = 0;


        while (true)
        {
            float time = speedAnimNewValute * Time.deltaTime;

            yield return new WaitForSeconds(time);

            long value = (long)Mathf.Lerp(oldValue, newValue, lerpValue);
            lerpValue += time;

            text.text = value.ToString();

            if (lerpValue >= 1.0f)
            {
                yield break;
            }
        }
    }

    }