
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NewBill
{

    // primary value
    [SerializeField] private JsonDateTime createDate;
    [SerializeField] private JsonDateTime deliveryDate;
    [SerializeField] private List<NewProduct> products;
    [SerializeField] private int paid;


    // private value
    private int totalPrice;
    private bool isDirty = true;

    // reference value
    [NonSerialized] private NewCustomer customer;


    public DateTime CreateDate { get => createDate; set => createDate = value; }
    public DateTime DeliveryDate { get => deliveryDate; set => deliveryDate = value; }
    public List<NewProduct> Products { get => products; }
    public int Paid { get => paid; }
    public int TotalPrice
    {
        get
        {
            if (isDirty)
            {
                CalculatingPrice();
                isDirty = false;
            }
            return totalPrice;
        }
    }
    public int Debt
    {
        get
        {
            if (isDirty)
            {
                CalculatingPrice();
                isDirty = false;
            }
            return totalPrice - paid;
        }
    }
    public bool IsDone
    {
        get {
            return Paid == TotalPrice;
        }
    }

    public NewCustomer Customer { get => customer; }

    public NewBill()
    {
        createDate = DateTime.Now;
        deliveryDate = DateTime.Now;
        products = new List<NewProduct>();
        paid = 0;
        customer = null;
    }

    public NewBill(NewBill from)
    {
        products = new List<NewProduct>();
        foreach (var p in from.products)
        {
            products.Add(new NewProduct(p));
        }
        customer = from.customer;
        paid = from.paid;
        createDate = from.createDate;
        deliveryDate = from.deliveryDate;
        isDirty = true;
    }

    public void SetCustomer(NewCustomer customer)
    {
        this.customer = customer;
    }

    public void SetPaid(int newPaid)
    {
        this.paid = newPaid;
        SetDirty();
    }

    public void AddPaid(int addPaid)
    {
        if (Paid + addPaid > TotalPrice)
        {
            return;
        }
        else if (Debt - addPaid < 0)
        {
            return;
        }
        paid += addPaid;
        SetDirty();
    }

    public NewProduct AddProduct(NewProduct newProduct = null)
    {
        if (newProduct == null)
        {
            newProduct = new NewProduct(this);
        }
        products.Add(newProduct);
        SetDirty();
        return newProduct;
    }

    public void RemoveProduct(NewProduct oldProduct)
    {
        if (oldProduct != null)
        {
            products.Remove(oldProduct);
            SetDirty();
        }
    }

    private void CalculatingPrice()
    {
        totalPrice = 0;
        if (products != null)
        {
            foreach (var i in products)
            {
                totalPrice += i.TotalPrice;
            }
        }
    }

    public void SetDirty()
    {
        isDirty = true;
        if(customer != null)
        {
            customer.SetDirty();
        }
    }

    public static void Transmission(NewBill from, NewBill to)
    {
        if (to == null)
        {
            to = new NewBill();
        }
        to.createDate = from.createDate;
        to.deliveryDate = from.deliveryDate;
        to.products = new List<NewProduct>();
        for (int i = 0; i < from.products.Count; ++i)
        {
            NewProduct product = new NewProduct(from.products[i]);
            product.SetBill(to);
            to.products.Add(product);
        }
        to.paid = from.paid;
        to.isDirty = true;
        to.SetCustomer(from.Customer);
    }

    public static NewBill Convert(Bill oldBill)
    {
        NewBill newBill = new NewBill();
        newBill.createDate = oldBill.Date;
        newBill.deliveryDate = DateTime.Now;
        newBill.products = new List<NewProduct>();
        foreach(var p in oldBill.Products)
        {
            NewProduct newProduct = NewProduct.Convert(p);
            newProduct.SetBill(newBill);
            newBill.products.Add(newProduct);
        }
        newBill.paid = oldBill.Paid;
        newBill.isDirty = true;
        return newBill;
    }
}

