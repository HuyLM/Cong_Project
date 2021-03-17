using AtoLib.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using TMPro;
using AtoLib;

public class EditBillPanel : DOTweenFrame
{
    [SerializeField] private ProductRowCollector productRowCollector;
    [SerializeField] private AutoCompleteComboBox cbName;
    [SerializeField] private InputField ipName;
    [SerializeField] private DropDownList ddState;
    [SerializeField] private TMP_InputField ipNote;
    [SerializeField] private ButtonBase btnSave;
    [SerializeField] private ButtonBase btnCancel;
    [SerializeField] private ButtonBase btnUndo;

    private Bill curBill;
    private Bill originBill;

    private void Start()
    {
        cbName.OnSelectionChanged.AddListener( OnNameSelectionChanged );
        ddState.OnSelectionChanged.AddListener( OnStateSelected );
        ipNote.onEndEdit.AddListener( OnNoteEndEdit );
        btnSave.onClick.AddListener( OnSaveButtonClicked );
        btnCancel.onClick.AddListener( OnCancelButtonClicked );
    }
    protected override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow( onCompleted, instant );
        ShowUI();
    }

    private void ShowUI()
    {
        ShowProducts();
        ShowNameCombox();
        SetDropDownListState();
        StartCoroutine( IDelayAFrame() );
    }

    private IEnumerator IDelayAFrame()
    {
        yield return null;
        ShowState();
    }

    public override Frame OnBack()
    {
        UIHUD.Instance.Show<ListBillPanel>();
        return base.OnBack();
    }

    public void SetOpenBill(Bill bill)
    {
        this.originBill = bill;
        Bill.Transmission( originBill, curBill );
    }

    #region Products

    private void OpenEditProductPopup(Product product)
    {
        Debug.Log($"Open Edit Product: {product.ProductName}");
    }

    private void ShowProducts()
    {
        List<Product> products = curBill.Products;
        productRowCollector.AddOnSelect(OnSelectProductRow).SetCapacity( products.Count ).SetItems( products ).Show();
    }
    private void OnSelectProductRow(ProductRowViewDisplayer displayer)
    {
        OpenEditProductPopup( displayer.Model );
    }

    #endregion

    #region Combox Name
    private void ShowNameCombox()
    {
        List<Bill> bills = BillList.Instance.Bills;
        List<string> allName = new List<string>();
        foreach(var b in bills) {
            if(!allName.Contains(b.CustomerName)) {
                allName.Add( b.CustomerName );
            }
        }
        cbName.SetAvailableOptions(allName);
        ipName.text = curBill.CustomerName;

    }

    private void OnNameSelectionChanged(string text, bool isSelect)
    {
        curBill.CustomerName = text;
    }

    #endregion

    #region dropdownlist state
    private void SetDropDownListState()
    {
        ddState.Items = GlobalResouces.Instance.UIConfigResource.GetListItemStateBill( ddState.Items, curBill.State );
    }
    private void ShowState()
    {
        ddState.OnItemForceSelect( (int)curBill.State );
    }

    private void OnStateSelected(int itemIdx)
    {
        curBill.ChangeState( (BillState)itemIdx );
        //curBill.ChangeState( (BillState)ddState.SelectedItem.ID ); other
    }
    #endregion

    #region Note 
    private void ShowNoteText()
    {
        ipNote.text = curBill.Note;
    }

    private void OnNoteEndEdit(string text)
    {
        curBill.Note = text;
    }
    #endregion

    #region Buttons
    private void OnSaveButtonClicked()
    {
        Bill.Transmission( curBill, originBill );
        Hide();
        UIHUD.Instance.Show<ListBillPanel>();
    }

    private void OnCancelButtonClicked()
    {
        Hide();
        UIHUD.Instance.Show<ListBillPanel>();
    }

    private void OnUndoButtonClicked()
    {
        Bill.Transmission( originBill, curBill );
        ShowUI();
    }
    #endregion

}
