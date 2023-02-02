using UnityEngine;

namespace GameExtensions
{
    public interface ISaveable
    {
        public string Save()
        {
            return JsonUtility.ToJson(this);
        }
        public void Load(string serializedClass)
        {
            JsonUtility.FromJsonOverwrite(serializedClass,this);
        }
    }
}