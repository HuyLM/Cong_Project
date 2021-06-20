
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Bill
{

    // primary value
    [SerializeField] private string customerName;
    [SerializeField] private JsonDateTime date;
    [SerializeField] private List<Product> products;
    [SerializeField] private BillState state;
    [SerializeField] private int paid;

    // private value
    private int totalPrice;
    private bool isDirty = true;


    public string CustomerName { get => customerName; set => customerName = value; }
    public DateTime Date { get => date; set => date = value; }
    public List<Product> Products { get => products; }
    public BillState State { get => state; }
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
            return totalPrice - paid;
        }
    }

    public Bill()
    {
        customerName = "default";
        date = DateTime.Now;
        products = new List<Product>();
        state = BillState.None;
        paid = 0;
    }

    public Bill(Bill from)
    {
        customerName = from.customerName;
        products = new List<Product>();
        foreach (var p in from.products)
        {
            products.Add(new Product(p));
        }

        state = from.state;
        paid = from.paid;
        isDirty = true;
    }

    public void SetPaid(int newPaid)
    {
        this.paid = newPaid;
        if (paid >= TotalPrice)
        {
            switch (state)
            {
                case BillState.Debt:
                {
                    state = BillState.Done;
                    break;
                }
                case BillState.Waiting:
                case BillState.None:
                case BillState.Done:
                case BillState.Cancel:
                default:
                break;
            }
        }
    }

    public Product AddProduct(Product newProduct = null)
    {
        if (newProduct == null)
        {
            newProduct = new Product(this);
        }
        products.Add(newProduct);
        SetDirty();
        return newProduct;
    }

    public void RemoveProduct(Product oldProduct)
    {
        if (oldProduct != null)
        {
            products.Remove(oldProduct);
            SetDirty();
        }
    }

    public void ChangeState(BillState newState)
    {
        BillState oldState = state;
        state = newState;
        if (newState != oldState)
        {
            switch (newState)
            {
                case BillState.None:
                {

                    break;
                }
                case BillState.Done:
                {

                    break;
                }
                case BillState.Debt:
                {

                    break;
                }
                case BillState.Waiting:
                {
                    break;
                }
                case BillState.Cancel:
                {
                    break;
                }
                default:
                {
                    break;
                }
            }
        }
    }

    public void CalculatingPrice()
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
    }

    public static void Transmission(Bill from, Bill to)
    {
        if (to == null)
        {
            to = new Bill();
        }
        to.customerName = from.customerName;
        to.products = new List<Product>();
        for (int i = 0; i < from.products.Count; ++i)
        {
            Product product = new Product(from.products[i]);
            product.SetBill(to);
            to.products.Add(product);
        }
        to.state = from.state;
        to.paid = from.paid;
        to.isDirty = true;
    }
}

public enum BillState
{
    None = 0,
    Done, // complete 
    Debt, // delivered  , have not paid all the money
    Waiting, // not delivered
    Cancel // no more receiving products
}
