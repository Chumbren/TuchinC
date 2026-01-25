global using ValueType = TuchinC.Semantic.Types.ValueType;


using System;
using System.Collections.Generic;
using System.Text;


namespace TuchinC.Semantic.Types
{
    public enum TypeValue
    {
        None = 0,
        Nil,
        Char,
        String,
        Boolean,
        Int8,
        Int16,
        Int32,
        Int64,
        Double16,
        Double32,
        Double64,
        Identifier
    }
    public readonly struct ValueType(string name, object? value , TypeValue type)
    {
        public readonly string Name = name;
        public readonly TypeValue Type = type;
        public readonly object? Value = value;

        public ValueType(TypeValue type):this(String.Empty,type)
        { }

        public ValueType(object? value, TypeValue type):this(String.Empty, value, type)
        {}




        public override string ToString() => Type switch
        {
            TypeValue.None => "none",
            TypeValue.Nil => "nil",
            TypeValue.Char => "char",
            TypeValue.String => "str",
            TypeValue.Boolean => "bool",
            TypeValue.Int8 => "i8",
            TypeValue.Int16 => "i16",
            TypeValue.Int32 => "i32",
            TypeValue.Int64 => "i64",
            TypeValue.Double16 => "d16",
            TypeValue.Double32 => "d32",
            TypeValue.Double64 => "d64",
            TypeValue.Identifier => Name,
            _ => "",
        };



        public static ValueType ToEmpty() => new(String.Empty, TypeValue.None);
        public static ValueType ToNull() => new(String.Empty, null, TypeValue.None);
        public static ValueType ToChar() => new(TypeValue.Char);
        public static ValueType ToChar(char value) => new(value, TypeValue.Char);
        public static ValueType ToString(string value) => new(value, TypeValue.String);
        public static ValueType GetToString() => new(TypeValue.String);
        public static ValueType ToBoolean() => new(TypeValue.Boolean);
        public static ValueType ToBoolean(bool value) => new(value, TypeValue.Boolean);
        public static ValueType ToByte() => new(TypeValue.Int8);
        public static ValueType ToByte(byte value) => new(value, TypeValue.Int8);
        public static ValueType ToShort() => new(TypeValue.Int16);
        public static ValueType ToShort(short value) => new(value, TypeValue.Int16);
        public static ValueType ToInt() => new(TypeValue.Int32);
        public static ValueType ToInt(int value) => new(value, TypeValue.Int32);
        public static ValueType ToLong() => new(TypeValue.Int64);
        public static ValueType ToLong(long value) => new(value, TypeValue.Int64);
        public static ValueType ToFloat() => new(TypeValue.Double16);
        public static ValueType ToFloat(float value) => new(value, TypeValue.Double16);
        public static ValueType ToDouble() => new(TypeValue.Double32);
        public static ValueType ToDouble(double value) => new(value, TypeValue.Double32);
        public static ValueType ToDecimal() => new(TypeValue.Double64);
        public static ValueType ToDecimal(decimal value) => new(value, TypeValue.Double64);
        public static ValueType ToIdentifier() => new(TypeValue.Identifier);
        public static ValueType ToIdentifier(object value) => new(value, TypeValue.Identifier);

        public static ValueType ToPrimitive(object? value)
        {
            if (value == null)
                return new ValueType(TypeValue.Nil);

            if (value is System.ValueType || value is string)
            {
                TypeValue? type = Primitive(value);
                if (type != null) return new ValueType((TypeValue)type);
            }

            throw new ArgumentOutOfRangeException(value?.ToString(), 
                $"Параметр '{nameof(value)}' должен быть значимым");
        }

        private static TypeValue? Primitive(object? value)
        {
            if (value is char)
                return TypeValue.Char;
            else if (value is string)
                return TypeValue.String;
            else if (value is bool)
                return TypeValue.Boolean;
            else if (value is byte)
                return TypeValue.Int8;
            else if (value is short)
                return TypeValue.Int16;
            else if (value is int)
                return TypeValue.Int32;
            else if (value is long)
                return TypeValue.Int64;
            else if (value is float)
                return TypeValue.Double16;
            else if (value is double)
                return TypeValue.Double32;
            else if (value is decimal)
                return TypeValue.Double64;
            else
                return null;
        }

        public bool IsEmpty() => Type == TypeValue.None; 
        public bool IsNull() => Type == TypeValue.Nil; 
        public bool IsBoolean() => Type == TypeValue.Boolean; 
        public bool IsAlpha() => Type == TypeValue.Char 
            || Type == TypeValue.String;
        public bool IsIdentifier() => Type == TypeValue.Identifier;

      
        //Проверка на строковые значения
        public bool IsChar() => Type == TypeValue.Char;
        public bool IsString() => Type == TypeValue.String;

        //Проверка на целого числа
        public bool IsInt8() => Type == TypeValue.Int8;
        public bool IsInt16() => Type == TypeValue.Int16;
        public bool IsInt32() => Type == TypeValue.Int32;
        public bool IsInt64() => Type == TypeValue.Int64;

        //Проверка на вещественные числа 
        public bool IsFloat() => Type == TypeValue.Double16;
        public bool IsDouble() => Type == TypeValue.Double32;
        public bool IsDecimal() => Type == TypeValue.Double64;
       
        //Проверка на классы типов
        public bool IsNumber() => Type switch
        {
            TypeValue.Int8 or TypeValue.Int16 or TypeValue.Int32 or TypeValue.Int64 or 
            TypeValue.Double16 or TypeValue.Double32 or TypeValue.Double64 => true,
            _ => false,
        };

        public bool IsReal() => Type == TypeValue.Double16
           || Type == TypeValue.Double32
           || Type == TypeValue.Double64;

        public bool IsInteger() => Type == TypeValue.Int8
            || Type == TypeValue.Int16 || Type == TypeValue.Int32
            || Type == TypeValue.Int64;

        public bool IsPrimitive() => Type switch
            {
                TypeValue.Char or TypeValue.String or TypeValue.Boolean or
                TypeValue.Int8 or TypeValue.Int16 or TypeValue.Int32 or TypeValue.Int64 or
                TypeValue.Double16 or TypeValue.Double32 or TypeValue.Double64 => true,
                _ => false,
            };
        
    }
}
