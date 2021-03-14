


using UnityEngine;
namespace AtoLib {
    public static class SaveLoad {

        public static void Save() {
            BillList.Instance.SaveData();
            PlayerPrefs.Save();
        }

        public static void Load() {
            BillList.Instance.LoadData();
        }
         
    }
}