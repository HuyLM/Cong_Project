using AtoLib;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class ProductRowViewDisplayer : ViewDisplayer<NewProduct> {
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


    private void Start() {
        btnSelect.onClick.AddListener(OnSelectButtonClicked);
    }

    public override void Show() {
        ShowNumberText();
        ShowNameText();
        ShowNoteText();
        ShowAmountText();
        ShowUnitPrice();
        ShowTotalPriceText();
        ShowBackgroundColor();
    }

    public void SetNumber(int number) {
        this.number = number;
    }

    private void OnSelectButtonClicked() {
        onSelect?.Invoke(this);
    }

    private void ShowUnitPrice() {
        if(txtUnitPrice != null)
        txtUnitPrice.text = System.String.Format("{0:N0}", Model.UnitPrice);
    }

    private void ShowNumberText() {
        if (txtNumber != null)
            txtNumber.text = number.ToString();
    }

    private void ShowAmountText() {
        if (txtAmount != null)
            txtAmount.text = Model.Amount.ToString("D3") + " x " + System.String.Format("{0:N0}", Model.UnitPrice);
    }

    private void ShowNameText() {
        if (txtName != null)
            txtName.text = Model.ProductName;
    }

    private void ShowTotalPriceText() {
        if (txtTotalPrice != null)
            txtTotalPrice.text = System.String.Format("{0:N0}", Model.TotalPrice);
    }

    private void ShowBackgroundColor() {
        if (imgBackground == null) return;
        if (number % 2 == 0) {
            imgBackground.color = color1;
        } else {
            imgBackground.color = color2;
        }
    }

    private void ShowNoteText() {
        if (txtNote == null) return;
        txtNote.text = Model.Note;
    }

    public ProductRowViewDisplayer AddOnSelect(Action<ProductRowViewDisplayer> onSelect) {
        this.onSelect = onSelect;
        return this;
    }
}
