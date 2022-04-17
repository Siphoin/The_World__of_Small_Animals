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
    [SerializeField] private Animator _userPanel;

    private Animator _animatorArrow;

    private Button _button;

    private bool _panelShowed = true;


      private void Start()
        {
        if (_userPanel == null)
        {
            throw new ArrowActionsUserPanelException("user panel not seted");
        }

        if (!TryGetComponent(out _animatorArrow))
        {
            throw new ArrowActionsUserPanelException("animator arrow not seted");
        }

        if (!TryGetComponent(out _button))
        {
            throw new ArrowActionsUserPanelException("button not seted");
        }

        _button.onClick.AddListener(SetStateShowUserPanel);
    }

    private void SetStateShowUserPanel()
    {
        _panelShowed = !_panelShowed;

        _animatorArrow.Play(_panelShowed == true ? NAME_ANIMATION_ARROW_SHOW : NAME_ANIMATION_ARROW_HIDE);

        _userPanel.Play(_panelShowed == true ? NAME_ANIMATION_USER_PANEL_SNOW : NAME_ANIMATION_USER_PANEL_HIDE);
    }
}
