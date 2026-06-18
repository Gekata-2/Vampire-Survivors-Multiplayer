using Newtonsoft.Json;
using UnityEngine;

namespace _Project.Scripts.Services
{
    public class SaveLoadService
    {
        private readonly string _key;

        public SaveLoadService(string key = "Save")
        {
         
            _key = key;
        }

        public SaveData Load()
        {
            string json = PlayerPrefs.GetString(_key, "");

            if (string.IsNullOrEmpty(json))
            {
                return null;
            }

            SaveData data = JsonConvert.DeserializeObject<SaveData>(json);
          
            return data;
        }

        public void Save(SaveData data)
        {
            PlayerPrefs.SetString(_key, JsonConvert.SerializeObject(data, Formatting.Indented));
            PlayerPrefs.Save();
        }
    }
}