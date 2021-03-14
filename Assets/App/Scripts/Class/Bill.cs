using System;
using System.Collections.Generic;



[Serializable]
public class Bill  {

    // primary value
    private string customerName;
    private DateTime date;
    private List<DetailBill> detailbills;
    private string note;
    private BillState state;
    private int paid;

    // private value
    [NonSerialized()]private int totalPrice;
    [NonSerialized()] private bool isDirty = true;


    public string CustomerName { get => customerName; set => customerName = value; }
    public DateTime Date { get => date; set => date = value; }
    public List<DetailBill> Detailbills { get => detailbills; }
    public string Note { get => note; set => note = value; }
    public BillState State { get => state; }
    public int Paid { get => paid;  }
    public int TotalPrice {
        get {
            if(isDirty) {
                CalculatingPrice();
                isDirty = false;
            }
            return totalPrice;
        }
    }
    public int Debt
    {
        get {
            return totalPrice - paid;
        }
    }

    public Bill()
    {
        customerName = "default";
        date = DateTime.Now;
        detailbills = new List<DetailBill>();
        note = "this new note";
        state = BillState.Waiting;
        paid = 0;
    }

    public void SetPaid(int newPaid)
    {
        this.paid = newPaid;
        if(paid >= TotalPrice) {
            switch ( state ) {
                case BillState.Debt: {
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

    public DetailBill AddDetailBill(DetailBill newDetailBill = null)
    {
        if(newDetailBill == null) {
            newDetailBill = new DetailBill(this);
        }
        detailbills.Add(newDetailBill);
        SetDirty();
        return newDetailBill;
    }

    public void RemoveDetailBill(DetailBill oldDetailBill)
    {
        if(oldDetailBill != null) {
            detailbills.Remove( oldDetailBill );
            SetDirty();
        }
    }

    public void CalculatingPrice()
    {
        totalPrice = 0;
        if ( detailbills != null ) {
            foreach ( var i in detailbills ) {
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
        to.customerName = from.customerName;
        to.detailbills = from.detailbills;
        foreach(var detail in to.detailbills) {
            detail.SetBill( to );
        }
        to.note = from.note;
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
