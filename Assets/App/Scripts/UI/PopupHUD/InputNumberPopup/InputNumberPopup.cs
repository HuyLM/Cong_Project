using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using AtoLib.Helper;
using UnityEngine.UI;

public class InputNumberPopup : ConfirmPopup
{
    [SerializeField] private TMP_InputField ipNumber;
    [SerializeField] private TextMeshProUGUI txtMin;
    [SerializeField] private TextMeshProUGUI txtMax;
    [SerializeField] private Button btnMin;
    [SerializeField] private Button btnMax;

    private int min;
    private int max;
    public int number;

    protected override void Start()
    {
        base.Start();
        ipNumber.onValueChanged.AddListener(OnNumberOnValueChanged);
        ipNumber.onEndEdit.AddListener(OnNumberEndEdit);
        btnMax.onClick.AddListener(OnMaxButtonClicked);
        btnMin.onClick.AddListener(OnMinButtonClicked);
    }

    protected override void OnShowAnimationCompleted()
    {
        base.OnShowAnimationCompleted();
        ipNumber.Select();
    }

    public void SetNumber(int number, int min, int max)
    {
        this.number = number;
        this.min = min;
        this.max = max;
        this.txtMin.text = StringHelper.GetCommaCurrencyFormat(min);
        this.txtMax.text = StringHelper.GetCommaCurrencyFormat(max);
        ShowAmountText();
    }

    private void ShowAmountText()
    {
        ipNumber.SetTextWithoutNotify(number.ToString());
    }

    private void OnNumberOnValueChanged(string text)
    {
        string valText = text.Replace(",", string.Empty);
        if (string.IsNullOrEmpty(valText))
        {
            this.number = 0;
        }
        else
        {
            int value = int.Parse(valText);
            if(value < this.min)
            {
                value = min;
            }
            else if(max < value)
            {
                value = max;
            }
            this.number = value;
        }
        ShowAmountText();
    }

    private void OnNumberEndEdit(string text)
    {
        ShowAmountText();
    }

    private void OnMinButtonClicked()
    {
        this.number = min;
        ShowAmountText();
    }

    private void OnMaxButtonClicked()
    {
        this.number = max;
        ShowAmountText();
    }
}
