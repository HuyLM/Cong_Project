using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AtoLib;
using System;

public class OptionSelectionDisplayer : ViewDisplayer<OptionSelect>
{
    [SerializeField] private TextMeshProUGUI txtText;
    [SerializeField] private ButtonBase btnSelect;

    private Action<OptionSelectionDisplayer> onSelect;

    protected void Start()
    {
        btnSelect.onClick.AddListener(OnSelectButtonClicked);
    }

    public override void Show()
    {
        if(Model == null)
        {
            return;
        }
        txtText.text = Model.Text;
    }

    private void OnSelectButtonClicked()
    {
        onSelect?.Invoke(this);
        if (Model != null)
        {
            Model.OnSelect?.Invoke();
        }
    }

    public OptionSelectionDisplayer SetOnSelect(Action<OptionSelectionDisplayer> onSelect)
    {
        this.onSelect = onSelect;
        return this;
    }
}
