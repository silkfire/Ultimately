namespace Ultimately.Utilities;

using System;
using System.Collections.Generic;
using System.Linq;

internal static class MissingReasons
{
    // https://stackoverflow.com/a/1362899/633098

    private static readonly Dictionary<Type, string> _typeAliases = new()
                                                                    {
                                                                        [typeof(byte)]    = "byte",
                                                                        [typeof(sbyte)]   = "sbyte",
                                                                        [typeof(short)]   = "short",
                                                                        [typeof(ushort)]  = "ushort",
                                                                        [typeof(int)]     = "int",
                                                                        [typeof(uint)]    = "uint",
                                                                        [typeof(long)]    = "long",
                                                                        [typeof(ulong)]   = "ulong",
                                                                        [typeof(float)]   = "float",
                                                                        [typeof(double)]  = "double",
                                                                        [typeof(decimal)] = "decimal",
                                                                        [typeof(object)]  = "object",
                                                                        [typeof(bool)]    = "bool",
                                                                        [typeof(char)]    = "char",
                                                                        [typeof(string)]  = "string",
                                                                        [typeof(void)]    = "void"
                                                                    };


    public const string KeyNotFound = "A key was not found in a dictionary.";

    public const string KeyNotFoundIndexer = "A key was not found in a type implementing an indexer.";

    public const string IndexNotFound = "An element was not found at a given index in a collection.";

    public const string CollectionWasEmpty = "A sequence or collection contained no elements.";

    public const string NoElementsFound = "No element matching the given predicate was found.";

    public static class CouldNotBeParsedAs<T>
    {
        public static string Value { get; } = $"A string could not be parsed as a value of type ´{typeof(T).PrettyName()}´";
    }


    /// <summary>
    /// Returns a pretty name for the type, such as using angle braces for a generic type.
    /// </summary>
    /// <param name="type">The type.</param>
    private static string PrettyName(this Type type)
    {
        if (_typeAliases.TryGetValue(type, out var prettyName))
        {
            return prettyName;
        }

        if (type.GetGenericArguments().Length == 0)
        {
            return type.Name;
        }

        var genericArguments = type.GetGenericArguments();
        var unmangledName = type.JustTypeName();

        return $"{unmangledName}<{string.Join(",", genericArguments.Select(PrettyName).ToArray())}>";
    }

    /// <summary>
    /// Returns the name of the type, without the ` symbol or generic type parameterization.
    /// </summary>
    /// <param name="type">The type.</param>
    private static string JustTypeName(this Type type)
    {
        var typeDefinition = type.Name;
        var indexOf = typeDefinition.IndexOf('`');

        return indexOf < 0 ? typeDefinition : typeDefinition[..indexOf];
    }
}
