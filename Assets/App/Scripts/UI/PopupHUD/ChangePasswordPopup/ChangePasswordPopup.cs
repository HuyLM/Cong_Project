using AtoLib;
using System;
using TMPro;
using UnityEngine;

public class ChangePasswordPopup : BasePopup
{
    [SerializeField] private TextMeshProUGUI txtNote;
    [SerializeField] private TMP_InputField ipPassword;
    [SerializeField] private TMP_InputField ipNewPassword;
    [SerializeField] private TMP_InputField ipConfirmPassword;
    [SerializeField] private ButtonBase btnSubmit;
    [SerializeField] private ButtonBase btnCancel;

    protected override void Start()
    {
        btnSubmit.onClick.AddListener(OnSubmitButtonClicked);
        ipPassword.onSelect.AddListener(OnPasswordSelected);
        btnCancel.onClick.AddListener(OnCancelButtonClicked);
    }

    protected override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow(onCompleted, instant);
        ipPassword.text = string.Empty;
        ipNewPassword.text = string.Empty;
        ipConfirmPassword.text = string.Empty;
        txtNote.text = string.Empty;
        ipPassword.Select();
    }

    private async void OnSubmitButtonClicked()
    {
        string text = ipPassword.text;
        if (text.Equals(GameSaveData.Instance.Password))
        {
            if (ipNewPassword.text.Equals(ipConfirmPassword.text))
            {
                GameSaveData.Instance.Password = ipNewPassword.text;
                await GameSaveData.Instance.SaveData(true);
                txtNote.text = "Đổi mật khẩu thành công";
                this.Hide();
            }
            else
            {
                txtNote.text = "Xác nhận không trùng với mất khẩu mới";
            }
        }
        else
        {
            txtNote.text = "Mật khẩu không chính xác";
        }
    }

    private void OnPasswordSelected(string text)
    {
        txtNote.text = string.Empty;
    }

    private void OnCancelButtonClicked()
    {
        this.Hide();
    }
}
