using AtoLib;
using System.Collections.Generic;
using UnityEngine;

public class BillList : Singleton<BillList>{
    private readonly string saveKey = "BILL_LIST_SAVE_KEY";
    private List<Bill> bills;

    public List<Bill> Bills { get => bills; }

    protected override void Initialize()
    {
        base.Initialize();
        bills = new List<Bill>();
    }

    public void LoadData()
    {
        string json = PlayerPrefs.GetString( saveKey );
        SaveDataModel saveData = null;
        if ( !string.IsNullOrEmpty( json ) ) {
            saveData = JsonUtility.FromJson<SaveDataModel>( json );
        }

        if ( saveData == null ) {
            bills = new List<Bill>();
            return;
        }
        bills = new List<Bill>();
        foreach ( var item in saveData.b ) {
            bills.Add( item );
        }
    }

    public void SaveData()
    {
        if ( bills == null ) {
            return;
        }

        SaveDataModel saveData = new SaveDataModel( bills.Count );

        int index = 0;
        foreach ( var item in bills ) {
            saveData.b[ index ] = item;
            index++;
        }
        string json = JsonUtility.ToJson( saveData );
        PlayerPrefs.SetString( saveKey, json );
    }


    [System.Serializable]
    private class SaveDataModel
    {
        public Bill[] b;

        public SaveDataModel(Bill[] items)
        {
            this.b = items;
        }

        public SaveDataModel(int capacity)
        {
            b = new Bill[ capacity ];
        }
    }
}
