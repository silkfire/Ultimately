namespace Ultimately.Utilities;

using Reasons;

using System;
using System.Globalization;


/// <summary>
/// A collection of static helper methods for parsing strings into simple types.
/// </summary>
public static class TryParse
{
    private delegate bool ParsingFunction<TValue>(string s, out TValue result);

    private delegate bool ParsingFunctionWithFormat<TValue>(string s, NumberStyles style, IFormatProvider provider, out TValue result);

    private delegate bool DateParsingFunctionWithFormat1<TValue>(string s, IFormatProvider provider, DateTimeStyles styles, out TValue result);

    private delegate bool DateParsingFunctionWithFormat2<TValue>(string s, string format, IFormatProvider provider, DateTimeStyles style, out TValue result);

    private delegate bool DateParsingFunctionWithFormat3<TValue>(string s, string[] formats, IFormatProvider provider, DateTimeStyles style, out TValue result);

    private static Option<TValue> To<TValue>(string s, ParsingFunction<TValue> parsingFunction, Error error = null)
    {
        return parsingFunction(s, out var result) ? result.Some() : Optional.None<TValue>(error ?? MissingReasons.CouldNotBeParsedAs<TValue>.Value);
    }

    private static Option<TValue> To<TValue>(string s, NumberStyles style, IFormatProvider provider, ParsingFunctionWithFormat<TValue> parsingFunctionWithFormat, Error error = null)
    {
        return parsingFunctionWithFormat(s, style, provider, out var result) ? result.Some() : Optional.None<TValue>(error ?? MissingReasons.CouldNotBeParsedAs<TValue>.Value);
    }

    private static Option<TValue> To<TValue>(string s, IFormatProvider provider, DateTimeStyles styles, DateParsingFunctionWithFormat1<TValue> parsingFunctionWithFormat, Error error = null)
    {
        return parsingFunctionWithFormat(s, provider, styles, out var result) ? result.Some() : Optional.None<TValue>(error ?? MissingReasons.CouldNotBeParsedAs<TValue>.Value);
    }

    private static Option<TValue> To<TValue>(string s, string format, IFormatProvider provider, DateTimeStyles style,  DateParsingFunctionWithFormat2<TValue> parsingFunctionWithFormat, Error error = null)
    {
        return parsingFunctionWithFormat(s, format, provider, style, out var result) ? result.Some() : Optional.None<TValue>(error ?? MissingReasons.CouldNotBeParsedAs<TValue>.Value);
    }

    private static Option<TValue> To<TValue>(string s, string[] formats, IFormatProvider provider, DateTimeStyles style, DateParsingFunctionWithFormat3<TValue> parsingFunctionWithFormat, Error error = null)
    {
        return parsingFunctionWithFormat(s, formats, provider, style, out var result) ? result.Some() : Optional.None<TValue>(error ?? MissingReasons.CouldNotBeParsedAs<TValue>.Value);
    }

