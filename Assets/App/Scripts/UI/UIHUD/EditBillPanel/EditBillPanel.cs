using AtoLib.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using TMPro;
using AtoLib;
using AtoLib.Helper;

public class EditBillPanel : DOTweenFrame
{
    [SerializeField] private ProductRowCollector productRowCollector;
    [SerializeField] private TextMeshProUGUI txtDate;
    [SerializeField] private AutoCompleteComboBox cbName;
    [SerializeField] private DropDownList ddState;
    [SerializeField] private TextMeshProUGUI txtTotalPrice;
    [SerializeField] private TMP_InputField ipPaid;
    [SerializeField] private TextMeshProUGUI txtDebt;
    [SerializeField] private ButtonBase btnSave;
    [SerializeField] private ButtonBase btnCancel;
    [SerializeField] private ButtonBase btnUndo;
    [SerializeField] private ButtonBase btnDel;
    [SerializeField] private ButtonBase btnAddProduct;

    private Bill curBill;
    private Bill originBill;
    private bool isAddNew;

    private void Start()
    {
        cbName.OnSelectionChanged.AddListener(OnNameSelectionChanged);
        ddState.OnSelectionChanged.AddListener(OnStateSelected);
        btnSave.onClick.AddListener(OnSaveButtonClicked);
        btnCancel.onClick.AddListener(OnCancelButtonClicked);
        btnUndo.onClick.AddListener(OnUndoButtonClicked);
        btnAddProduct.onClick.AddListener(OnAddProductButtonClicked);
        btnDel.onClick.AddListener(OnDeleteButtonClicked);
        ipPaid.onEndEdit.AddListener(OnPaidEndEdit);
        ipPaid.onValueChanged.AddListener(OnPaidOnValueChanged);
    }
    protected override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow(onCompleted, instant);
        ShowUI();
    }

    protected override void OnResume(Action onCompleted = null, bool instant = false)
    {
        base.OnResume(onCompleted, instant);
        ShowUI();
    }

    public void Refresh()
    {
        ShowUI();
    }

    private void ShowUI()
    {
        ShowDate();
        ShowProducts();
        ShowNameCombox();
        SetDropDownListState();
        ShowTotalPrice();
        ShowPaidText();
        ShowDebtText();
        StartCoroutine(IDelayAFrame());
    }

    private IEnumerator IDelayAFrame()
    {
        yield return null;
        ShowState();
    }

    public void SetOpenBill(Bill bill, bool isAddNew)
    {
        this.isAddNew = isAddNew;
        this.originBill = bill;
        btnDel.SetState(!isAddNew);
        if (curBill == null)
        {
            curBill = new Bill();
        }
        Bill.Transmission(originBill, curBill);
    }

    #region Products

    private void OpenEditProductPopup(Product product, bool isAddNewProduct)
    {
        Pause();
        PopupHUD.Instance.GetFrame<EditProductPopup>().SetOpenProduct(product, isAddNewProduct);
        PopupHUD.Instance.Show<EditProductPopup>();
    }

    private void ShowProducts()
    {
        List<Product> products = curBill.Products;
        productRowCollector.AddOnSelect(OnSelectProductRow).SetCapacity(products.Count).SetItems(products).Show();
    }
    private void OnSelectProductRow(ProductRowViewDisplayer displayer)
    {
        OpenEditProductPopup(displayer.Model, false);
    }

    private void OnAddProductButtonClicked()
    {
        Product newProduct = new Product(curBill);
        //curBill.AddProduct(newProduct);
        OpenEditProductPopup(newProduct, true);
    }

    #endregion

    #region Date
    private void ShowDate()
    {
        txtDate.text = curBill.Date.ToString("dd/MM/yyyy");
    }

    #endregion

    #region Combox Name
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
        cbName.InputComponent.text = curBill.CustomerName;
        cbName.SetAvailableOptions(allName);
        cbName.ItemsToDisplay = 5;

    }

    private void OnNameSelectionChanged(string text, bool isSelect)
    {
        curBill.CustomerName = text;
    }

    #endregion

    #region dropdownlist state
    private void SetDropDownListState()
    {
        ddState.Items = GlobalResouces.Instance.UIConfigResource.GetListItemStateBill(ddState.Items, curBill.State);
    }
    private void ShowState()
    {
        ddState.OnItemForceSelect((int)curBill.State);
    }

    private void OnStateSelected(int itemIdx)
    {
        curBill.ChangeState((BillState)itemIdx);
        //curBill.ChangeState( (BillState)ddState.SelectedItem.ID ); other
    }
    #endregion



    #region Buttons
    private async void OnSaveButtonClicked()
    {
        if (curBill == null)
        {
            curBill = new Bill();
        }
        Bill.Transmission(curBill, originBill);
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("Error. Check internet connection!");
        }
        else
        {
            if (isAddNew)
            {
                BillList.Instance.Bills.Add(originBill);
            }
            await GameSaveData.Instance.SaveData(true);
            Hide();
        }
    }

    private void OnCancelButtonClicked()
    {
        Hide();
    }

    private void OnUndoButtonClicked()
    {
        if (curBill == null)
        {
            curBill = new Bill();
        }
        Bill.Transmission(originBill, curBill);
        ShowUI();
    }

    private async void OnDeleteButtonClicked()
    {
        ConfirmPopup confirmPopup = PopupHUD.Instance.Show<ConfirmPopup>();
        confirmPopup.SetTile(null, false);
        confirmPopup.SetMessage("Bạn có muốn xoá không?");
        confirmPopup.SetConfirmText("Xoá");
        confirmPopup.SetCancelText("Thoát");
        confirmPopup.SetOnConfirm(async () =>
        {
            List<Bill> bills = BillList.Instance.Bills;
            bills.Remove(originBill);
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                Debug.Log("Error. Check internet connection!");
            }
            else
            {
                await GameSaveData.Instance.SaveData(true);
                Hide();
            }
        });
    }
    #endregion


    #region Total, Debt, Paid
    private void ShowTotalPrice()
    {
        txtTotalPrice.text = StringHelper.GetCommaCurrencyFormat(curBill.TotalPrice);
    }

    private void ShowPaidText()
    {
        ipPaid.text = StringHelper.GetCommaCurrencyFormat(curBill.Paid);
    }

    private void OnPaidEndEdit(string text)
    {
        string valText = text.Replace(",", string.Empty);
        if (string.IsNullOrEmpty(valText))
        {
            curBill.SetPaid(0);
        }
        else
        {
            curBill.SetPaid(int.Parse(valText));
        }
        ShowUI();
    }

    private void OnPaidOnValueChanged(string text)
    {
        string valText = text.Replace(",", string.Empty);
        if (string.IsNullOrEmpty(valText))
        {
            curBill.SetPaid(0);
        }
        else
        {
            int valInt = int.Parse(valText);
            valInt = Mathf.Clamp(valInt, 0, curBill.TotalPrice);
            curBill.SetPaid(valInt);
        }
        ShowUI();
        StopCoroutine(IDelyPaidMoveEnd());
        StartCoroutine(IDelyPaidMoveEnd());
    }

    private IEnumerator IDelyPaidMoveEnd()
    {
        yield return null;
        ipPaid.MoveTextEnd(false);
    }

    private void ShowDebtText()
    {
        txtDebt.text = StringHelper.GetCommaCurrencyFormat(curBill.Debt);
    }

    #endregion
}
