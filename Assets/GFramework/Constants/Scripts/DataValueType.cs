using System;

namespace GFramework.Constants
{
    public enum DataValueType
    {
        STRING,
        INT,
        FLOAT,
        DOUBLE,
        LONG,
        BOOL,
        VECTOR2,
        VECTOR3,
        QUATERNION,
        COLOR,
    }

    public static class DataValueTypeExtensions
    {
        public static string GetTypeValue(this DataValueType type)
        {
            return type switch
            {
                DataValueType.BOOL => "bool",
                DataValueType.INT => "int",
                DataValueType.FLOAT => "float",
                DataValueType.LONG => "long",
                DataValueType.DOUBLE => "double",
                DataValueType.STRING => "string",
                DataValueType.VECTOR2 => "Vector2",
                DataValueType.VECTOR3 => "Vector3",
                DataValueType.QUATERNION => "Quaternion",
                DataValueType.COLOR => "Color",
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };
        }
        
        public static bool IsStructType(this DataValueType type)
        {
            return type switch
            {
                DataValueType.BOOL => false,
                DataValueType.INT => false,
                DataValueType.FLOAT => false,
                DataValueType.LONG => false,
                DataValueType.DOUBLE => false,
                DataValueType.STRING => false,
                DataValueType.VECTOR2 => true,
                DataValueType.VECTOR3 => true,
                DataValueType.QUATERNION => true,
                DataValueType.COLOR => true,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, null),
            };
        }
    }
}