using System.Collections;
using UnityEngine;

    public class WebFormFragment : MonoBehaviour
    {
    [Header("Ключ значения параметра для фориы")]
    [SerializeField] private string idField;

    public string Name { get => idField; }

    public virtual void Ini ()
    {
       if (string.IsNullOrEmpty(idField))
        {
            throw new WebFormFragmentException($"{name} имеет пустой ключ формы");
        }
    }



}