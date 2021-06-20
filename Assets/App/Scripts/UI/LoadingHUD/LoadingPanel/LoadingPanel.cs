using AtoLib.UI;
using System;
using UnityEngine;

public class LoadingPanel : DOTweenFrame
{
    [SerializeField] private DOTweenTransition tween;
    private int numberCall = 0;
    protected override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow(onCompleted, instant);
        if (numberCall == 0)
        {
            HUDManager.IgnoreUserInput(true);
            tween.DoTransition(null, true);
        }
        numberCall++;
    }

    protected override void OnHide(Action onCompleted = null, bool instant = false)
    {
        numberCall--;
        if (numberCall > 0)
        {
            return;
        }
        HUDManager.IgnoreUserInput(false);
        tween.Stop();
        base.OnHide(onCompleted, instant);

    }
}
