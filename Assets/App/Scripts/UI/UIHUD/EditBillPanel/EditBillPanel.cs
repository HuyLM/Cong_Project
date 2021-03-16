using AtoLib.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

public class EditBillPanel : DOTweenFrame
{
    [SerializeField] private ProductRowCollector productRowCollector;
    [SerializeField] private AutoCompleteComboBox cbName;
    [SerializeField] private InputField ipName;
    [SerializeField] private DropDownList ddState;


    private Bill curBill;
    protected override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow( onCompleted, instant );
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
        this.curBill = bill;
    }

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
        cbName.OnSelectionChanged.RemoveAllListeners();
        cbName.OnSelectionChanged.AddListener( OnSelectionChaned );
    }

    private void OnSelectionChaned(string text, bool isSelect)
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
    #endregion

}
