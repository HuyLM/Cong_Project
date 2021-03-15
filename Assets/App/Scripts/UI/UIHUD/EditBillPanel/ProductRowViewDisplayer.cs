using AtoLib;
using UnityEngine;
using TMPro;
using System;

public class ProductRowViewDisplayer : ViewDisplayer<Product>
{
    [SerializeField] private TextMeshProUGUI txtNumber;
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtAmount;
    [SerializeField] private TextMeshProUGUI txtUnitPrice;
    [SerializeField] private TextMeshProUGUI txtTotalPrice;
    [SerializeField] private ButtonBase btnSelect;

    private int number;
    private Action<ProductRowViewDisplayer> onSelect;


    private void Start()
    {
        btnSelect.onClick.AddListener( OnSelectButtonClicked );
    }

    public override void Show()
    {
        ShowNumberText();
        ShowNameText();
        ShowAmountText();
        ShowUnitPrice();
        ShowTotalPriceText();
    }

    public void SetNumber(int number)
    {
        this.number = number;
    }

    private void OnSelectButtonClicked()
    {

    }

    private void ShowUnitPrice()
    {
        txtUnitPrice.text = System.String.Format( "{0:N0}", Model.UnitPrice );
    }

    private void ShowNumberText()
    {
        txtNumber.text = number.ToString();
    }

    private void ShowAmountText()
    {
        txtAmount.text = Model.Amount.ToString("D3");
    }

    private void ShowNameText()
    {
        txtName.text = Model.ProductName;
    }

    private void ShowTotalPriceText()
    {
        txtTotalPrice.text = System.String.Format( "{0:N0}", Model.TotalPrice );
    }

    public ProductRowViewDisplayer AddOnSelect(Action<ProductRowViewDisplayer> onSelect)
    {
        this.onSelect = onSelect;
        return this;
    }
}
