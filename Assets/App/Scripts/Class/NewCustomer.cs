


using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class NewCustomer 
{
    // primary value
    [SerializeField] private JsonDateTime lastModifiedDate;
    [SerializeField] private string customerName;
    [SerializeField] private List<NewBill> bills;


    // private value
    private int totalPrice;
    private int paid;
    private bool isDirty = true;


    public DateTime LastModifiedDate { get => lastModifiedDate; set => lastModifiedDate = value; }
    public string CustomerName { get => customerName; set => customerName = value; }
    public List<NewBill> Bills { get => bills; }
    public int Paid {
        get
        {
            if (isDirty)
            {
                CalculatingPrice();
                isDirty = false;
            }
            return paid;
        }
    }
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

    public NewCustomer()
    {
        lastModifiedDate = DateTime.Now;
        bills = new List<NewBill>();
        customerName = "Cong ocho";
    }

    public NewBill AddBill(NewBill newBill = null)
    {
        if (newBill == null)
        {
            newBill = new NewBill();
            newBill.SetCustomer(this);
        }
        bills.Add(newBill);
        SetDirty();
        return newBill;
    }

    public void RemoveBill(NewBill oldBill)
    {
        if (oldBill != null)
        {
            bills.Remove(oldBill);
            SetDirty();
        }
    }

    private void CalculatingPrice()
    {
        totalPrice = 0;
        paid = 0;
        if (bills != null)
        {
            foreach (var i in bills)
            {
                totalPrice += i.TotalPrice;
            }
        }
    }
    public void SetDirty()
    {
        isDirty = true;
    }

    public static void Transmission(NewCustomer from, NewCustomer to)
    {
        if (to == null)
        {
            to = new NewCustomer();
        }
        to.lastModifiedDate = from.lastModifiedDate;
        to.bills = new List<NewBill>();
        for (int i = 0; i < from.bills.Count; ++i)
        {
            NewBill bill = new NewBill(from.bills[i]);
            bill.SetCustomer(to);
            to.bills.Add(bill);
        }
        to.isDirty = true;
    }
}
