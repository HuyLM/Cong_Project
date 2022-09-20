using AtoLib;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
public class GameSaveData : Singleton<GameSaveData>
{

    private readonly bool useConvertData = false;

    private readonly string saveKey = "GAME_SAVE_DATA_KEY";
    private readonly string pathFileOnline = "Cong/data";
    private readonly string pathFileOnlineTest = "Cong/test";

    private string password;

    public string Password
    {
        get => password;
        set => password = value;
    }
    public async Task<int> LoadData()
    {
        LoadingHUD.Instance.Show<LoadingPanel>();
        int result = 0;
        if (useConvertData)
        {
            Debug.LogError("Start Convert.......");
            Debug.LogError("-----Start LoadDataWithOldData------");
            result = await LoadDataWithOldData();
            Debug.LogError("-----LoadDataWithOldData Completed------");
            ConvertData();
            Debug.LogError("-----Start SaveData------");
            await SaveData(true);
            Debug.LogError("-----SaveData Completed------");
            Debug.LogError(".......Convert Completed");
        }
        else
        {
            result = await LoadDataWithData();
        }
        LoadingHUD.Instance.Hide<LoadingPanel>();

        return result;
    }

    private void ConvertData()
    {
        List<Bill> bills = BillList.Instance.Bills;
        List<NewCustomer> customers = new List<NewCustomer>();

        foreach(var bill in bills)
        {
            NewCustomer searchCustomer = customers.FirstOrDefault(c => c.CustomerName.Equals(bill.CustomerName));
            if(searchCustomer == null)
            {
                searchCustomer = new NewCustomer();
                searchCustomer.SetName(bill.CustomerName);
                customers.Add(searchCustomer);
            }
            NewBill newBill = NewBill.Convert(bill);
            newBill.SetCustomer(searchCustomer);
            searchCustomer.AddBill(newBill);
        }

        foreach(var c in customers)
        {
            c.Bills.Sort(delegate (NewBill x, NewBill y)
            {
                if(x.CreateDate.Date.CompareTo(y.CreateDate.Date) == 0)
                {
                    return x.DeliveryDate.Date.CompareTo(y.DeliveryDate.Date) * -1;
                }
                return x.CreateDate.Date.CompareTo(y.CreateDate.Date) * -1;
            });
        }

        GameData.Instance.Customers = customers;
    }


    private async Task<int> LoadDataWithOldData()
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
        SaveDataModelOld saveData = null;
        if (!string.IsNullOrEmpty(json))
        {
            saveData = JsonUtility.FromJson<SaveDataModelOld>(json);
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


    private async Task<int> LoadDataWithData()
    {
        List<NewCustomer> customers = null;
        // find file data
        var files = await UnityGoogleDriveHelper.FindFilesByPathAsync(pathFileOnlineTest, fields: new List<string> { "files(id, name, size, mimeType, modifiedTime)" });
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
            customers = new List<NewCustomer>();
            password = string.Empty;
            return 0;
        }
        customers = new List<NewCustomer>();
        foreach (var item in saveData.cs)
        {
            customers.Add(item);
            foreach (var b in item.Bills)
            {
                b.SetCustomer(item);
                foreach(var p in b.Products)
                {
                    p.SetBill(b);
                }
            }
        }
        GameData.Instance.Customers = customers;
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
        List<NewCustomer> customers = GameData.Instance.Customers;
        customers.Sort(delegate (NewCustomer x, NewCustomer y)
        {
            return x.LastModifiedDate.CompareTo(y.LastModifiedDate) * -1;
        });
        SaveDataModel saveData = new SaveDataModel(customers.Count);
        saveData.password = password;
        saveData.isUsing = false;//isUsing;
        int index = 0;
        foreach (var item in customers)
        {
            saveData.cs[index] = item;
            index++;
        }
        string json = JsonUtility.ToJson(saveData);
        PlayerPrefs.SetString(saveKey, json);
        UnityGoogleDrive.Data.File file = new UnityGoogleDrive.Data.File() { Name = "test", Content = System.Text.Encoding.Default.GetBytes(json) };
        await UnityGoogleDriveHelper.CreateOrUpdateFileAtPathAsync(file, pathFileOnlineTest);
        LoadingHUD.Instance.Hide<LoadingPanel>();
        return 0;
    }

    [System.Serializable]
    private class SaveDataModelOld
    {
        public Bill[] b;
        public bool isUsing;
        public string password;


        public SaveDataModelOld(int capacity)
        {
            b = new Bill[capacity];
        }
    }


    [System.Serializable]
    private class SaveDataModel
    {
        public NewCustomer[] cs;
        public bool isUsing;
        public string password;


        public SaveDataModel(int capacity)
        {
            cs = new NewCustomer[capacity];
        }
    }

}
