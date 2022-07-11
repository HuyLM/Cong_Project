using AtoLib;
using AtoLib.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using TMPro;
using AtoLib.Helper;

public class SearchPanel : DOTweenFrame
{
    [SerializeField] private AutoCompleteComboBox cbName;
    [SerializeField] private BillRowCollector billRowCollector;
    [SerializeField] private ButtonBase btnBack;
    [SerializeField] private TextMeshProUGUI txtTotalPrice;
    [SerializeField] private TextMeshProUGUI txtTotalPaid;
    [SerializeField] private TextMeshProUGUI txtTotalDebt;

    private string curSearchName;
    private List<Bill> results;

    protected override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow(onCompleted, instant);
        curSearchName = string.Empty;
        cbName.InputComponent.text = string.Empty;

        ShowBills();
        ShowNameCombox();
    }

    protected override void OnResume(Action onCompleted = null, bool instant = false)
    {
        base.OnResume(onCompleted, instant);
        ShowBills();
        ShowNameCombox();
    }

    private void Start()
    {
        results = new List<Bill>();
        btnBack.onClick.AddListener(OnBackButtonClicked);
        cbName.OnSelectionChanged.AddListener(OnNameSelectionChanged);
    }

    private void ShowNameCombox()
    {
        List<Bill> bills = BillList.Instance.Bills;
        List<string> allName = new List<string>();
        foreach (var b in bills)
        {
            if (!allName.Contains(b.CustomerName))
            {
                allName.Add(b.CustomerName);
            }
        }
        cbName.SetAvailableOptions(allName);
        cbName.ItemsToDisplay = 5;

    }


    private void OpenEditBillPanel(Bill bill)
    {
        //Hide();
        Pause();
        EditBillPanel editBillPanel = UIHUD.Instance.GetFrame<EditBillPanel>();
        editBillPanel.SetOpenBill(bill, false);
        UIHUD.Instance.Show<EditBillPanel>();
    }

    private void ShowBills()
    {
        List<Bill> bills = BillList.Instance.Bills;
        results = new List<Bill>();
        foreach (var b in bills)
        {
            if (b.CustomerName.Equals(curSearchName) || curSearchName.Equals("all"))
            {
                results.Add(b);
            }
        }
        billRowCollector.AddOnSelect(OnSelectBillRow).SetCapacity(results.Count).SetItems(results).Show();
        ShowTotalPrice();
        ShowTotalPaid();
        ShowTotalDebt();
    }

    private void OnSelectBillRow(BillRowViewDisplayer displayer)
    {
        OpenEditBillPanel(displayer.Model);
    }

    private void OnBackButtonClicked()
    {
        OnBack();
    }

    private void OnNameSelectionChanged(string text, bool isSelect)
    {
        curSearchName = text;
        if (isSelect)
        {
            ShowBills();
        }
    }

    private void ShowTotalPrice()
    {
        int total = 0;
        foreach (var b in results)
        {
            total += b.TotalPrice;
        }
        txtTotalPrice.text = StringHelper.GetCommaCurrencyFormat(total);
    }

    private void ShowTotalPaid()
    {
        int total = 0;
        foreach (var b in results)
        {
            total += b.Paid;
        }
        txtTotalPaid.text = StringHelper.GetCommaCurrencyFormat(total);
    }

    private void ShowTotalDebt()
    {
        int total = 0;
        foreach (var b in results)
        {
            total += b.Debt;
        }
        txtTotalDebt.text = StringHelper.GetCommaCurrencyFormat(total);
    }
}
