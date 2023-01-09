using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GameExtensions
{
    public class SaveManager
    {
        public static SaveManager Instance => new();

        private readonly List<SaveDataEntry> entries = new();
        private readonly string savePath = Application.persistentDataPath + "/Saves/savegame.mjsf";
        
        public void SaveGame()
        {
            if(entries.Count == 0) return;
            
            var stream = new FileStream(savePath, FileMode.Create);
            var json = JsonUtility.ToJson(entries);
            var writer = new BinaryWriter(stream);
            writer.Write(json);
            writer.Flush();
            writer.Close();
        }

        public void LoadGame()
        {
            if (!File.Exists(savePath))
                Debug.LogWarning("The specified save file was not found. Make sure the current savePath (" + savePath +
                                 ") is correct.");
            else
            {
                var stream = new FileStream(savePath, FileMode.Open);
                var sr = new BinaryReader(stream);
                var saveData = JsonUtility.FromJson<List<SaveDataEntry>>(sr.ReadString());
                //todo:load player state
            }
        }

        public void AddData(string name, string value,System.Type type)
        {
           entries.Add(new SaveDataEntry(name,value,type)); 
        }
    }
}        
