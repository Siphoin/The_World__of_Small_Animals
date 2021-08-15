using System.Collections;
using UnityEngine;
using TMPro;

    public class PanelValute : GeterDataCharacter
    {

    private int _oldValueAnicoins = 0;
    private int _oldValueGems = 0;


    [Header("Скорость анимации набирания счета у текста валюты")]
     [SerializeField] private float _speedAnimNewValute = 0.5f;

    [Header("Текст значения аникоинов")]
    [SerializeField] private TextMeshProUGUI _textAnicoins;

    [Header("Текст значения самоцветов")]
    [SerializeField] private TextMeshProUGUI _textGems;

   private void Start()
        {
        Ini();

        if (_speedAnimNewValute <= 0f)
        {
            throw new PanelValuteException("speed anim new value invalid");
        }
        if (_textGems == null)
        {
            throw new PanelValuteException("text gems not seted");
        }

        if (_textAnicoins == null)
        {
            throw new PanelValuteException("text anicoins not seted");
        }

        RefreshValuteData();
        }

    private void RefreshValuteDataUsingAnimation ()
    {
        StopAllCoroutines();

        if (CheckingValuteData(_oldValueAnicoins, CharacterData.Anicoins, _textAnicoins))
        {
            _oldValueAnicoins = CharacterData.Anicoins;
        }

        if (CheckingValuteData(_oldValueGems, CharacterData.Gems, _textGems))
        {
            _oldValueGems = CharacterData.Gems;
        }

    }

    private void RefreshValuteData()
    {
        _oldValueGems = CharacterData.Gems;
        _oldValueAnicoins = CharacterData.Anicoins;

        _textAnicoins.text = _oldValueAnicoins.ToString();
        _textGems.text = _oldValueGems.ToString();

    }

    private bool CheckingValuteData (int oldValue, int currentValue, TextMeshProUGUI text)
    {
        bool equalsValues = oldValue != currentValue;

        if (equalsValues)
        {
            StartCoroutine(AnimationSetingValueValute(text, oldValue, currentValue));
        }

        else
        {
            text.text = currentValue.ToString();
        }

        return equalsValues;

        
    }

    private IEnumerator AnimationSetingValueValute (TextMeshProUGUI text, int oldValue, int newValue)
    {
        float lerpValue = 0;


        while (true)
        {
            float time = _speedAnimNewValute * Time.deltaTime;

            yield return new WaitForSeconds(time);
            lerpValue += time;
            int value = (int)Mathf.Lerp(oldValue, newValue, lerpValue);

            text.text = value.ToString();

            if (lerpValue >= 1.0f)
            {
                yield break;
            }
        }
    }

    }