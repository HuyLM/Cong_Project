using System;
using System.Collections.Generic;



[Serializable]
public class Bill  {

    // primary value
    private string customerName;
    private DateTime date;
    private List<Product> products;
    private string note;
    private BillState state;
    private int paid;

    // private value
    [NonSerialized()]private int totalPrice;
    [NonSerialized()] private bool isDirty = true;


    public string CustomerName { get => customerName; set => customerName = value; }
    public DateTime Date { get => date; set => date = value; }
    public List<Product> Products { get => products; }
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
        products = new List<Product>();
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

    public Product AddProduct(Product newProduct = null)
    {
        if(newProduct == null) {
            newProduct = new Product(this);
        }
        products.Add(newProduct);
        SetDirty();
        return newProduct;
    }

    public void RemoveDetailBill(Product oldProduct)
    {
        if(oldProduct != null) {
            products.Remove( oldProduct );
            SetDirty();
        }
    }

    public void CalculatingPrice()
    {
        totalPrice = 0;
        if ( products != null ) {
            foreach ( var i in products ) {
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
        to.products = from.products;
        foreach(var detail in to.products) {
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
