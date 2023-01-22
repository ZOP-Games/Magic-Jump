using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameExtensions
{
    public class SaveManager
    {
        private SaveManager() {}
        public static SaveManager Instance => new();

        private readonly string savePath = Application.persistentDataPath + "/Saves/savegame.mjsf";
        
        public void SaveGame()
        {
            var stream = new FileStream(savePath, FileMode.Create);
            //var objects = todo:fix saving
            var json = JsonUtility.ToJson();
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
                //todo:load player state
            }
        }

    }
}        
