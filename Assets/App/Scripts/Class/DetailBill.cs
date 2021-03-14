using System;



[Serializable]
public class DetailBill  {

    // primary value
    private string productName;
    private int amount;
    private int unitPrice;

    // private value
    [NonSerialized()] private bool isDirty = true;
    [NonSerialized()] private int totalPrice;

    // reference value
    [NonSerialized()] private Bill myBill;


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
    public DetailBill(Bill bill)
    {
        myBill = bill;
        productName = "New";
        amount = 69;
        unitPrice = 96;
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
