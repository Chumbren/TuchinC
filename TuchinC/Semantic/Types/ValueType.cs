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
    public readonly struct ValueType(string name, TypeValue type)
    {
        public readonly string Name = type == TypeValue.Identifier || name != String.Empty 
            ? name : type.ToString();
        public readonly TypeValue Type = type;

        private static readonly ValueType empty = new(String.Empty, TypeValue.None);
        public static ValueType Empty { get => empty;}
        public override string ToString() => Type switch
        {
            TypeValue.None => "none",
            TypeValue.Nil => "nil",
            TypeValue.Char => "ch",
            TypeValue.String => "str",
            TypeValue.Boolean => "bl",
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

        public ValueType(TypeValue type):this(String.Empty,type)
        { }


        public static ValueType ToPrimitive(object? value)
        {
            if (value == null)
                return new ValueType(TypeValue.Nil);

            if (value is System.ValueType)
            {
                TypeValue type = Primitive(value);
                return new ValueType(type);
            }

            throw new ArgumentOutOfRangeException(value?.ToString(), 
                $"Параметр '{nameof(value)}' должно быть значемым");
        }

        private static TypeValue Primitive(object? value)
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
                throw new ArgumentOutOfRangeException(value?.ToString(),
                    "В качестве примитивного типа могут выступать только литералы");
        }

        public bool IsEmpty() => Type == TypeValue.None; 
        public bool IsNull() => Type == TypeValue.Nil; 
        public bool IsBoolean() => Type == TypeValue.Boolean; 
        public bool IsLiteral() => Type == TypeValue.Char || Type == TypeValue.String;
        public bool IsIdentifier() => Type == TypeValue.Identifier;

       
        public bool IsNumber() => Type switch
        {
            TypeValue.Int8 or TypeValue.Int16 or TypeValue.Int32 or TypeValue.Int64 or TypeValue.Double16 or TypeValue.Double32 or TypeValue.Double64 => true,
            TypeValue.Char or TypeValue.String or TypeValue.Boolean or TypeValue.None or TypeValue.Nil or TypeValue.Identifier => false,
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
                TypeValue.Char or TypeValue.String or TypeValue.Boolean or TypeValue.Int8 or TypeValue.Int16 or TypeValue.Int32 or TypeValue.Int64 or TypeValue.Double16 or TypeValue.Double32 or TypeValue.Double64 => true,
                TypeValue.None or TypeValue.Nil or TypeValue.Identifier => false,
                _ => false,
            };
        
    }
}
