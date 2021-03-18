﻿using System;
using UnityEngine;


[Serializable]
public class Product  {

    // primary value
    [SerializeField] private string productName;
    [SerializeField] private int amount;
    [SerializeField] private int unitPrice;

    // private value
     private bool isDirty = true;
     private int totalPrice;

    // reference value
    [NonSerialized]private Bill myBill;


    public string ProductName { get => productName; set => productName = value; }
    public int Amount { get => amount;}
    public int UnitPrice { get => unitPrice; set => unitPrice = value; }

    public int TotalPrice
    {
        get {
            if(isDirty) {
                totalPrice = amount * unitPrice;
                isDirty = false;
            }
            return totalPrice;
        }
    }
    public Product(Bill bill)
    {
        myBill = bill;
        productName = "New";
        amount = 69;
        unitPrice = 96;
    }

    public Product(Product product)
    {
        this.productName = product.productName;
        this.amount = product.amount;
        this.unitPrice = product.unitPrice;
        this.myBill = product.myBill;
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

    public void SetBill(Bill newBill)
    {
        this.myBill = newBill;
    }
    public void SetDirty()
    {
        isDirty = true;
        myBill.SetDirty();
    }

}
