using System;
using System.Collections.Generic;
using UnityEngine;

namespace GFramework.Macro
{
    [System.Serializable]
    public class MacroData : ScriptableObject
    {
        [SerializeField]
        private List<string> _macros = new();

        public List<string> Macros => _macros;

        [HideInInspector] public bool UseOdin;
        [HideInInspector] public List<string> OdinMacros;
    }
}