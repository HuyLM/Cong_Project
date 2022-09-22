using AtoLib;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseDateTimePopup : BasePopup
{
    [SerializeField] private DatePickerControl datePicker;
    [SerializeField] private ButtonBase btnConfirm;
    [SerializeField] private ButtonBase btnCancel;

    private Action<string, DateTime> onConfirm;
    private Action onCancel;
    private DateTime setDatetime;

    protected override void Start()
    {
        base.Start();
        btnConfirm.onClick.AddListener(OnConfirmButtonClicked);
        btnCancel.onClick.AddListener(OnCancelButtonClicked);
    }

    protected override void OnShowAnimationCompleted()
    {
        base.OnShowAnimationCompleted();
        datePicker.SetDateTime(setDatetime);
    }

    protected override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow(onCompleted, instant);
        setDatetime = DateTime.Now;
    }

    public ChooseDateTimePopup SetDateTime(DateTime dateTime)
    {
        setDatetime = dateTime;
        return this;
    }

    public ChooseDateTimePopup SetOnConfirm(Action<string, DateTime> onConfirm)
    {
        this.onConfirm = onConfirm;
        return this;
    }

    public ChooseDateTimePopup SetOnCancel(Action onCancel)
    {
        this.onCancel = onCancel;
        return this;
    }

    private void OnConfirmButtonClicked()
    {
        onConfirm?.Invoke(DatePickerControl.dateStringFormato, DatePickerControl.DateGlobal);
        Close();
    }

    private void OnCancelButtonClicked()
    {
        onCancel?.Invoke();
        Close();
    }
}
