using UnityEngine;


[CreateAssetMenu(menuName = "Data/Customize/Color Variants", order = 0)]
public class ColorVariants : ScriptableObject
    {
    [Header("Варианты цветов")]
    [SerializeField] private Color[] colors;

    public Color[] Colors { get => colors; }

    public Color GetColorWithIndex (int index)
    {
        if (index < 0)
        {
            throw new ColorVariantsException("index value not valid");
        }

        if (index > colors.Length)
        {
            throw new ColorVariantsException("index value not valid");
        }

        return colors[index];
    }
}