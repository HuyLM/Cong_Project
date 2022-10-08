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
    [SerializeField] private ButtonBase btnOption;

    private NewCustomer curCustomer;
    private NewCustomer originCustomer;
    private bool isAddNew;
    private bool canDelete;

    private OptionSelect[] options;

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
        btnOption.onClick.AddListener(OnOptionSelectButtonClicked);

        options = new OptionSelect[] {
            new OptionSelect(){ Text = "Thêm mới",  OnSelect = AddNew},
            new OptionSelect(){ Text = "Sửa thanh toán",  OnSelect = EditPaid},
            new OptionSelect(){ Text = "Thêm thanh toán mới",  OnSelect = AddPaid},
            new OptionSelect(){ Text = "Bỏ thay đổi",  OnSelect = Undo},
            new OptionSelect(){ Text = "Lưu",  OnSelect = Save},
            new OptionSelect(){ Text = "Xóa",  OnSelect = Delete},
        };
    }

    public void Refresh()
    {
        Show();
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
        canDelete = !isAddNew;
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
    private async void OnSaveButtonClicked()
    {
        Save();
    }

    private void OnBackButtonClicked()
    {
        OnBack();
    }

    private void OnOptionSelectButtonClicked()
    {
        OptionPopup optionPopup = PopupHUD.Instance.GetFrame<OptionPopup>();
        optionPopup.SetOptions(options);
        PopupHUD.Instance.Show<OptionPopup>();
    }

    private void OnNameInputEndEdit(string text)
    {
        curCustomer.SetName(text);
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

    #region Options

    private void AddNew()
    {
        AddNewBill();
    }

    private void EditPaid()
    {
        InputNumberPopup inputNumberPopup = PopupHUD.Instance.Show<InputNumberPopup>();
        inputNumberPopup.SetTile("Sửa thanh toán");
        inputNumberPopup.SetNumber(curCustomer.Paid, 0, curCustomer.TotalPrice);
        inputNumberPopup.SetOnConfirm(() => {
            curCustomer.SetPaid(inputNumberPopup.number);
            Refresh();
        });
    }

    private void AddPaid()
    {
        InputNumberPopup inputNumberPopup = PopupHUD.Instance.Show<InputNumberPopup>();
        inputNumberPopup.SetTile("Thêm thanh toán mới");
        inputNumberPopup.SetNumber(curCustomer.Debt, 0, curCustomer.Debt);
        inputNumberPopup.SetOnConfirm(() => {
            curCustomer.AddPaid(inputNumberPopup.number);
            Refresh();
        });
    }

    private void Undo()
    {
        NewCustomer.Transmission(originCustomer, curCustomer);
        Refresh();
    }

    private async void Save()
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

    private void Delete()
    {
        if (canDelete)
        {
            ConfirmPopup confirmPopup = PopupHUD.Instance.Show<ConfirmPopup>();
            confirmPopup.SetTile(null, false);
            confirmPopup.SetMessage("Bạn có muốn xoá không?");
            confirmPopup.SetConfirmText("Xoá");
            confirmPopup.SetCancelText("Thoát");
            confirmPopup.SetOnConfirm( async () =>
            {
                GameData.Instance.Customers.Remove(originCustomer);
                await GameSaveData.Instance.SaveData(true);
                Hide();
            });
        }
        else
        {
            Hide();
        }
    }

    #endregion
}
