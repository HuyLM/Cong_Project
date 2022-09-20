using AtoLib;
using AtoLib.UI;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AtoLib.Helper;

public class ListBillPanel : DOTweenFrame
{
    [SerializeField] private TextMeshProUGUI txtModifiedDate; 
    [SerializeField] private TMP_InputField ipName;
    [SerializeField] private BillRowCollector billRowCollector;
    [SerializeField] private ButtonBase btnSave;
    [SerializeField] private ButtonBase btnBack;
    [SerializeField] private TextMeshProUGUI txtTotalPrice;
    [SerializeField] private TextMeshProUGUI txtPaid;
    [SerializeField] private TextMeshProUGUI txtDebt;

    private NewCustomer curCustomer;
    private NewCustomer originCustomer;
    private bool isAddNew;

    protected override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow(onCompleted, instant);
        Show();
    }

    protected override void OnResume(Action onCompleted = null, bool instant = false)
    {
        base.OnResume(onCompleted, instant);
        Show();
    }

    private void Start()
    {
        btnSave.onClick.AddListener(OnSaveButtonClicked);
        btnBack.onClick.AddListener(OnBackButtonClicked);
        ipName.onEndEdit.AddListener(OnNameInputEndEdit);
    }

    private void Show()
    {
        ShowBills();
        txtModifiedDate.text = "Ngày sửa: " + curCustomer.LastModifiedDate.ToString("dd/MM/yyyy HH:mm");
        ipName.SetTextWithoutNotify(curCustomer.CustomerName);
        ShowTotalPrice();
        ShowPaidText();
        ShowDebtText();
    }

    private void AddNewBill()
    {
        NewBill newBill = new NewBill();
        newBill.SetCustomer(curCustomer);
        OpenEditBillPanel(newBill, true);
    }

    private void OpenEditBillPanel(NewBill bill, bool isAddNew)
    {
        //Hide();
        Pause();
        EditBillPanel editBillPanel = UIHUD.Instance.GetFrame<EditBillPanel>();
        editBillPanel.SetOpenBill(bill, isAddNew);
        UIHUD.Instance.Show<EditBillPanel>();
    }

    public void SetCustomer(NewCustomer customer, bool isAddNew)
    {
        this.isAddNew = isAddNew;
        originCustomer = customer;
        curCustomer = new NewCustomer();
        NewCustomer.Transmission(originCustomer, curCustomer);
    }

    private void ShowBills()
    {
        billRowCollector.AddOnSelect(OnSelectBillRow).SetCapacity(curCustomer.Bills.Count).SetItems(curCustomer.Bills).Show();
    }

    private void OnSelectBillRow(BillRowViewDisplayer displayer)
    {
        OpenEditBillPanel(displayer.Model, false);
    }

    private void OnAddBillButtonClicked()
    {
        AddNewBill();
    }

    private async void OnSaveButtonClicked()
    {
        if (curCustomer == null)
        {
            curCustomer = new NewCustomer();
        }
        NewCustomer.Transmission(curCustomer, originCustomer);
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
        }
        else
        {
            if (isAddNew)
            {
                GameData.Instance.Customers.Add(curCustomer);
            }
            await GameSaveData.Instance.SaveData(true);
            Hide();
        }
    }

    private void OnBackButtonClicked()
    {
        OnBack();
    }

    private void OnNameInputEndEdit(string text)
    {
        curCustomer.CustomerName = text;
    }

    #region Total, Debt, Paid
    private void ShowTotalPrice()
    {
        txtTotalPrice.text = StringHelper.GetCommaCurrencyFormat(curCustomer.TotalPrice);
    }

    private void ShowPaidText()
    {
        txtPaid.text = StringHelper.GetCommaCurrencyFormat(curCustomer.Paid);
    }

    private void ShowDebtText()
    {
        txtDebt.text = StringHelper.GetCommaCurrencyFormat(curCustomer.Debt);
    }

    #endregion
}
