using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class WindowSelectSmiles : MiniWindow
    {
    private const string PATH_SMILES_DATA = "Data/UI/Smiles/SmileContainer";

    private const string PATH_PREFAB_SMILE_UI_BUTTON = "Prefabs/UI/smile_button_ui";

    private SmileButtonUI smileButtonUIPrefab;

    private SmileContainer smileContainer;


    [Header("Контейнер смайликов")]
    [SerializeField] Transform contentSmiles;

    [Header("Задержка появления кнопки смайла")]
    [SerializeField] float timeOutShowbuttonSmile = 0.3f;

    [Header("Задержка появления кнопки смайла")]
    [SerializeField] float timeOutShowSmiles = 0.4f;

    // Use this for initialization
    void Awake()
    {

        Ini();

        Exit();
    }

    private void Exit()
    {
        SetActiveSelfGameObject(gameObject, false);
    }

    private void OnEnable()
    {

        if (contentSmiles == null)
        {
            throw new WindowSelectSmilesException("content smiles not seted");
        }



        if (smileContainer == null)
        {
            smileContainer = Resources.Load<SmileContainer>(PATH_SMILES_DATA);

            if (smileContainer == null)
            {
                throw new WindowSelectSmilesException("smile container not found");
            }
        }

        if (smileButtonUIPrefab == null)
        {
            smileButtonUIPrefab = Resources.Load<SmileButtonUI>(PATH_PREFAB_SMILE_UI_BUTTON);

            if (smileButtonUIPrefab == null)
            {
                throw new WindowSelectSmilesException("smile button ui prefab not found");
            }
        }

        StartCoroutine(ShowSmilesButtons());
    }

    private IEnumerator ShowSmilesButtons ()
    {
        if (contentSmiles.childCount < smileContainer.SmilesSprites.Length)
        {
            RemoveAllSmilesButtons();
        }

        else
        {
            for (int i = 0; i < contentSmiles.childCount; i++)
            {
                SetActiveSelfSmileButton(i, false);
            }
        }

        yield return new WaitForSeconds(timeOutShowSmiles);

        if (contentSmiles.childCount == 0)
        {

            Sprite[] arraySmiles = smileContainer.SmilesSprites;
            for (int i = 0; i < arraySmiles.Length; i++)
            {
                yield return new WaitForSeconds(timeOutShowbuttonSmile);

                SmileButtonUI newButton = Instantiate(smileButtonUIPrefab, contentSmiles);


                newButton.SetListSprites(arraySmiles);
                newButton.SetIndexSprite(i);
                newButton.SetSprite(arraySmiles[i]);
            }
        }

        else if (contentSmiles.childCount == smileContainer.SmilesSprites.Length)
        {

            for (int i = 0; i < contentSmiles.childCount; i++)
            {
                yield return new WaitForSeconds(timeOutShowbuttonSmile);
                SetActiveSelfSmileButton(i, true);
            }
        }
    }


    private void SetActiveSelfSmileButton (int index, bool activeState)
    {
        SetActiveSelfGameObject(contentSmiles.GetChild(index).gameObject, activeState);
    }

    private void RemoveAllSmilesButtons ()
    {
        for (int i = 0; i < contentSmiles.childCount; i++)
        {
            GameObject smile = contentSmiles.GetChild(i).gameObject;
            Destroy(smile);
        }
    }

}