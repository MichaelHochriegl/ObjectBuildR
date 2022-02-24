﻿using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace CodeGenHelpers.Internals
{
    internal static class CodeBuilderExtensions
    {
        private static readonly Dictionary<string, string> _mappings = new Dictionary<string, string>
        {
            { "Boolean", "bool" },
            { "Byte", "byte" },
            { "SByte", "sbyte" },
            { "Char", "char" },
            { "Decimal", "decimal" },
            { "Double", "double" },
            { "Single", "float" },
            { "Int32", "int" },
            { "UInt32", "uint" },
            { "Int64", "long" },
            { "UInt64", "ulong" },
            { "Int16", "short" },
            { "UInt16", "ushort" },
            { "Object", "object" },
            { "String", "string" }
        };

        public static string GetTypeName(this ITypeSymbol symbol)
        {
            if (symbol.ContainingNamespace.Name == "System" && _mappings.ContainsKey(symbol.Name))
                return _mappings[symbol.Name];

            return symbol.GetFullMetadataName();
        }

        public static string GetTypeName(this Type type)
        {
            if (type.Namespace == "System" && _mappings.ContainsKey(type.Name))
                return _mappings[type.Name];

            return type.FullName;
        }
    }
}
