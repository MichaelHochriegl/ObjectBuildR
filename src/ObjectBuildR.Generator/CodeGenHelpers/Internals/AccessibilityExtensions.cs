﻿using Microsoft.CodeAnalysis;

namespace CodeGenHelpers.Internals
{
    internal static class AccessibilityExtensions
    {
        public static string Code(this Accessibility accessModifier) =>
            accessModifier switch
            {
                Accessibility.ProtectedAndInternal => "protected internal",
                Accessibility.ProtectedOrInternal => "protected internal",
                Accessibility.NotApplicable => null,
                _ => accessModifier.ToString().ToLower()
            };

        public static string Code(this Accessibility? accessModifier) =>
            accessModifier.HasValue ? accessModifier.Value.Code() : null;

        public static string Code(this Accessibility? accessModifier, Accessibility defaultValue) =>
            accessModifier.HasValue ? accessModifier.Value.Code() : defaultValue.Code();
    }
}
