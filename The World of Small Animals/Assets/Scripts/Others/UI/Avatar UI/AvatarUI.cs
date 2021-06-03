using System.Collections;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Image))]
public class AvatarUI : MonoBehaviour, ISeterSprite
    {

    private const string PATH_CHARACTER_LIST_DATA = "Data/Character/CharacterList";


    private const int DEFAULT_CAMERA_ANGLE_INDEX = 5;


    private CharactersList characterList;

    private Image image;

    private CharacterCameraAngles characterCameraAngles;
    // Use this for initialization
    void Start()
        {

        }


    private void Ini ()
    {

        if (image == null)
        {
            if (!TryGetComponent(out image))
            {
                throw new AvatarUIException($"avatar {name} not have component Image");
            }
        }

        if (characterList == null)
        {
            characterList = Resources.Load<CharactersList>(PATH_CHARACTER_LIST_DATA);

            if (characterList == null)
            {
                throw new AvatarUIException("character list not found");
            }
        }
    }

    public void SetIndexCharacter (int index)
    {
        Ini();
        if (index < 0)
        {
            throw new AvatarUIException("index argument invalid");
        }

        try
        {
            characterCameraAngles = characterList.GetCharacter(index).CharacterCameraAnglesSettings;
            SetCameraAngle(DEFAULT_CAMERA_ANGLE_INDEX);
        }
        catch (AvatarUIException)
        {

            throw;
        }
    }

    public void SetCameraAngle (int index)
    {

       if (!characterCameraAngles)
        {
            throw new AvatarUIException("character camera angles not seted");
        }

        Ini();

        SetSprite(characterCameraAngles.CameraAngles[index]);
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;
    }
}