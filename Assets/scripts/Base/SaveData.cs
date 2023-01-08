using UnityEngine;

namespace GameExtensions
{
    [System.Serializable]
    public record SaveData
    {
        public float[] position;
        public int health;
        public int experience;
        public int level;
        public int lastCutscene;
        public SaveData(Vector3 pos, int hp, int xp, int lvl, int lastCs)
        {
            position = new[] {pos.x, pos.y, pos.z};
            health = hp;
            experience = xp;
            level = lvl;
            lastCutscene = lastCs;
        }
    }
}