using System.Linq;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

namespace GameExtensions
{
    public interface ISaveable
    {
        public byte Id { get; protected set; }

        public void Save()
        {
            var id = Id;
            SaveManager.SaveToFile(JsonUtility.ToJson(this), id);
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
            Debug.Log("Added self to saveable object list: #" + Id);
        }
    }
}