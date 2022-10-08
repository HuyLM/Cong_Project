using UnityEngine;
using TMPro;
using AtoLib;
using System;
using AtoLib.Helper;
using System.Collections;

public class EditProductPopup : BasePopup
{
    [SerializeField] private TMP_InputField ipName;
    [SerializeField] private TMP_InputField ipUnitPrice;
    [SerializeField] private TMP_InputField ipAmount;
    [SerializeField] private TextMeshProUGUI txtTotalPrice;
    [SerializeField] private TMP_InputField ipNote;

    [SerializeField] private ButtonBase btnSave;
    [SerializeField] private ButtonBase btnDel;
    [SerializeField] private ButtonBase btnCancel;

    private NewProduct curProduct;
    private NewProduct originProduct;
    private bool isAddNewProduct;

    protected override void Start()
    {
        base.Start();
        ipNote.onEndEdit.AddListener(OnNoteEndEdit);
        btnSave.onClick.AddListener(OnSaveButtonClicked);
        btnDel.onClick.AddListener(OnDeleteButtonClicked);
        btnCancel.onClick.AddListener(OnCancelButtonClicked);
        ipName.onEndEdit.AddListener(OnNameEndEdit);
        ipUnitPrice.onEndEdit.AddListener(OnUnitPriceEndEdit);
        ipUnitPrice.onValueChanged.AddListener(OnUnitPriceOnValueChanged);
        ipUnitPrice.onSelect.AddListener(OnUnitPriceOnSelected);
        ipAmount.onEndEdit.AddListener(OnAmountEndEdit);
        ipAmount.onValueChanged.AddListener(OnAmountOnValueChanged);
        ipAmount.onSelect.AddListener(OnAmountOnSelected);
    }

    protected override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow(onCompleted, instant);
        ShowUI();
    }

    protected override void OnHide(Action onCompleted = null, bool instant = false)
    {
        base.OnHide(onCompleted, instant);
        UIHUD.Instance.GetActiveFrame<EditBillPanel>().Refresh();
    }

    public void SetOpenProduct(NewProduct product, bool isAddNewProduct)
    {
        this.isAddNewProduct = isAddNewProduct;
        btnDel.SetState(!isAddNewProduct);
        this.originProduct = product;
        if (curProduct == null)
        {
            curProduct = new NewProduct();
        }
        NewProduct.Transmission(originProduct, curProduct);
    }

    #region ShowUI

    private void ShowUI()
    {
        ShowNameText();
        ShowUnitPriceText();
        ShowAmountText();
        ShowTotalPrice();
        ShowNoteText();
    }

    private void ShowTotalPrice()
    {
        txtTotalPrice.text = StringHelper.GetCommaCurrencyFormat(curProduct.TotalPrice);
    }

    private void ShowNameText()
    {
        ipName.text = curProduct.ProductName;
    }

    private void OnNameEndEdit(string text)
    {
        curProduct.ProductName = text;
    }

    private void ShowUnitPriceText()
    {
        ipUnitPrice.SetTextWithoutNotify(StringHelper.GetCommaCurrencyFormat(curProduct.UnitPrice));
    }

    private void OnUnitPriceEndEdit(string text)
    {
        string valText = text.Replace(",", string.Empty);
        if (string.IsNullOrEmpty(valText))
        {
            curProduct.SetUnitPrice(0);
        }
        else
        {
            curProduct.SetUnitPrice(int.Parse(valText));
        }
        ShowUI();
    }

    private void OnUnitPriceOnValueChanged(string text)
    {
        string valText = text.Replace(",", string.Empty);
        if (string.IsNullOrEmpty(valText))
        {
            curProduct.SetUnitPrice(0);
        }
        else
        {
            curProduct.SetUnitPrice(int.Parse(valText));
        }/*
        ShowUI();
        StopCoroutine(IDelyUnitPriceMoveEnd());
        StartCoroutine(IDelyUnitPriceMoveEnd());
        */
        ShowTotalPrice();
    }

    private void OnUnitPriceOnSelected(string text)
    {
        ipUnitPrice.SetTextWithoutNotify(curProduct.UnitPrice.ToString());
    }

    private IEnumerator IDelyUnitPriceMoveEnd()
    {
        yield return null;
        ipUnitPrice.MoveTextEnd(false);
    }

    private void ShowAmountText()
    {
        ipAmount.SetTextWithoutNotify(StringHelper.GetCommaCurrencyFormat(curProduct.Amount));
    }

    private void OnAmountEndEdit(string text)
    {
        string valText = text.Replace(",", string.Empty);
        if (string.IsNullOrEmpty(valText))
        {
            curProduct.SetAmount(0);
        }
        else
        {
            curProduct.SetAmount(int.Parse(valText));
        }
        ShowUI();
    }

    private void OnAmountOnValueChanged(string text)
    {
        string valText = text.Replace(",", string.Empty);
        if (string.IsNullOrEmpty(valText))
        {
            curProduct.SetAmount(0);
        }
        else
        {
            curProduct.SetAmount(int.Parse(valText));
        }
        ShowTotalPrice();
        /*
        ShowUI();
        StopCoroutine(IDelyAmountMoveEnd());
        StartCoroutine(IDelyAmountMoveEnd());*/
    }

    private void OnAmountOnSelected(string text)
    {
        ipAmount.SetTextWithoutNotify(curProduct.Amount.ToString());
    }

    private IEnumerator IDelyAmountMoveEnd()
    {
        yield return null;
        ipAmount.MoveTextEnd(false);
    }

    #endregion

    #region Buttons listener
    private void OnSaveButtonClicked()
    {
        if (curProduct == null)
        {
            curProduct = new NewProduct();
        }
        NewProduct.Transmission(curProduct, originProduct);
        if (isAddNewProduct)
        {
            originProduct.GetBill().AddProduct(originProduct);
        }
        originProduct.SetDirty();
        Hide();
    }

    private void OnDeleteButtonClicked()
    {
        NewBill bill = originProduct.GetBill();
        if (bill != null)
        {
            bill.RemoveProduct(originProduct);
        }
        Hide();
    }
    private void OnCancelButtonClicked()
    {
        Hide();
    }
    #endregion

    #region Note 
    private void ShowNoteText()
    {
        ipNote.text = curProduct.Note;
    }

    private void OnNoteEndEdit(string text)
    {
        curProduct.Note = text;
    }
    #endregion
}
