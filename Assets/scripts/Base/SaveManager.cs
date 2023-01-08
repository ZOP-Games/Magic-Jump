using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace GameExtensions
{
    public class SaveManager
    {
        public static SaveManager Instance => new();
        public int LastCutscene { get; set; }
        private readonly string savePath = Application.persistentDataPath + "/Saves/";
        private readonly string mainSavePath = Application.persistentDataPath + "/Saves/savegame.mjsf";
        
        private Player player;
        private List<string> extraJsonData;
        public void SaveGame()
        {
            var saveData = new SaveData(player.transform.position, player.Hp, player.Xp, player.Lvl, LastCutscene);
            var stream = new FileStream(mainSavePath, FileMode.Create);
            var json = JsonUtility.ToJson(saveData);
            var writer = new BinaryWriter(stream);
            writer.Write(json);
            writer.Flush();
            writer.Close();
            if (extraJsonData.Count == 0) return;
            for (var i = 0; i < extraJsonData.Count; i++)
            {
                var extraWriter = new BinaryWriter(new FileStream(savePath + "extra" + i + ".mjsf",FileMode.Create));
                extraWriter.Write(extraJsonData[i]);
                extraWriter.Flush();
                extraWriter.Close();
            }
            
        }

        public void LoadGame()
        {
            if (!File.Exists(savePath))
            {
                Debug.LogWarning("The specified save file was not found. Make sure the current savePath (" + savePath + ") is correct.");
            }
            else
            {
                var sr = new StreamReader(mainSavePath);
                var saveData = JsonUtility.FromJson<SaveData>(sr.ReadToEnd());
                //todo:load player state
            }
        }

        public void AddData(object data)
        {
            var json = JsonUtility.ToJson(data);
            extraJsonData.Add(json);
        }

        public void AddData(string json)
        {
            extraJsonData.Add(json);
        }
        
        public SaveManager()
        {
            Player.PlayerReady += () =>
            {
                player = Player.Instance;
            };
        }
    }
}        
