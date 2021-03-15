using AtoLib;
using AtoLib.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ListBillPanel : DOTweenFrame
{
    [SerializeField] private BillRowCollector billRowCollector;
    [SerializeField] private ButtonBase btnAddBill;

    protected override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow( onCompleted, instant );
        ShowBills();
    }

    private void Start()
    {
        btnAddBill.onClick.AddListener(OnAddBillButtonClicked);
    }

    private void AddNewBill()
    {
        Bill newBill = new Bill();
        BillList.Instance.Bills.Add( newBill );
        OpenEditBillPanel(newBill);
    }

    private void OpenEditBillPanel(Bill bill)
    {
        Hide();
        EditBillPanel editBillPanel = UIHUD.Instance.GetFrame<EditBillPanel>();
        editBillPanel.SetOpenBill( bill );
        UIHUD.Instance.Show<EditBillPanel>();
    }

    private void ShowBills()
    {
        List<Bill> bills = BillList.Instance.Bills;
        billRowCollector.AddOnSelect(OnSelectBillRow).SetCapacity( bills.Count ).SetItems( bills ).Show();
    }

    private void OnSelectBillRow(BillRowViewDisplayer displayer)
    {
        OpenEditBillPanel( displayer.Model );
    }

    private void OnAddBillButtonClicked()
    {
        AddNewBill();
    }
}
