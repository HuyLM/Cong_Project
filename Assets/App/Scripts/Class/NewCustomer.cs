


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
    public string CustomerName { get => customerName; private set => customerName = value; }
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

    public bool IsDone
    {
        get
        {
            return Paid == TotalPrice;
        }
    }

    public NewCustomer()
    {
        lastModifiedDate = DateTime.Now;
        bills = new List<NewBill>();
        customerName = "Cong ocho";
    }

    public void SetName(string name)
    {
        CustomerName = name;
        LastModifiedDate = DateTime.Now;
    }

    public bool SetPaid(int paid)
    {
        if(paid < 0 || paid > TotalPrice)
        {
            return false;
        }
        for (int i = Bills.Count-1; i >= 0; ++i)
        {
            NewBill bill = Bills[i];
            int debt = bill.Debt;
            if (debt >= paid)
            {
                bill.AddPaid(paid);
                break;
            }
            bill.AddPaid(debt);
            paid -= debt;
        }
        SetDirty();
        LastModifiedDate = DateTime.Now;
        return true;
    }

    public bool AddPaid(int addPaid)
    {
        if(Paid + addPaid > TotalPrice)
        {
            return false;
        }
        else if(Paid + addPaid < 0)
        {
            return false;
        }
        for(int i = Bills.Count - 1; i >= 0 ; --i)
        {
            NewBill bill = Bills[i];
            if(bill.IsDone)
            {
                continue;
            }
            int debt = bill.Debt;
            if(debt >= addPaid)
            {
                bill.AddPaid(addPaid);
                break;
            }
            bill.AddPaid(debt);
            addPaid -= debt;
        }
        SetDirty();
        LastModifiedDate = DateTime.Now;
        return true;
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
        LastModifiedDate = DateTime.Now;
        return newBill;
    }

    public void RemoveBill(NewBill oldBill)
    {
        if (oldBill != null)
        {
            bills.Remove(oldBill);
            SetDirty();
            LastModifiedDate = DateTime.Now;
        }
    }

    public void SortBills()
    {
        Bills.Sort(delegate (NewBill x, NewBill y)
        {
            if ((x.Debt == 0 && y.Debt == 0) || (x.Debt != 0 && y.Debt != 0))
            {
                if (x.CreateDate.Date.CompareTo(y.CreateDate.Date) == 0)
                {
                    return x.DeliveryDate.Date.CompareTo(y.DeliveryDate.Date) * -1;
                }
                return x.CreateDate.Date.CompareTo(y.CreateDate.Date) * -1;
            }
            if (x.Debt == 0)
            {
                return 1;
            }
            return 0;
        });
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
                paid += i.Paid;
            }
        }
    }
    public void SetDirty()
    {
        isDirty = true;
        SortBills();
    }

    public static void Transmission(NewCustomer from, NewCustomer to)
    {
        if (to == null)
        {
            to = new NewCustomer();
        }
        to.LastModifiedDate = DateTime.Now;
        to.CustomerName = from.CustomerName;
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
