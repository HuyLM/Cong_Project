using AtoLib;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ProductRowViewDisplayer : ViewDisplayer<Product>
{
    [SerializeField] private TextMeshProUGUI txtNumber;
    [SerializeField] private TextMeshProUGUI txtName;
    [SerializeField] private TextMeshProUGUI txtNote;
    [SerializeField] private TextMeshProUGUI txtAmount;
    [SerializeField] private TextMeshProUGUI txtUnitPrice;
    [SerializeField] private TextMeshProUGUI txtTotalPrice;
    [SerializeField] private ButtonBase btnSelect;
    [SerializeField] private Image imgBackground;
    [SerializeField] private Color color1;
    [SerializeField] private Color color2;

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
        ShowBackgroundColor();
    }

    public void SetNumber(int number)
    {
        this.number = number;
    }

    private void OnSelectButtonClicked()
    {
        onSelect?.Invoke( this );
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

    private void ShowBackgroundColor()
    {
        if(number % 2 == 0) {
            imgBackground.color = color1;
        }
        else {
            imgBackground.color = color2;
        }
    }

    private void ShowNoteText() {
        txtNote.text = Model.Note;
    }

    public ProductRowViewDisplayer AddOnSelect(Action<ProductRowViewDisplayer> onSelect)
    {
        this.onSelect = onSelect;
        return this;
    }
}
