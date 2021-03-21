using AtoLib;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using AtoLib.Helper;

public class BillRowViewDisplayer : ViewDisplayer<Bill>
{
    [SerializeField] private Image imgState;
    [SerializeField] private TextMeshProUGUI txtNumber;
    [SerializeField] private TextMeshProUGUI txtDate;
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtTotalPrice;
    [SerializeField] private ButtonBase btnSelect;

    private int number;
    private Action<BillRowViewDisplayer> onSelect;


    private void Start()
    {
        btnSelect.onClick.AddListener( OnSelectButtonClicked );
    }

    public override void Show()
    {
        ShowStateColor();
        ShowNumberText();
        ShowDateText();
        ShowNameText();
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
        imgState.color = GlobalResouces.Instance.UIConfigResource.GetBillColor( Model.State );
    }

    private void ShowNumberText()
    {
        txtNumber.text = number.ToString();
    }

    private void ShowDateText()
    {
        txtDate.text = Model.Date.ToString( "dd/MM/yyyy" );
    }

    private void ShowNameText()
    {
        txtName.text = Model.CustomerName;
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
