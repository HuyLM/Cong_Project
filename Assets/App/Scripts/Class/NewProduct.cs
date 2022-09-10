using System;
using UnityEngine;


[Serializable]
public class NewProduct
{
    // primary value
    [SerializeField] private string productName;
    [SerializeField] private int amount;
    [SerializeField] private int unitPrice;
    [SerializeField] private string note;


    // private value
    private bool isDirty = true;
    private int totalPrice;

    // reference value
    [NonSerialized] private NewBill myBill;


    public string ProductName { get => productName; set => productName = value; }
    public int Amount { get => amount; }
    public int UnitPrice { get => unitPrice; }
    public string Note { get => note; set => note = value; }


    public int TotalPrice
    {
        get
        {
            if (isDirty)
            {
                totalPrice = amount * unitPrice;
                isDirty = false;
            }
            return totalPrice;
        }
    }

    public NewProduct()
    {
        myBill = null;
        productName = "New";
        amount = 0;
        unitPrice = 0;
        note = "Không có";
    }

    public NewProduct(NewBill bill)
    {
        myBill = bill;
        productName = "New";
        amount = 0;
        unitPrice = 0;
        note = "Không có";
    }

    public NewProduct(NewProduct product)
    {
        this.productName = product.productName;
        this.amount = product.amount;
        this.unitPrice = product.unitPrice;
        this.myBill = product.myBill;
        this.note = product.note;
    }

    public void SetAmount(int newAmount)
    {
        amount = newAmount;
        SetDirty();
    }

    public void SetUnitPrice(int newPrice)
    {
        this.unitPrice = newPrice;
        SetDirty();
    }

    public void SetBill(NewBill newBill)
    {
        this.myBill = newBill;
    }

    public NewBill GetBill()
    {
        return this.myBill;
    }

    public void SetDirty()
    {
        isDirty = true;
        if (myBill != null)
        {
            myBill.SetDirty();
        }
    }

    public static void Transmission(NewProduct from, NewProduct to)
    {
        to.productName = from.productName;
        to.amount = from.amount;
        to.unitPrice = from.unitPrice;
        to.myBill = from.myBill;
        to.note = from.note;
        to.isDirty = true;

    }

    public static NewProduct Convert(Product oldProduct)
    {
        NewProduct newProduct = new NewProduct();
        newProduct.productName = oldProduct.ProductName;
        newProduct.amount = oldProduct.Amount;
        newProduct.unitPrice = oldProduct.UnitPrice;
        newProduct.note = oldProduct.Note;
        newProduct.isDirty = true;
        return newProduct;
    }

}
