using AtoLib;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GameSaveData : Singleton<GameSaveData>
{

    private readonly string saveKey = "GAME_SAVE_DATA_KEY";
    private readonly string pathFileOnline = "Cong/data";

    private string password;

    public string Password
    {
        get => password;
        set => password = value;
    }
    public async Task<int> LoadData()
    {
        LoadingHUD.Instance.Show<LoadingPanel>();
        // new app with old data
        /*
        if (PlayerPrefs.HasKey(BillList.Instance.SaveKey))
        {
            await LoadDataWithOldData();
            LoadingHUD.Instance.Hide<LoadingPanel>();
            return 0;
        }
        */

        // new app with new data
        int result = await LoadDataWithNewData();
        LoadingHUD.Instance.Hide<LoadingPanel>();

        return result;
    }

    private async Task<int> LoadDataWithOldData()
    {
        LoadingHUD.Instance.Show<LoadingPanel>();
        BillList.Instance.LoadData();
        password = string.Empty;
        await SaveData(true);
        PlayerPrefs.DeleteKey(BillList.Instance.SaveKey);
        LoadingHUD.Instance.Hide<LoadingPanel>();

        return 0;
    }

    private async Task<int> LoadDataWithNewData()
    {
        List<Bill> bills = BillList.Instance.Bills;
        // find file data
        var files = await UnityGoogleDriveHelper.FindFilesByPathAsync(pathFileOnline, fields: new List<string> { "files(id, name, size, mimeType, modifiedTime)" });
        // download content data
        UnityGoogleDrive.Data.File file = await UnityGoogleDriveHelper.GetFileWithContent(files[0].Id, "text/plain");
        // content byte[] to string json
        string json = string.Empty;
        if (file != null && file.Content != null)
        {
            string json1 = System.Text.Encoding.UTF8.GetString(file.Content, 3, file.Content.Length - 3);
            json = json1.Clone().ToString();
        }
        SaveDataModel saveData = null;
        if (!string.IsNullOrEmpty(json))
        {
            saveData = JsonUtility.FromJson<SaveDataModel>(json);
        }

        if (saveData == null)
        {
            bills = new List<Bill>();
            password = string.Empty;
            return 0;
        }
        bills = new List<Bill>();
        foreach (var item in saveData.b)
        {
            bills.Add(item);
            foreach (var p in item.Products)
            {
                p.SetBill(item);
            }
        }
        BillList.Instance.Bills = bills;
        password = saveData.password;
        if (saveData.isUsing == true)
        {
            return -1;
        }
        await SaveData(true);
        return 0;
    }

    public async Task<int> SaveData(bool isUsing)
    {
        LoadingHUD.Instance.Show<LoadingPanel>();
        List<Bill> bills = BillList.Instance.Bills;
        SaveDataModel saveData = new SaveDataModel(bills.Count);
        saveData.password = password;
        saveData.isUsing = false;//isUsing;
        int index = 0;
        foreach (var item in bills)
        {
            saveData.b[index] = item;
            index++;
        }
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(saveKey, json);
        UnityGoogleDrive.Data.File file = new UnityGoogleDrive.Data.File() { Name = "data", Content = System.Text.Encoding.Default.GetBytes(json) };
        await UnityGoogleDriveHelper.CreateOrUpdateFileAtPathAsync(file, pathFileOnline);
        LoadingHUD.Instance.Hide<LoadingPanel>();
        return 0;
    }

    [System.Serializable]
    private class SaveDataModel
    {
        public Bill[] b;
        public bool isUsing;
        public string password;


        public SaveDataModel(int capacity)
        {
            b = new Bill[capacity];
        }
    }
}
