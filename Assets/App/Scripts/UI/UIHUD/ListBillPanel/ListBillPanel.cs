using AtoLib;
using AtoLib.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class ListBillPanel : DOTweenFrame {
    [SerializeField] private BillRowCollector billRowCollector;
    [SerializeField] private ButtonBase btnAddBill;
    [SerializeField] private ButtonBase btnBack;

    protected override void OnShow(Action onCompleted = null, bool instant = false) {
        base.OnShow(onCompleted, instant);
        ShowBills();
    }

    protected override void OnResume(Action onCompleted = null, bool instant = false) {
        base.OnResume(onCompleted, instant);
        ShowBills();
    }

    private void Start() {
        btnAddBill.onClick.AddListener(OnAddBillButtonClicked);
        btnBack.onClick.AddListener(OnBackButtonClicked);
    }

    private void AddNewBill() {
        Bill newBill = new Bill();
        BillList.Instance.Bills.Add(newBill);
        OpenEditBillPanel(newBill);
    }

    private void OpenEditBillPanel(Bill bill) {
        //Hide();
        Pause();
        EditBillPanel editBillPanel = UIHUD.Instance.GetFrame<EditBillPanel>();
        editBillPanel.SetOpenBill(bill);
        UIHUD.Instance.Show<EditBillPanel>();
    }

    private void ShowBills() {
        List<Bill> bills = BillList.Instance.Bills;
        billRowCollector.AddOnSelect(OnSelectBillRow).SetCapacity(bills.Count).SetItems(bills).Show();
    }

    private void OnSelectBillRow(BillRowViewDisplayer displayer) {
        OpenEditBillPanel(displayer.Model);
    }

    private void OnAddBillButtonClicked() {
        AddNewBill();
    }
    private void OnBackButtonClicked() {
        OnBack();
    }
}
