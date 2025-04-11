using UnityEngine;

namespace DefaultNamespace.SaveSystem
{
    public class PlayerPrefsSaveSystem : ISaveSystem
    {
        private const string SaveKey = "GameSave";

        public void Save(GameSaveData data)
        {
            var json = JsonUtility.ToJson(data);
            PlayerPrefs.SetString(SaveKey, json);
            PlayerPrefs.Save();
        }

        public GameSaveData Load()
        {
            if (!PlayerPrefs.HasKey(SaveKey)) return null;

            var json = PlayerPrefs.GetString(SaveKey);
            return JsonUtility.FromJson<GameSaveData>(json);
        }

        public bool HasSave() => PlayerPrefs.HasKey(SaveKey);
        public void Clear() => PlayerPrefs.DeleteKey(SaveKey);
    }
}