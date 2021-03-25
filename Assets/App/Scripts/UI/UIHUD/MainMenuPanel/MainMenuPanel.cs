using AtoLib;
using AtoLib.UI;
using System;
using UnityEngine;

public class MainMenuPanel : DOTweenFrame {
    [SerializeField] private ButtonBase btnList;
    [SerializeField] private ButtonBase btnSearch;


    protected override void OnShow(Action onCompleted = null, bool instant = false) {
        base.OnShow(onCompleted, instant);

    }

    private void Start() {
        btnList.onClick.AddListener(OnListButtonClicked);
        btnSearch.onClick.AddListener(OnSearchButtonClicked);
    }

    private void OnListButtonClicked() {
        UIHUD.Instance.Show<ListBillPanel>();
    }

    private void OnSearchButtonClicked() {
        UIHUD.Instance.Show<SearchPanel>();
    }
}
