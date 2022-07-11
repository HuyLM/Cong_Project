using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
namespace AtoLib
{

    public class SaveLoadListener : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI txtNote;
        private bool isLoaded;
        private async void Awake()
        {

            DontDestroyOnLoad(this);
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
            int result = await GameSaveData.Instance.LoadData();
            if (result == -1)
            { // other app is using data
                txtNote.text = "App khác đang mở";
                return;
            }
            //SaveLoad.Load();
            isLoaded = true;
            CheckDayOpenGame();
            PopupHUD.Instance.Show<PasswordPopup>();
        }

        private async void OnApplicationPause(bool pause)
        {
            if (!isLoaded)
            {
                return;
            }
            if (pause)
            {
                //await GameSaveData.Instance.SaveData(false);
                //SaveLoad.Save();
            }
        }

        private async void OnApplicationQuit()
        {
            if (!isLoaded)
            {
                return;
            }
            //await GameSaveData.Instance.SaveData(false);
            // SaveLoad.Save();
        }

        private void CheckDayOpenGame()
        {

        }
    }
}
