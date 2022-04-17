using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class ButtonPageBanner : MonoBehaviour, ISeterColor
    {
    [Header("Цвет, при котором страница баннера соотвествует позиции кнопки")]
    [SerializeField] Color colorActive = Color.white;

    private Color defaultColor;


    private Button button;

    private Image image;



    public event Action<int> onSelect;

    private int currentIndexPage = -1;
    

    private void Ini ()
    {



        if (button == null)
        {
            if (!TryGetComponent(out button))
            {
                throw new ButtonPageBannerException($"{name} not have component Button");
            }

            button.onClick.AddListener(Select);
        }

        if (image == null)
        {
            if (!TryGetComponent(out image))
            {
                throw new ButtonPageBannerException($"{name} not have component Image");
            }
        }

        if (defaultColor == new Color())
        {
            defaultColor = image.color;
        }
    }

    private void Select()
    {

        if (currentIndexPage == -1)
        {
            throw new ButtonPageBannerException($"button page {name} not seted index page");
        }
        onSelect?.Invoke(currentIndexPage);

        

    }

    public void SetStateColor (int index)
    {
        Color color = index == currentIndexPage ? colorActive : defaultColor;
        SetColor(color);
    }

    public void SetIndexPage (int index)
    {

        Ini();


        if (index < 0)
        {
            throw new ButtonPageBannerException("not valid index");
        }

        currentIndexPage = index;
    }

    public void SetColor(Color color)
    {
        Ini();
        button.interactable = color != colorActive;
        image.color = color;
    }
    
          private  void Start() =>  Ini();
}
