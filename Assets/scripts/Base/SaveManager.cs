﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace GameExtensions
{
    public static class SaveManager
    {
        public static List<ISaveable> Savebles { get; } = new();

        public static bool SaveExists => new DirectoryInfo(SavePath).EnumerateFiles().Any(f => f.Extension == Extension);
        private static readonly string SavePath = Application.persistentDataPath + "/Saves/";
        private const string SaveName = "Savegame";
        private const string Extension = ".mjsd";

        public static void SaveToFile(string data, byte id)
        {
            if (!Directory.Exists(SavePath)) Directory.CreateDirectory(SavePath);
            var stream = new FileStream(string.Concat(SavePath,SaveName,id,Extension), FileMode.Create);
            var writer = new StreamWriter(stream);
            writer.Write(data);
            writer.Flush();
            writer.Close();
            stream.Close();
            Debug.Log("saved file to: " + string.Concat(SavePath,SaveName,id,Extension));
        }

        public static string ReadFromFile(byte id)
        {
            var saveFile = string.Concat(SavePath, SaveName, id, Extension);
            if (!File.Exists(saveFile))
            {
                Debug.LogError("The specified save file was not found. Make sure the current savePath (" + saveFile +
                                 ") is correct.");
                throw new FileNotFoundException();
            }
            var stream = new FileStream(saveFile, FileMode.Open);
            var sr = new StreamReader(stream);
            var readData = sr.ReadToEnd();
            sr.Close();
            return readData;
        }

        public static void SaveAll()
        {
            Debug.Log("saving " + Savebles.Count + " objects");
            foreach (var saveable in Savebles)
            {
                Debug.Log("saving " + nameof(saveable));
                saveable.Save();
            }
        }

        public static void LoadAll()
        {
            if (!Player.Instance.isActiveAndEnabled)
            {
                Player.Instance.Refill();
                //UnityEngine.SceneManagement.SceneManager.LoadScene(1);
            }
            foreach (var saveable in Savebles)
            {
                saveable.Load(ReadFromFile(saveable.Id));
            }

            Debug.Log("save has loaded");
        }
    }
}        