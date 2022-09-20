using AtoLib;
using AtoLib.UI;
using System;
using UnityEngine;

public class MainMenuPanel : DOTweenFrame
{
    [SerializeField] private ButtonBase btnList;
    [SerializeField] private ButtonBase btnSearch;
    [SerializeField] private ButtonBase btnChangePass;


    protected override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow(onCompleted, instant);
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            btnChangePass.SetState(false);
        }
    }

    public override Frame OnBack()
    {
        return this;
    }

    private void Start()
    {
        btnList.onClick.AddListener(OnListButtonClicked);
        btnSearch.onClick.AddListener(OnSearchButtonClicked);
        btnChangePass.onClick.AddListener(OnChangePassButtonClicked);
    }


    private void OnListButtonClicked()
    {
        UIHUD.Instance.Show<ListCustomerPanel>();
    }

    private void OnSearchButtonClicked()
    {
        UIHUD.Instance.Show<SearchPanel>();
    }

    private void OnChangePassButtonClicked()
    {
        PopupHUD.Instance.Show<ChangePasswordPopup>();
    }
}
