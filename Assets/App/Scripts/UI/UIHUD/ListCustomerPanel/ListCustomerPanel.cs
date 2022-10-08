using AtoLib;
using AtoLib.Helper;
using AtoLib.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;
using System.Linq;

public class ListCustomerPanel : DOTweenFrame
{
    [SerializeField] private AutoCompleteComboBox cbName;
    [SerializeField] private CustomerRowCollector customerRowCollector;
    [SerializeField] private ButtonBase btnBack;
    [SerializeField] private TextMeshProUGUI txtTotalPrice;
    [SerializeField] private TextMeshProUGUI txtTotalPaid;
    [SerializeField] private TextMeshProUGUI txtTotalDebt;
    [SerializeField] private ButtonBase btnOption;

    private string curSearchName;
    private List<NewCustomer> results;

    private OptionSelect[] options;

    private void Start()
    {
        results = new List<NewCustomer>();
        btnBack.onClick.AddListener(OnBackButtonClicked);
        cbName.OnSelectionChanged.AddListener(OnNameSelectionChanged);
        btnOption.onClick.AddListener(OnOptionSelectButtonClicked);

        options = new OptionSelect[] {
            new OptionSelect(){ Text = "Thêm mới",  OnSelect = AddNew},
            new OptionSelect(){ Text = "Đổi mật khẩu",  OnSelect = ChangePassword},
            new OptionSelect(){ Id = "showModeOption", Text =  "Show Mode",  OnSelect = ChangeShowMode},
        };
        LoadShowModeOption();
    }
    private void LoadShowModeOption()
    {
        OptionSelect optionSelect = options.FirstOrDefault<OptionSelect>(o => o.Id.Equals("showModeOption"));

        int showMode = GameData.Instance.ShowMode;
        string showModeText = "Show mode text";
        if (showMode == 0)
        {
            showModeText = "Chỉ hiện đang nợ";
        }
        else if (showMode == 1)
        {
            showModeText = "Hiện tất cả";
        }
        optionSelect.Text = showModeText;
    }


    public override Frame OnBack()
    {
        return this;
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

    private void OpenListBill(NewCustomer customer, bool isAddNew)
    {
        //Hide();
        Pause();

        Pause();
        ListBillPanel listBillPanel = UIHUD.Instance.GetFrame<ListBillPanel>();
        listBillPanel.SetCustomer(customer, isAddNew);
        UIHUD.Instance.Show<ListBillPanel>();
    }

    private void ShowCustomers()
    {
        List<NewCustomer> customers = GameData.Instance.Customers;
        results = new List<NewCustomer>();
        foreach (var b in customers)
        {
            if (GameData.Instance.ShowMode == 1 && b.Debt <= 0)
            {
                continue;
            }
            if (string.IsNullOrEmpty(curSearchName) || b.CustomerName.Equals(curSearchName) || curSearchName.Equals("all"))
            {
                results.Add(b);
            }
        }
        customerRowCollector.AddOnSelect(OnSelectCustomerRow).SetCapacity(results.Count).SetItems(results).Show();
    }

    private void OnSelectCustomerRow(CustomerRowDisplayer displayer)
    {
        OpenListBill(displayer.Model, false);
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
    #region Options
    private void AddNew()
    {
        NewCustomer newCustomer = new NewCustomer();
        OpenListBill(newCustomer, true);
    }

    private void ChangePassword()
    {
        PopupHUD.Instance.Show<ChangePasswordPopup>();
    }

    private void ChangeShowMode()
    {
        if (GameData.Instance.ShowMode == 0)
        {
            // to do: chi hien dang no
            GameData.Instance.ShowMode = 1;
        }
        else if (GameData.Instance.ShowMode == 1)
        {
            // to do: hien tat
            GameData.Instance.ShowMode = 0;
        }
        LoadShowModeOption();
        ShowCustomers();

    }
    #endregion
}
