using AtoLib;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using AtoLib.Helper;

public class BillRowViewDisplayer : ViewDisplayer<NewBill>
{
    [SerializeField] private Image imgState;
    [SerializeField] private TextMeshProUGUI txtNumber;
    [SerializeField] private TextMeshProUGUI txtCreateDate;
    [SerializeField] private TextMeshProUGUI txtDeliveryDate;
    [SerializeField] private TextMeshProUGUI txtTotalPrice;
    [SerializeField] private ButtonBase btnSelect;

    private int number;
    private Action<BillRowViewDisplayer> onSelect;


    private void Start()
    {
        btnSelect.onClick.AddListener(OnSelectButtonClicked);
    }

    public override void Show()
    {
        ShowStateColor();
        ShowNumberText();
        ShowCreateDateText();
        ShowDeliveryDateText();
        ShowTotalPriceText();
    }

    public void SetNumber(int number)
    {
        this.number = number;
    }

    private void OnSelectButtonClicked()
    {
        onSelect?.Invoke( this );
    }

    private void ShowStateColor()
    {
        imgState.color = GlobalResouces.Instance.UIConfigResource.GetBillColor( Model.IsDone ? BillState.Done : BillState.Debt );
    }

    private void ShowNumberText()
    {
        txtNumber.text = number.ToString();
    }

    private void ShowCreateDateText()
    {
        txtCreateDate.text = Model.CreateDate.ToString( "dd/MM/yyyy" );
    }
    private void ShowDeliveryDateText()
    {
        txtDeliveryDate.text = Model.DeliveryDate.ToString( "dd/MM/yyyy" );
    }

    private void ShowTotalPriceText()
    {
        txtTotalPrice.text = StringHelper.GetCommaCurrencyFormat( Model.TotalPrice );
    }

    public BillRowViewDisplayer AddOnSelect(Action<BillRowViewDisplayer> onSelect)
    {
        this.onSelect = onSelect;
        return this;
    }
}
