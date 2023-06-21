using System;
using System.Collections.Generic;
using UnityEngine;

namespace GFramework.Constants
{
    [Serializable]
    public class ConstantGroup
    {
        [SerializeField] 
        private string _groupName;
        public string GroupName => _groupName;
        [SerializeField] 
        private List<ConstantMap> _values;
        public List<ConstantMap> Values => _values;
    }
}