using GameExtensions.Debug;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace GameExtensions
{
    public static class SaveManager
    {
        private const string SaveName = "Savegame";
        private const string Extension = ".mjsd";
        private static readonly string SavePath = Application.persistentDataPath + "/Saves/";
        public static List<ISaveable> Savebles { get; } = new();

        public static bool SaveExists =>
            new DirectoryInfo(SavePath).EnumerateFiles().Any(f => f.Extension == Extension);

        public static void SaveToFile(string data, byte id)
        {
            if (!Directory.Exists(SavePath)) Directory.CreateDirectory(SavePath);
            var stream = new FileStream(string.Concat(SavePath, SaveName, id, Extension), FileMode.Create);
            var writer = new StreamWriter(stream);
            writer.Write(data);
            writer.Flush();
            writer.Close();
            stream.Close();
            DebugConsole.Log("saved file to: " + string.Concat(SavePath, SaveName, id, Extension), 24);
        }

        private static string ReadFromFile(byte id)
        {
            var saveFile = string.Concat(SavePath, SaveName, id, Extension);
            if (!File.Exists(saveFile))
            {
                DebugConsole.Log("The specified save file was not found. Make sure the current savePath (" + saveFile +
                                 ") is correct.", Color.red);
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
            foreach (var saveable in Savebles) saveable.Save();
        }

        public static void LoadAll()
        {
            DebugConsole.Log("Loading " + Savebles.Count + " objects");
            foreach (var saveable in Savebles) saveable.Load(ReadFromFile(saveable.Id));
            if (Player.Instance is not null && !Player.Instance.isActiveAndEnabled) Player.Instance.Revive();
            DebugConsole.Log("save has loaded");
        }
    }
}