using AtoLib;
using AtoLib.UI;
using System;
using UnityEngine;

public class MainMenuPanel : DOTweenFrame
{
    [SerializeField] private ButtonBase btnList;


    protected override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow( onCompleted, instant );

    }

    private void Start()
    {
        btnList.onClick.AddListener( OnListButtonClicked );
    }

    private void OnListButtonClicked()
    {
        Hide();
        //UIHUD.Instance.Show<GameplayPanel>();
    }
}
