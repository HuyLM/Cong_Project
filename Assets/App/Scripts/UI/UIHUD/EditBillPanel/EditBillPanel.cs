using AtoLib.UI;
using System;
using System.Collections.Generic;
using UnityEngine;

public class EditBillPanel : DOTweenFrame
{
    [SerializeField] private ProductRowCollector productRowCollector;

    private Bill curBill;
    protected override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow( onCompleted, instant );
        ShowProducts();
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
        OpenEditProductPopup(displayer.Model);
    }
}
