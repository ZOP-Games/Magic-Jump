using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameExtensions
{
    public class SaveDataManager
    {
        internal SaveDataManager(byte id)
        {
            ownId = id;
        }
        private readonly byte ownId;
        private Dictionary<ISaveable, string> data = new();

        public void AddClass(ISaveable entity)
        {
            var json = entity.Save();
            data.Add(entity, json);
        }

        public void WriteSave()
        {
            var saveString = JsonUtility.ToJson(data);
            SaveManager.SaveToFile(saveString,ownId);
        }

        public void ReadSave()
        {
            data = JsonUtility.FromJson<Dictionary<ISaveable, string>>(SaveManager.ReadFromFile(ownId));
            foreach (var item in data)
            {
                item.Key.Load(item.Value);
            }
        }

    }
}