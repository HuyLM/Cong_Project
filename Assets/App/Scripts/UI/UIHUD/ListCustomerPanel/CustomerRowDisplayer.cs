using AtoLib;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AtoLib.Helper;
using System;

public class CustomerRowDisplayer : ViewDisplayer<NewCustomer>
{
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtModifier;
    [SerializeField] private TextMeshProUGUI txtPaid;
    [SerializeField] private TextMeshProUGUI txtDebt;
    [SerializeField] private TextMeshProUGUI txtSum;
    [SerializeField] private ButtonBase btnSelect;

    private Action<CustomerRowDisplayer> onSelect;

    private void Start()
    {
        btnSelect.onClick.AddListener(OnSelectButtonClicked);
    }

    public override void Show()
    {
        if(Model == null)
        {
            return;
        }
        txtName.color = GlobalResouces.Instance.UIConfigResource.GetBillColor(Model.IsDone ? BillState.Done : BillState.Debt);
        txtName.text = Model.CustomerName;
        txtModifier.text = Model.LastModifiedDate.ToString("dd/MM/yyyy HH:mm");
        txtPaid.text = StringHelper.GetCommaCurrencyFormat(Model.Paid);
        txtDebt.text = StringHelper.GetCommaCurrencyFormat(Model.Debt);
        txtSum.text = StringHelper.GetCommaCurrencyFormat(Model.TotalPrice);
    }

    public CustomerRowDisplayer AddOnSelect(Action<CustomerRowDisplayer> onSelect)
    {
        this.onSelect = onSelect;
        return this;
    }

    private void OnSelectButtonClicked()
    {
        onSelect?.Invoke(this);
    }
}
