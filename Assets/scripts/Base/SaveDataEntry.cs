using System;
using JetBrains.Annotations;

namespace GameExtensions
{
    public struct SaveDataEntry
    {
        public string name;
        public string serializedValue;
        public System.Type type;

        public SaveDataEntry([NotNull] string name, [NotNull] string serializedValue, [NotNull] Type type)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.serializedValue = serializedValue ?? throw new ArgumentNullException(nameof(serializedValue));
            this.type = type ?? throw new ArgumentNullException(nameof(type));
        }
    }
}