    /// <summary>
    /// Tries to parse a string into a byte.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<byte> ToByte(string s, Error error = null)
    {
        return To<byte>(s, byte.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a byte.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<byte> ToByte(string s, NumberStyles style, IFormatProvider provider, Error error = null)
    {
        return To<byte>(s, style, provider, byte.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a signed byte.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<sbyte> ToSByte(string s, Error error = null)
    {
        return To<sbyte>(s, sbyte.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a signed byte.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<sbyte> ToSByte(string s, NumberStyles style, IFormatProvider provider, Error error = null)
    {
        return To<sbyte>(s, style, provider, sbyte.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a short.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<short> ToShort(string s, Error error = null)
    {
        return To<short>(s, short.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a short.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<short> ToShort(string s, NumberStyles style, IFormatProvider provider, Error error = null)
    {
        return To<short>(s, style, provider, short.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into an unsigned short.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<ushort> ToUShort(string s, Error error = null)
    {
        return To<ushort>(s, ushort.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into an unsigned short.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<ushort> ToUShort(string s, NumberStyles style, IFormatProvider provider, Error error = null)
    {
        return To<ushort>(s, style, provider, ushort.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into an int.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<int> ToInt(string s, Error error = null)
    {
        return To<int>(s, int.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into an int.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<int> ToInt(string s, NumberStyles style, IFormatProvider provider, Error error = null)
    {
        return To<int>(s, style, provider, int.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into an unsigned int.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<uint> ToUInt(string s, Error error = null)
    {
        return To<uint>(s, uint.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into an unsigned int.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<uint> ToUInt(string s, NumberStyles style, IFormatProvider provider, Error error = null)
    {
        return To<uint>(s, style, provider, uint.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a long.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<long> ToLong(string s, Error error = null)
    {
        return To<long>(s, long.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a long.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<long> ToLong(string s, NumberStyles style, IFormatProvider provider, Error error = null)
    {
        return To<long>(s, style, provider, long.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into an unsigned long.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<ulong> ToULong(string s, Error error = null)
    {
        return To<ulong>(s, ulong.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into an unsigned long.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<ulong> ToULong(string s, NumberStyles style, IFormatProvider provider, Error error = null)
    {
        return To<ulong>(s, style, provider, ulong.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a float.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<float> ToFloat(string s, Error error = null)
    {
        return To<float>(s, float.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a float.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<float> ToFloat(string s, NumberStyles style, IFormatProvider provider, Error error = null)
    {
        return To<float>(s, style, provider, float.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a double.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<double> ToDouble(string s, Error error = null)
    {
        return To<double>(s, double.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a double.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<double> ToDouble(string s, NumberStyles style, IFormatProvider provider, Error error = null)
    {
        return To<double>(s, style, provider, double.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a decimal.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<decimal> ToDecimal(string s, Error error = null)
    {
        return To<decimal>(s, decimal.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a decimal.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<decimal> ToDecimal(string s, NumberStyles style, IFormatProvider provider, Error error = null)
    {
        return To<decimal>(s, style, provider, decimal.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a bool.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<bool> ToBool(string s, Error error = null)
    {
        return To<bool>(s, bool.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a char.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<char> ToChar(string s, Error error = null)
    {
        return To<char>(s, char.TryParse, error);
    }


    /// <summary>
    /// Tries to parse a string into a guid.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<Guid> ToGuid(string s, Error error = null)
    {
        return To<Guid>(s, Guid.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into an enum.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<TEnum> ToEnum<TEnum>(string s, Error error = null)
        where TEnum : struct, Enum
    {
        return To<TEnum>(s, Enum.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into an enum.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<TEnum> ToEnum<TEnum>(string s, bool ignoreCase, Error error = null)
        where TEnum : struct, Enum
    {
        return Enum.TryParse<TEnum>(s, ignoreCase, out var result) ? result.Some() : Optional.None<TEnum>(error ?? MissingReasons.CouldNotBeParsedAs<TEnum>.Value);
    }

    /// <summary>
    /// Tries to parse a string into a datetime.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<DateTime> ToDateTime(string s, Error error = null)
    {
        return To<DateTime>(s, DateTime.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a datetime.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<DateTime> ToDateTime(string s, IFormatProvider provider, DateTimeStyles styles, Error error = null)
    {
        return To<DateTime>(s, provider, styles, DateTime.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a datetime with a specific format.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<DateTime> ToDateTimeExact(string s, string format, IFormatProvider provider, DateTimeStyles styles, Error error = null)
    {
        return To<DateTime>(s, format, provider, styles, DateTime.TryParseExact, error);
    }

    /// <summary>
    /// Tries to parse a string into a datetime with a specific format.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<DateTime> ToDateTimeExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles styles, Error error = null)
    {
        return To<DateTime>(s, formats, provider, styles, DateTime.TryParseExact, error);
    }

    /// <summary>
    /// Tries to parse a string into a timespan.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<TimeSpan> ToTimeSpan(string s, Error error = null)
    {
        return To<TimeSpan>(s, TimeSpan.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a timespan.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<TimeSpan> ToTimeSpan(string s, IFormatProvider provider, Error error = null)
    {
        return TimeSpan.TryParse(s, provider, out var result) ? result.Some() : Optional.None<TimeSpan>(error ?? MissingReasons.CouldNotBeParsedAs<TimeSpan>.Value);
    }

    /// <summary>
    /// Tries to parse a string into a timespan with a specific format.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<TimeSpan> ToTimeSpanExact(string s, string format, IFormatProvider provider, Error error = null)
    {
        return TimeSpan.TryParseExact(s, format, provider, out var result) ? result.Some() : Optional.None<TimeSpan>(error ?? MissingReasons.CouldNotBeParsedAs<TimeSpan>.Value);
    }

    /// <summary>
    /// Tries to parse a string into a timespan with a specific format.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<TimeSpan> ToTimeSpanExact(string s, string[] formats, IFormatProvider provider, Error error = null)
    {
        return TimeSpan.TryParseExact(s, formats, provider, out var result) ? result.Some() : Optional.None<TimeSpan>(error ?? MissingReasons.CouldNotBeParsedAs<TimeSpan>.Value);
    }

    /// <summary>
    /// Tries to parse a string into a timespan with a specific format.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<TimeSpan> ToTimeSpanExact(string s, string format, IFormatProvider provider, TimeSpanStyles styles, Error error = null)
    {
        return TimeSpan.TryParseExact(s, format, provider, styles, out var result) ? result.Some() : Optional.None<TimeSpan>(error ?? MissingReasons.CouldNotBeParsedAs<TimeSpan>.Value);
    }

    /// <summary>
    /// Tries to parse a string into a timespan with a specific format.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<TimeSpan> ToTimeSpanExact(string s, string[] formats, IFormatProvider provider, TimeSpanStyles styles, Error error = null)
    {
        return TimeSpan.TryParseExact(s, formats, provider, styles, out var result) ? result.Some() : Optional.None<TimeSpan>(error ?? MissingReasons.CouldNotBeParsedAs<TimeSpan>.Value);
    }

    /// <summary>
    /// Tries to parse a string into a datetime offset.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<DateTimeOffset> ToDateTimeOffset(string s, Error error = null)
    {
        return To<DateTimeOffset>(s, DateTimeOffset.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a datetime offset.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<DateTimeOffset> ToDateTimeOffset(string s, IFormatProvider provider, DateTimeStyles styles, Error error = null)
    {
        return To<DateTimeOffset>(s, provider, styles, DateTimeOffset.TryParse, error);
    }

    /// <summary>
    /// Tries to parse a string into a datetime offset with a specific format.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<DateTimeOffset> ToDateTimeOffsetExact(string s, string format, IFormatProvider provider, DateTimeStyles styles, Error error = null)
    {
        return To<DateTimeOffset>(s, format, provider, styles, DateTimeOffset.TryParseExact, error);
    }

    /// <summary>
    /// Tries to parse a string into a datetime offset with a specific format.
    /// </summary>
    /// <returns>An optional value containing the result if any.</returns>
    public static Option<DateTimeOffset> ToDateTimeOffsetExact(string s, string[] formats, IFormatProvider provider, DateTimeStyles styles, Error error = null)
    {
        return To<DateTimeOffset>(s, formats, provider, styles, DateTimeOffset.TryParseExact, error);
    }
}
