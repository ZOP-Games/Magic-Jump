using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

namespace GameExtensions
{
    public static class SaveManager
    {

        private static readonly string SavePath = Application.persistentDataPath + "/Saves/";
        private const string SaveName = "Savegame";
        private const string Extension = ".mjsd";

        public static void SaveToFile(string data,byte id)
        {
            var stream = new FileStream(string.Concat(SavePath,SaveName,id,Extension), FileMode.Create);
            var writer = new BinaryWriter(stream);
            writer.Write(data);
            writer.Flush();
            writer.Close();
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
            var sr = new BinaryReader(stream);
            var readData = sr.ReadString();
            sr.Close();
            return readData;
        }

    }
}        
