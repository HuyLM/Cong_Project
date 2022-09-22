using AtoLib;
using AtoLib.Helper;
using AtoLib.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class ListCustomerPanel : DOTweenFrame
{
    [SerializeField] private AutoCompleteComboBox cbName;
    [SerializeField] private CustomerRowCollector customerRowCollector;
    [SerializeField] private ButtonBase btnBack;
    [SerializeField] private TextMeshProUGUI txtTotalPrice;
    [SerializeField] private TextMeshProUGUI txtTotalPaid;
    [SerializeField] private TextMeshProUGUI txtTotalDebt;

    private string curSearchName;
    private List<NewCustomer> results;

    private void Start()
    {
        results = new List<NewCustomer>();
        btnBack.onClick.AddListener(OnBackButtonClicked);
        cbName.OnSelectionChanged.AddListener(OnNameSelectionChanged);
    }


    protected override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow(onCompleted, instant);
        curSearchName = string.Empty;
        cbName.InputComponent.text = string.Empty;

        ShowCustomers();
        ShowNameCombox();
    }

    protected override void OnResume(Action onCompleted = null, bool instant = false)
    {
        base.OnResume(onCompleted, instant);
        ShowCustomers();
        ShowNameCombox();
    }

    private void ShowNameCombox()
    {
        List<NewCustomer> customers = GameData.Instance.Customers;
        List<string> availiableNames = new List<string>();
        foreach (var b in customers)
        {
            if (!availiableNames.Contains(b.CustomerName))
            {
                availiableNames.Add(b.CustomerName);
            }
        }
        cbName.SetAvailableOptions(availiableNames);
        cbName.ItemsToDisplay = 5;

    }

    private void OpenListBill(NewCustomer customer)
    {
        //Hide();
        Pause();

        Pause();
        ListBillPanel listBillPanel = UIHUD.Instance.GetFrame<ListBillPanel>();
        listBillPanel.SetCustomer(customer, false);
        UIHUD.Instance.Show<ListBillPanel>();
    }

    private void ShowCustomers()
    {
        List<NewCustomer> customers = GameData.Instance.Customers;
        results = new List<NewCustomer>();
        foreach (var b in customers)
        {
            if (string.IsNullOrEmpty(curSearchName) || b.CustomerName.Equals(curSearchName) || curSearchName.Equals("all"))
            {
                results.Add(b);
            }
        }
        customerRowCollector.AddOnSelect(OnSelectCustomerRow).SetCapacity(results.Count).SetItems(results).Show();
    }

    private void OnSelectCustomerRow(CustomerRowDisplayer displayer)
    {
        OpenListBill(displayer.Model);
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
            ShowCustomers();
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
