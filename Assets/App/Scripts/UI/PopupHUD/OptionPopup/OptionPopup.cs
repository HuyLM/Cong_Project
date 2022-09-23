using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionPopup : BasePopup
{
    [SerializeField] private OptionSelectionCollector optionSelectionCollector;

    private OptionSelect[] options;

    protected override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow(onCompleted, instant);
        optionSelectionCollector.AddOnSelect(OnOptionSelect).SetCapacity(options.Length).SetItems(options).Show();
    }

    public void SetOptions(OptionSelect[] options)
    {
        this.options = options;
    }

    private void OnOptionSelect(OptionSelectionDisplayer displayer)
    {
        Hide();
    }
}
