using System.Linq;
using GameExtensions.Debug;
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
            json = '{' + json + '}';
            SaveManager.SaveToFile(json, id);
        }
        public void Load(string serializedClass)
        {
            JsonUtility.FromJsonOverwrite(serializedClass,this);
        }

        public void AddToList()
        {
            if(SaveManager.Savebles.Any(s => s.GetType() == GetType())) return;
            Id = (byte)SaveManager.Savebles.Count;
            SaveManager.Savebles.Add(this);
            DebugConsole.Log("Added self ("+ GetType() +") to saveable object list: #" + Id);
        }
    }
}