﻿using System.Linq;
using UnityEngine;

namespace GameExtensions.UI.Menus
{
    public class Bootstrapper : MonoBehaviour
    {
        private void Awake()
        {
            var saveables = FindObjectsOfType<Object>(true).OfType<ISaveable>();
            foreach (var saveable in saveables) saveable.AddToList();
            SaveManager.LoadAll();
            var astarts = FindObjectsOfType<Object>(true).OfType<IAwakeStart>();
            foreach (var astart in astarts) astart.AwakeStart();
        }

        private void Start()
        {
            var pstarts = FindObjectsOfType<Object>(true).OfType<IPassiveStart>();
            foreach (var pstart in pstarts) pstart.PassiveStart();
            Destroy(gameObject);
        }
    }
}