global using ValueType = TuchinC.AST.Semantic.Types.ValueType;


using System;
using System.Collections.Generic;
using System.Text;


namespace TuchinC.AST.Semantic.Types
{
    public enum TypeValue:byte
    {
        None = 0x00,
        Nil = 0x01,
        
        Char = 0x02,
        Boolean = 0x03,
        
        Int8 = 0x04,
        Int16 = 0x05,
        Int32 = 0x06,
        Int64 = 0x07,
        
        Double32 = 0x08,
        Double64 = 0x09,
        Double128 = 0x0A,
        
        String = 0x0B,
        Identifier = 0x0C
    }

    public readonly struct ValueType(string name, object? value , TypeValue type)
    {
        public static readonly List<string> PrimitiveTypes = [
                "bool",
                "char",
                "i8",
                "i16",
                "i32",
                "i64",
                "d32",
                "d64",
                "d128",
                "str",
            ];

        public readonly string Name = name;
        public readonly TypeValue Type = type;
        public readonly object? Value = value;

        public ValueType(object? value):this(value == null ? "nil" : (value.ToString() ?? "nil"), value, CastObjectToType(value))
        {}
        public ValueType(string name, TypeValue type):this(name, null, type)
        { }

        public ValueType(string name):this(name, null, CastStringToType(name))
        { }

        public ValueType(TypeValue type):this(String.Empty, null, type)
        {}

        public ValueType(object? value, TypeValue type):this(String.Empty, value, type)
        {}

        public static TypeValue CastStringToType(string name) => name switch 
        {
            "none" => TypeValue.None,
            "nil" => TypeValue.Nil,
            "bool" => TypeValue.Boolean,
            "char" => TypeValue.Char,
            "i8" => TypeValue.Int8,
            "i16" => TypeValue.Int16,
            "i32" => TypeValue.Int32,
            "i64" => TypeValue.Int64,
            "d32" => TypeValue.Double32,
            "d64" => TypeValue.Double64,
            "d128" => TypeValue.Double128,
            "str" => TypeValue.String,
            _ => TypeValue.Identifier
        };

        public static TypeValue CastObjectToType(object? value)
        {
            if (value == null)
                return TypeValue.Nil;

            TypeValue type = Primitive(value);
            if (type != TypeValue.None && type != TypeValue.Nil)
                return type;



            return type == TypeValue.Identifier ? TypeValue.Identifier : TypeValue.None;
        }


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
            TypeValue.Double32 => "d32",
            TypeValue.Double64 => "d64",
            TypeValue.Double128 => "d128",
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
        public static ValueType ToFloat() => new(TypeValue.Double32);
        public static ValueType ToFloat(float value) => new(value, TypeValue.Double32);
        public static ValueType ToDouble() => new(TypeValue.Double32);
        public static ValueType ToDouble(double value) => new(value, TypeValue.Double32);
        public static ValueType ToDecimal() => new(TypeValue.Double64);
        public static ValueType ToDecimal(decimal value) => new(value, TypeValue.Double64);
        public static ValueType ToIdentifier(string name) => new(name, TypeValue.Identifier);

        public static ValueType ToPrimitive(object? value)
        {
            if (value == null)
                return new ValueType(TypeValue.Nil);

            if (value is System.ValueType || value is string)
            {
                TypeValue type = Primitive(value);
                ValueType result = new(value,type);
                if (!result.IsEmpty()) 
                    return result;
            }

            return ToEmpty();
        }


        public static TypeValue Primitive(object? value)
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
                return TypeValue.Double32;
            else if (value is double)
                return TypeValue.Double32;
            else if (value is decimal)
                return TypeValue.Double64;
            else if (value is null)
                return TypeValue.Nil;
            else
                return TypeValue.None;
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
        public bool IsFloat() => Type == TypeValue.Double32;
        public bool IsDouble() => Type == TypeValue.Double32;
        public bool IsDecimal() => Type == TypeValue.Double64;
       
        //Проверка на классы типов
        public bool IsNumber() => Type switch
        {
            TypeValue.Int8 or TypeValue.Int16 or TypeValue.Int32 or TypeValue.Int64 or 
            TypeValue.Double32 or TypeValue.Double32 or TypeValue.Double64 => true,
            _ => false,
        };

        public bool IsReal() => Type == TypeValue.Double32
           || Type == TypeValue.Double32
           || Type == TypeValue.Double64;

        public bool IsInteger() => Type == TypeValue.Int8
            || Type == TypeValue.Int16 || Type == TypeValue.Int32
            || Type == TypeValue.Int64;

        public bool IsPrimitive() => Type switch
            {
                TypeValue.Char or TypeValue.String or TypeValue.Boolean or
                TypeValue.Int8 or TypeValue.Int16 or TypeValue.Int32 or TypeValue.Int64 or
                TypeValue.Double32 or TypeValue.Double32 or TypeValue.Double64 => true,
                _ => false,
            };
        
    }
}
