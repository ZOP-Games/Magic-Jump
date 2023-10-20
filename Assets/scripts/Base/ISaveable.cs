using GameExtensions.Debug;
using System;
using System.Linq;
using UnityEngine;

namespace GameExtensions
{
    public interface ISaveable
    {
        public byte Id { get; protected set; }

        public void Save()
        {
            var id = Id;
            var json = JsonUtility.ToJson(this).Split(',').Where(i => !i.Contains("instanceID"))
                .Aggregate((c, n) => c + "," + n);
            if (!json.StartsWith('{')) json = json.Insert(0, "{");
            if (!json.EndsWith('}')) json = json.Insert(json.Length, "}");
            SaveManager.SaveToFile(json, id);
        }

        public void Load(string serializedClass)
        {
            try
            {
                JsonUtility.FromJsonOverwrite(serializedClass, this);
            }
            catch (Exception e)
            {
                DebugConsole.Log(this + "fak'd up: " + e.Message, DebugConsole.ErrorColor);
            }
        }

        public void AddToList()
        {
            if (SaveManager.Savebles.Any(s => s.GetType() == GetType())) return;
            Id = (byte)SaveManager.Savebles.Count;
            SaveManager.Savebles.Add(this);
            DebugConsole.Log("Added self (" + GetType() + ") to saveable object list: #" + Id);
        }
    }
}