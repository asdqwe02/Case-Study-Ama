using System;
using System.Globalization;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GFramework.Constants
{
    [Serializable]
    public class ConstantMap
    {
        [SerializeField]
        private string _valueName;
        public string ValueName => _valueName;

        [EnumToggleButtons] 
        [SerializeField] 
        private DataValueType _type;
        public DataValueType Type => _type;
        
        [ShowIf("Type", DataValueType.INT)]
        [SerializeField]
        private int _intValue;
        [ShowIf("Type", DataValueType.FLOAT)]
        [SerializeField]
        private float _floatValue;
        [ShowIf("Type", DataValueType.DOUBLE)]
        [SerializeField]
        private double _doubleValue;
        [ShowIf("Type", DataValueType.LONG)]
        [SerializeField]
        private long _longValue;
        [ShowIf("Type", DataValueType.BOOL)]
        [SerializeField]
        private bool _boolValue;
        [ShowIf("Type", DataValueType.STRING)]
        [SerializeField]
        private string _stringValue;
        [ShowIf("Type", DataValueType.VECTOR2)]
        [SerializeField]
        private Vector2 _vector2Value;
        [ShowIf("Type", DataValueType.VECTOR3)]
        [SerializeField]
        private Vector3 _vector3Value;
        [ShowIf("Type", DataValueType.QUATERNION)]
        [SerializeField]
        private Quaternion _quaternionValue;
        [ShowIf("Type", DataValueType.COLOR)]
        [SerializeField]
        private Color _colorValue;

        public string ValueString
        {
            get
            {
                return Type switch
                {
                    DataValueType.BOOL => _boolValue ? "true" : "false",
                    DataValueType.INT => _intValue.ToString(),
                    DataValueType.FLOAT => _floatValue.ToString(CultureInfo.InvariantCulture) + "f",
                    DataValueType.LONG => _longValue.ToString(),
                    DataValueType.DOUBLE => _doubleValue.ToString(CultureInfo.InvariantCulture),
                    DataValueType.STRING => _stringValue,
                    DataValueType.VECTOR2 => $"new Vector2({_vector2Value.x}f, {_vector2Value.y}f)",
                    DataValueType.VECTOR3 => $"new Vector3({_vector3Value.x}f, {_vector3Value.y}f, {_vector3Value.z}f)",
                    DataValueType.QUATERNION => $"new Quaternion({_quaternionValue.x}f, {_quaternionValue.y}f, {_quaternionValue.z}f, {_quaternionValue.w}f)",
                    DataValueType.COLOR => $"new Color({_colorValue.r}f, {_colorValue.g}f, {_colorValue.b}f, {_colorValue.a}f)",

                    _ => throw new ArgumentOutOfRangeException()
                };
            }
        }

    }
}