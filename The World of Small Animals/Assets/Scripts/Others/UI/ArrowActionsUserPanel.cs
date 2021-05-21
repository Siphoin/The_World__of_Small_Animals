using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Button))]
public class ArrowActionsUserPanel : MonoBehaviour
    {

    private const string NAME_ANIMATION_ARROW_SHOW = "arrow_snow";

    private const string NAME_ANIMATION_ARROW_HIDE = "arrow_hide";

    private const string NAME_ANIMATION_USER_PANEL_HIDE = "user_panel_hde";

    private const string NAME_ANIMATION_USER_PANEL_SNOW = "user_panel_snow";

    [Header("Панель пользователя")]
    [SerializeField] private Animator userPanel;

    private Animator animatorArrow;

    private Button button;

    private bool panelShowed = true;


        // Use this for initialization
        void Start()
        {
        if (userPanel == null)
        {
            throw new ArrowActionsUserPanelException("user panel not seted");
        }

        if (!TryGetComponent(out animatorArrow))
        {
            throw new ArrowActionsUserPanelException("animator arrow not seted");
        }

        if (!TryGetComponent(out button))
        {
            throw new ArrowActionsUserPanelException("button not seted");
        }

        button.onClick.AddListener(SetStateShowUserPanel);
    }

    private void SetStateShowUserPanel()
    {
        panelShowed = !panelShowed;

        animatorArrow.Play(panelShowed == true ? NAME_ANIMATION_ARROW_SHOW : NAME_ANIMATION_ARROW_HIDE);

        userPanel.Play(panelShowed == true ? NAME_ANIMATION_USER_PANEL_SNOW : NAME_ANIMATION_USER_PANEL_HIDE);
    }
}