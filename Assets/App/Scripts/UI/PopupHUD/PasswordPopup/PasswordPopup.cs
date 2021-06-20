using UnityEngine;
using TMPro;
using AtoLib;
using UnityEngine.SceneManagement;
using System;
using AtoLib.UI;

public class PasswordPopup : BasePopup
{
    [SerializeField] private TextMeshProUGUI txtNote;
    [SerializeField] private TMP_InputField ipPassword;
    [SerializeField] private ButtonBase btnSubmit;

    protected override void Start()
    {
        btnSubmit.onClick.AddListener(OnSubmitButtonClicked);
        ipPassword.onSelect.AddListener(OnPasswordSelected);
    }

    public override Frame OnBack()
    {
        return this;
    }

    protected override void OnShow(Action onCompleted = null, bool instant = false)
    {
        base.OnShow(onCompleted, instant);
        ipPassword.text = string.Empty;
        txtNote.text = string.Empty;
        ipPassword.Select();
    }

    private void OnSubmitButtonClicked()
    {
        string text = ipPassword.text;
        if (text.Equals(GameSaveData.Instance.Password))
        {
            SceneManager.LoadScene(1);
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
}
