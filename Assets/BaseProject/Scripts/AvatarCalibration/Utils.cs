using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;

#if UNITY_EDITOR
using System.Linq;
#endif

[System.Serializable]
public struct Int_Range
{
    public int min;
    public int max;

    public Int_Range(int minimum, int maximum)
    {
        min = minimum;
        max = maximum;
    }
}

[System.Serializable]
public struct Float_Range
{
    public float min;
    public float max;

    public Float_Range(float minimum, float maximum)
    {
        min = minimum;
        max = maximum;
    }
}

public class CoroutineValue<T>
{
    private IEnumerator _coroutine;
    private T value;
    public IEnumerator WaitForValue(IEnumerator coroutine = null)
    {
        if (coroutine != null)
            _coroutine = coroutine;

        yield return _coroutine;

        var v = _coroutine.Current;

        while (!(v is T) && v != null)
            v = (v as IEnumerator).Current;

        if (v is T)
            value = (T)v;

        yield break;
    }

    public T Value { get => value; }

    public CoroutineValue(IEnumerator coroutine)
    {
        _coroutine = coroutine;
        value = default(T);
    }
}

public static class Utils
{
    public enum BitmaskOp
    {
        NONE,
        REPLACE,
        AND,
        OR,
        XOR,
        COMPLIMENT
    }

    public static readonly char[][] PaddedInts = 
        {
            new char[] { '0', '0' },
            new char[] { '0', '1' },
            new char[] { '0', '2' },
            new char[] { '0', '3' },
            new char[] { '0', '4' },
            new char[] { '0', '5' },
            new char[] { '0', '6' },
            new char[] { '0', '7' },
            new char[] { '0', '8' },
            new char[] { '0', '9' },
            new char[] { '1', '0' },
            new char[] { '1', '1' },
            new char[] { '1', '2' },
            new char[] { '1', '3' },
            new char[] { '1', '4' },
            new char[] { '1', '5' },
            new char[] { '1', '6' },
            new char[] { '1', '7' },
            new char[] { '1', '8' },
            new char[] { '1', '9' },
            new char[] { '2', '0' },
            new char[] { '2', '1' },
            new char[] { '2', '2' },
            new char[] { '2', '3' },
            new char[] { '2', '4' },
            new char[] { '2', '5' },
            new char[] { '2', '6' },
            new char[] { '2', '7' },
            new char[] { '2', '8' },
            new char[] { '2', '9' },
            new char[] { '3', '0' },
            new char[] { '3', '1' },
            new char[] { '3', '2' },
            new char[] { '3', '3' },
            new char[] { '3', '4' },
            new char[] { '3', '5' },
            new char[] { '3', '6' },
            new char[] { '3', '7' },
            new char[] { '3', '8' },
            new char[] { '3', '9' },
            new char[] { '4', '0' },
            new char[] { '4', '1' },
            new char[] { '4', '2' },
            new char[] { '4', '3' },
            new char[] { '4', '4' },
            new char[] { '4', '5' },
            new char[] { '4', '6' },
            new char[] { '4', '7' },
            new char[] { '4', '8' },
            new char[] { '4', '9' },
            new char[] { '5', '0' },
            new char[] { '5', '1' },
            new char[] { '5', '2' },
            new char[] { '5', '3' },
            new char[] { '5', '4' },
            new char[] { '5', '5' },
            new char[] { '5', '6' },
            new char[] { '5', '7' },
            new char[] { '5', '8' },
            new char[] { '5', '9' },
            new char[] { '6', '0' },
            new char[] { '6', '1' },
            new char[] { '6', '2' },
            new char[] { '6', '3' },
            new char[] { '6', '4' },
            new char[] { '6', '5' },
            new char[] { '6', '6' },
            new char[] { '6', '7' },
            new char[] { '6', '8' },
            new char[] { '6', '9' },
            new char[] { '7', '0' },
            new char[] { '7', '1' },
            new char[] { '7', '2' },
            new char[] { '7', '3' },
            new char[] { '7', '4' },
            new char[] { '7', '5' },
            new char[] { '7', '6' },
            new char[] { '7', '7' },
            new char[] { '7', '8' },
            new char[] { '7', '9' },
            new char[] { '8', '0' },
            new char[] { '8', '1' },
            new char[] { '8', '2' },
            new char[] { '8', '3' },
            new char[] { '8', '4' },
            new char[] { '8', '5' },
            new char[] { '8', '6' },
            new char[] { '8', '7' },
            new char[] { '8', '8' },
            new char[] { '8', '9' },
            new char[] { '9', '0' },
            new char[] { '9', '1' },
            new char[] { '9', '2' },
            new char[] { '9', '3' },
            new char[] { '9', '4' },
            new char[] { '9', '5' },
            new char[] { '9', '6' },
            new char[] { '9', '7' },
            new char[] { '9', '8' },
            new char[] { '9', '9' }
        };

    public static readonly char[][] UnpaddedInts =
    {
            new char[] { '0' },
            new char[] { '1' },
            new char[] { '2' },
            new char[] { '3' },
            new char[] { '4' },
            new char[] { '5' },
            new char[] { '6' },
            new char[] { '7' },
            new char[] { '8' },
            new char[] { '9' }
    };

    private static readonly uint[] UIntPairScales = { 100000000, 1000000, 10000, 100 };

    public static void AppendInvariant(this System.Text.StringBuilder builder, int intValue)
    {
        AppendInvariant(builder, (long)intValue);
    }

    public static void AppendInvariant(this System.Text.StringBuilder builder, long intValue)
    {
        uint value;

        if (intValue < 0)
        {
            value = (uint)(intValue * -1);
            builder.Append('-');
        }
        else
            value = (uint)intValue;

        bool next = false;
        foreach (var scale in UIntPairScales)
        {
            if (value >= scale)
            {
                uint pair = value / scale;
                if (!next && pair < 10)
                    builder.Append((char)('0' + pair));
                else
                    builder.Append(PaddedInts[pair]);
                value -= pair * scale;
                next = true;
            }
            else if (next)
            {
                builder.Append("00");
            }
        }
        if (!next && value < 10)
            builder.Append((char)('0' + value));
        else
            builder.Append(PaddedInts[value]);
    }

    public static char[] GetIntPadded(int intValue)
    {
        if (intValue < 0)
            intValue = 0;

        intValue %= 100;
        return PaddedInts[intValue];
    }

    public static char[] GetIntUnpadded(int intValue)
    {
        if (intValue < 0)
            intValue = 0;

        intValue %= 100;

        if (intValue < 10)
            return UnpaddedInts[intValue];
        else
            return PaddedInts[intValue];
    }

    /// <summary>
    /// Splits a floating point value into an integer part and a decimal part,
    /// both represented as integers. The decimal part is taken to the specified 
    /// number of decimal places. The sign is retianed for both parts.
    /// </summary>
    /// <param name="value">The value to split.</param>
    /// <param name="intPart">Will hold the integer part.</param>
    /// <param name="decimalPart">Will hold the decimal part to the specified decimal places.</param>
    /// <param name="decimalPlaces">The number of decimal places to get the decimal part.</param>
    /// <param name="reduce">If true, trailing zeroes are cut from the decimal part.</param>
    public static void SplitFloat(double value, out int intPart, out int decimalPart, int decimalPlaces, bool reduce = true)
    {
        double frac = value - Math.Floor(value);
        intPart = (int)(value - frac);
        decimalPart = (int)(frac * Math.Pow(10, decimalPlaces));

        // Reduce:
        if (reduce)
        {
            while (decimalPart % 10 == 0 && decimalPart != 0)
                decimalPart /= 10;
        }
    }

    /// <summary>
    /// Splits a floating point value into an integer part and a decimal part,
    /// both represented as integers. The decimal part is taken to the specified 
    /// number of decimal places. The sign is retianed for both parts.
    /// The minimum number of decimal places are used up to, but not exceeding,
    /// the specified maxDecimalPlaces.
    /// </summary>
    /// <param name="value">The value to split.</param>
    /// <param name="intPart">Will hold the integer part.</param>
    /// <param name="decimalPart">Will hold the decimal part to the specified decimal places.</param>
    /// <param name="decimalPlaces">The number of decimal places to get the decimal part.</param>
    public static void SplitFloatMin(double value, out int intPart, out int decimalPart, int maxDecimalPlaces = int.MaxValue)
    {
        double temp = 0;
        double fracPart = value - Math.Floor(value);

        intPart = (int)(value - fracPart);

        for (int dec=0; dec < maxDecimalPlaces; ++dec)
        {
            fracPart *= 10d;
            temp = fracPart - Math.Floor(fracPart);

            if (temp < 0.01)
                break;
        }

        decimalPart = (int)Math.Floor(fracPart);
    }

    /// <summary>
    /// Assigns the content of a string to a TextMeshPro object, replacing unsupported characters with
    /// the specified replacement character if the associated font does not support any characters.
    /// </summary>
    /// <param name="str">The string to assign to the TextMeshPro</param>
    /// <param name="replacementChar">Character to use to replace any characters that are unsupported by the associated font.</param>
    /// <param name="tmPro">The TextMeshPro object to be assigned the specified string.</param>
    /// <returns>True if replacements had to be made, false otherwise.</returns>
    public static bool FilterUnsupportedChars(ref string str, char replacementChar, TextMeshPro tmPro)
    {
        List<char> missing;
        char[] newChars = null;
        
        if (!tmPro.font.HasCharacters(str, out missing))
        {
            newChars = new char[str.Length];

            for (int i=0; i < str.Length; ++i)
            {
                newChars[i] = str[i];

                for (int j=0; j < missing.Count; ++j)
                {
                    if (str[i] == missing[j])
                    {
                        newChars[i] = replacementChar;
                        break;
                    }
                }
            }

            tmPro.SetCharArray(newChars);
            return true;
        }

        tmPro.text = str;
        return false;
    }

    static public bool IsInLayerMask(int layer, int mask)
    {
        return BitPositionIsSet(mask, layer);
    }

    static public bool IsInLayerMask(GameObject go, int mask)
    {
        return IsInLayerMask(go.layer, mask);
    }

    static public bool IsInLayerMask(GameObject go, LayerMask mask)
    {
        return IsInLayerMask(go.layer, mask.value);
    }

    static public bool BitfieldContainsAny(int field, int test)
    {
        return (field & test) != 0;
    }

    static public bool BitfieldContainsAny(uint field, uint test)
    {
        return (field & test) != 0;
    }

    static public bool BitfieldContainsAll(int field, int test)
    {
        return (field & test) == test;
    }

    /// <summary>
    /// Returns true if the bit at the specified position is set.
    /// </summary>
    /// <param name="field">The bitfield</param>
    /// <param name="bitPosition">The position of the bit to be checked (1 will be left-shifted) by this number</param>
    /// <returns></returns>
    static public bool BitPositionIsSet(int field, int bitPosition)
    {
        return (((1 << bitPosition) & field) != 0);
    }

    static public string SecondsToSongTime(float totalSeconds)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60f);
        int seconds = Mathf.FloorToInt(totalSeconds - (minutes * 60f));
        int ms = Mathf.FloorToInt((totalSeconds - (float)Mathf.FloorToInt(totalSeconds)) * 60f);
        return string.Format("{0}:{1}.{2}", minutes, seconds.ToString("D2"), ms.ToString("D2"));
    }

    static public void SecondsToSongTime(float totalSeconds, System.Text.StringBuilder sb)
    {
        int minutes = Mathf.FloorToInt(totalSeconds / 60f);
        int seconds = Mathf.FloorToInt(totalSeconds - (minutes * 60f));
        int ms = Mathf.FloorToInt((totalSeconds - (float)Mathf.FloorToInt(totalSeconds)) * 60f);
        sb.AppendFormat("{0}:{1}.{2}", minutes, seconds.ToString("D2"), ms.ToString("D2"));
    }

    /// <summary>
    /// Converts the number of seconds to a leading 0-padded character array.
    /// NOTE: Only supports values from 00 through 59. Values beyond that will
    /// loop (via modulus operation) in this range.
    /// </summary>
    /// <param name="seconds">The number of seconds</param>
    /// <returns>A character array representation of the number of seconds with leading 0 padding.</returns>
    static public char[] GetSecondsPadded(int seconds)
    {
        seconds %= 60;
        return GetIntPadded(seconds);
    }

    /// <summary>
    /// Avoids string allocations by copying the contents of a StringBuilder into a TextMeshPro using a character array.
    /// </summary>
    /// <param name="textMeshPro">The TextMesh Pro object to update.</param>
    /// <param name="sb">A StringBuilder containing the text to be copied</param>
    /// <param name="charBuffer">A temporary character buffer -- must be at least the length of the string about to be copied.</param>
    static public void UpdateTMProTextFromStringBuilder(TMPro.TMP_Text textMeshPro, System.Text.StringBuilder sb, char[] charBuffer)
    {
        if (charBuffer.Length < sb.Length)
        {
            throw new System.Exception("Character buffer not large enough to hold string!");
        }

        sb.CopyTo(0, charBuffer, 0, sb.Length);
        textMeshPro.SetCharArray(charBuffer, 0, sb.Length);
        textMeshPro.SetVerticesDirty();
    }

    static public float QuantizeValue(float val, float quantVal)
    {
        if (quantVal == 0)
            return val;

        return RoundToNearest(val, quantVal);
/*
        float sign = val > 0 ? 1f : -1f;

        val = Mathf.Abs(val);
        val = val - Mathf.Repeat(val, quantVal);
        return val * sign;
*/
    }

    static public int QuantizeValue(int val, int quantVal)
    {
        if (quantVal == 0)
            return val;

        int sign = val > 0 ? 1 : -1;

        val = Mathf.Abs(val);
        val = val - (val % quantVal);
        return val * sign;
    }

    static public Vector3 QuantizeXY(Vector3 pos, float quantVal)
    {
        pos.x = QuantizeValue(pos.x, quantVal);
        pos.y = QuantizeValue(pos.y, quantVal);
        return pos;
    }

    /// <summary>
    /// Divides a value into chunks of the specified size and apportioning the remainder up into each chunk
    /// so that as much as possible, each chunk is of uniform size with minimal remainder. The value returned
    /// is the actual adjusted chunk size that gives the most even distribution of the total value.
    /// 
    /// Example values:
    /// total   chunkSize   returns     remainder
    /// 19      7           9           1
    /// 20      7           10          0
    /// 21      7           7           0
    /// 22      7           7           1
    /// 23      7           7           2
    /// 24      8           8           0
    /// </summary>
    /// <param name="total">The total value to be divided into chunks</param>
    /// <param name="chunkSize">The starting chunk size - the resulting chunk size will always be equal to or greater than this value, and less than 2x this value.</param>
    /// <returns>The size of each chunk with even distribution of the remainder</returns>
    static public int Chunkify(int total, int chunkSize)
    {
        int remainder = total % chunkSize;
        int additionalPerChunk = remainder / (total / chunkSize);
        return chunkSize + additionalPerChunk;
    }

    /// <summary>
    /// Divides a value into chunks of the specified size and apportioning the remainder up into each chunk
    /// so that as much as possible, each chunk is of uniform size with minimal remainder. The value returned
    /// is the actual adjusted chunk size that gives the most even distribution of the total value.
    /// 
    /// This is similar to the integer version of Chunkify, but the remainder will always be fractional and very small.
    /// </summary>
    /// <param name="total">The total value to be divided into chunks</param>
    /// <param name="chunkSize">The starting chunk size - the resulting chunk size will always be equal to or greater than this value, and less than 2x this value.</param>
    /// <returns>The size of each chunk with even distribution of the remainder</returns>
    static public double Chunkify(double total, double chunkSize)
    {
        double remainder = Modulo(total, chunkSize);
        double additionalPerChunk = remainder / (int)Math.Floor(total / chunkSize);
        return chunkSize + additionalPerChunk;
    }

    /// <summary>
    /// Floating point/Double modulo.
    /// </summary>
    /// <param name="a">Numerator</param>
    /// <param name="b">Denominator</param>
    /// <returns>The remainder</returns>
    public static double Modulo(double a, double b)
    {
        return a - b * (double)Math.Floor(a / b);
    }

    static public float RoundToNearest(float num, float multiple)
    {
        return Mathf.RoundToInt(num / multiple) * multiple;
    }

    static public float RoundDownTo(float num, float multiple)
    {
        return Mathf.FloorToInt(num / multiple) * multiple;
    }

    static public float RoundUpTo(float num, float multiple)
    {
        return Mathf.CeilToInt(num / multiple) * multiple;
    }

    /// <summary>
    /// Finds how many divisions width must have been divided into to
    /// arrive at value (which is a multiple of that divisor).
    /// Threshold allows for a certain inaccuracy.
    /// Example: Let width be the number of samples for a single musical
    /// beat. Let value be the sample of a given note. This method allows
    /// you to determine whether value is, say, a whole, half, quarter, etc
    /// beat note. Use the threshold argument to make sure that small 
    /// inaccuracies in the beat placement don't result in ridiculous divisors
    /// such as 1/256th beat or something.
    /// </summary>
    /// <param name="width">The width of the "whole"</param>
    /// <param name="value">The value that is some multiple of an unknown division of the whole.</param>
    /// <param name="step">The value by which the divisor will be incremented each iteration. Ex: 1 will look for the divisor at width/1, width/2, width/3, etc. 2 will look at width/1, width/2, width/4, etc.</param>
    /// <param name="threshold"></param>
    /// <returns></returns>
    static public int FindNearestDivisor(int width, int value, int step, int threshold)
    {
        int divisor = 1;
        value = value % width;

        while (value % (width / divisor) > threshold)
        {
            divisor *= step;
        }

        return divisor;
    }

    static public int Lerp(int start, int end, double pct)
    {
        int diff = end - start;
        return start + (int)(diff * pct);
    }

    static public T DeepCloneSerializable<T>(T obj, params ITypedSerializationSurrogate[] surrogates)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();

            // Check for surrogates:
            if (surrogates != null && surrogates.Length > 0)
            {
                SurrogateSelector surrogateSelector = new SurrogateSelector();

                for (int i=0; i < surrogates.Length; ++i)
                {
                    surrogateSelector.AddSurrogate(surrogates[i].SerializedType, new StreamingContext(StreamingContextStates.All), surrogates[i]);
                }

                formatter.SurrogateSelector = surrogateSelector;
            }

            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T)formatter.Deserialize(ms);
        }
    }

    public static T CopyComponent<T>(T original, GameObject destination) where T : Component
    {
        System.Type type = original.GetType();
        Component copy = destination.AddComponent(type);
        System.Reflection.FieldInfo[] fields = type.GetFields();
        foreach (System.Reflection.FieldInfo field in fields)
        {
            field.SetValue(copy, field.GetValue(original));
        }
        return copy as T;
    }

    #region extension methods
    // String extension method:
    public static bool CaseInsensitiveContains(this string text, string value,
    System.StringComparison stringComparison = System.StringComparison.CurrentCultureIgnoreCase)
    {
        return text.IndexOf(value, stringComparison) >= 0;
    }

    /// <summary>
    /// Searches an array of strings to see if any of its elements exactly match
    /// (with or without case-sensitivity, depending on the comparer used).
    /// </summary>
    /// <param name="strs">The array of strings to search</param>
    /// <param name="value">The string to match</param>
    /// <param name="stringComparison">Optional string comparer. Defaults to current-culture, case-insensitive.</param>
    /// <returns>True if a match is found, false otherwise.</returns>
    public static bool ContainsExact(this string[] strs, string value, System.StringComparison stringComparison = System.StringComparison.CurrentCultureIgnoreCase)
    {
        for (int i=0; i < strs.Length; ++i)
        {
            if (strs[i].Equals(value, stringComparison))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Searches an array of strings to see if any of its elements contain
    /// the sought substring (with or without case-sensitivity, depending on the comparer used).
    /// </summary>
    /// <param name="strs">The array of strings to search</param>
    /// <param name="value">The string to match</param>
    /// <param name="stringComparison">Optional string comparer. Defaults to current-culture, case-insensitive.</param>
    /// <returns>True if a match is found, false otherwise.</returns>
    public static bool ContainsSubStr(this string[] strs, string value, System.StringComparison stringComparison = System.StringComparison.CurrentCultureIgnoreCase)
    {
        for (int i = 0; i < strs.Length; ++i)
        {
            if (CaseInsensitiveContains(strs[i], value, stringComparison))
                return true;
        }

        return false;
    }

    /// <summary>
    /// Converts an int value to characters and stores them in the buffer supplied.
    /// </summary>
    /// <param name="value">The value to convert</param>
    /// <param name="charArray">The buffer in which to store the character/string representation of the integer value. If null, the number of characters required to represent this integer value will be returned.</param>
    /// <param name="startIndex">The index in the character array where insertion of characters should begin.</param>
    /// <param name="radix">Optional: parameter for the base to use, from 2 to 16. Defaults to base 10.</param>
    /// <returns>The numbero of characters copied into the buffer, or if the buffer was insufficient, a negative value that when negated equals the total number of characters required.</returns>
    public static int ToCharArray(this int value, char[] charArray, int startIndex = 0, int radix = 10)
    {
        var chars = new char[] {'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F'};
        var buff = new char[11]; // Enough to hold the max possible length (without commas) of a 32-bit integer, plus sign

        var i = buff.Length;
        bool isNegative = false;
        if (value <= 0) // handles 0 and int.MinValue special cases
        {
            isNegative = (value < 0);
            buff[--i] = chars[-(value % radix)];
            value = -(value / radix);
        }

        while (value != 0)
        {
            buff[--i] = chars[value % radix];
            value /= radix;
        }

        if (isNegative)
            buff[--i] = '-';

        // Copy to our destination buffer, if we can:
        int charCount = buff.Length - i;

        if (charArray == null)
            return -charCount;

        if (charArray.Length <= startIndex + charCount)
            return -charCount;

        for (int index=0; index < charCount; ++index)
        {
            charArray[startIndex + index] = buff[i + index];
        }

        return charCount;
    }

    public static string Replace(this string s, char[] separators, string newVal)
    {
        string[] temp;

        temp = s.Split(separators, StringSplitOptions.RemoveEmptyEntries);
        return String.Join(newVal, temp);
    }

    // Unity extensions:
    public static int GetListenerNumber(this UnityEventBase unityEvent)
    {
        var field = typeof(UnityEventBase).GetField("m_Calls", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.DeclaredOnly);
        var invokeCallList = field.GetValue(unityEvent);
        var property = invokeCallList.GetType().GetProperty("Count");
        return (int)property.GetValue(invokeCallList);
    }

    #endregion

    public static float InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }

    public static double InverseLerp(double xx, double yy, double value)
    {
        return (value - xx) / (yy - xx);
    }

    public static Color ParseColor(string aCol)
    {
        var strings = aCol.Split(new char[] { 'R','G','B','A','(',',',')' });
        Color output = Color.black;
        int writeIndex = 0;

        for (var i = 0; i < strings.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(strings[i]))
                continue;

            float v;
            if (float.TryParse(strings[i], out v))
                output[writeIndex++] = v;
        }
        return output;
    }

    /// <summary>
    /// Takes a linear 0-1 value and converts it to a -80 to 0 value according to a logarithmic scale.
    /// </summary>
    /// <param name="linearVolume">A linear normalized value for the volume.</param>
    /// <returns>A value from -80 to 0 on a logarithmic scale that maps to the 0-1 range provided.</returns>
    public static float LinearToDBAudioVolume(float linearVolume)
    {
        linearVolume = Mathf.Clamp(linearVolume, 0.0001f, 1f);
        return Mathf.Log10(linearVolume) * 20;
    }

    /// <summary>
    /// Takes a -80 to 0 value decible value and converts it to a linear 0-1 value according to a logarithmic scale.
    /// </summary>
    /// <param name="decibles">A -80 to 0 decibles value.</param>
    /// <returns>A linear 0-1 value corresponding to the decible value supplied according to a logarithmic scale.</returns>
    public static float DBToLinearAudioVolume(float decibles)
    {
        decibles = Mathf.Clamp(decibles, -80f, 0f);
        return Mathf.Pow(10, decibles / 20);
    }

    public static float DistanceOfPointToSegment2D(Vector2 pt, Vector2 p1, Vector3 p2, out Vector2 closest)
    {
        float dx = p2.x - p1.x;
        float dy = p2.y - p1.y;
        if ((dx == 0) && (dy == 0))
        {
            // It's a point not a line segment.
            closest = p1;
            dx = pt.x - p1.x;
            dy = pt.y - p1.y;
            return Mathf.Sqrt(dx * dx + dy * dy);
        }

        // Calculate the t that minimizes the distance.
        float t = ((pt.x - p1.x) * dx + (pt.y - p1.y) * dy) /
            (dx * dx + dy * dy);

        // See if this represents one of the segment's
        // end points or a point in the middle.
        if (t < 0)
        {
            closest = new Vector2(p1.x, p1.y);
            dx = pt.x - p1.x;
            dy = pt.y - p1.y;
        }
        else if (t > 1)
        {
            closest = new Vector2(p2.x, p2.y);
            dx = pt.x - p2.x;
            dy = pt.y - p2.y;
        }
        else
        {
            closest = new Vector2(p1.x + t * dx, p1.y + t * dy);
            dx = pt.x - closest.x;
            dy = pt.y - closest.y;
        }

        return Mathf.Sqrt(dx * dx + dy * dy);
    }

    /// <summary>
    /// Locates a value in a sorted list.
    /// </summary>
    /// <typeparam name="T1">Type of the list</typeparam>
    /// <typeparam name="T2">Type of the key used by the comparer to identify items in the list (should be the same as was used to sort the list).</typeparam>
    /// <param name="list">The sorted list to be searched.</param>
    /// <param name="key">The key used by the comparer to identify items in the list (should be the same as was used to sort the list).</param>
    /// <returns>The index of the item, if found. Otherwise the compliment of the index where the item should be inserted to maintain sorting is returned.</returns>
    public static int BinarySearch<T1,T2>(IList<T1> list, T2 key) where T1: IComparable<T2>
    {
        int min = 0, max = list.Count - 1;
        int mid = (min + max) / 2;

        while (min <= max)
        {
            mid = (min + max) / 2;
            T1 midValue = list[mid];

            int res = midValue.CompareTo(key);

            if (res == 0)
                return mid;
            else if (res < 0)
                min = mid + 1;
            else
                max = mid - 1;
        }
        return ~mid;
    }

    public delegate int AsymmetricalCompare<T1, T2>(T1 a, T2 b);

    /// <summary>
    /// Locates a value in a sorted list.
    /// </summary>
    /// <typeparam name="T1">Type of the list</typeparam>
    /// <typeparam name="T2">Type of the key used by the comparer to identify items in the list (should be the same as was used to sort the list).</typeparam>
    /// <param name="list">The sorted list to be searched.</param>
    /// <param name="key">The key used by the comparer to identify items in the list (should be the same as was used to sort the list).</param>
    /// <param name="comparer">The method to use for comparing items in the lsit.</param>
    /// <returns>The index of the item, if found. Otherwise the compliment of the index where the item should be inserted to maintain sorting is returned.</returns>
    public static int BinarySearch<T1, T2>(IList<T1> list, T2 key, AsymmetricalCompare<T1,T2> comparer)
    {
        int min = 0, max = list.Count - 1;
        int mid = (min + max) / 2;

        while (min <= max)
        {
            mid = (min + max) / 2;
            T1 midValue = list[mid];

            int res = comparer(midValue, key);

            if (res == 0)
                return mid;
            else if (res < 0)
                min = mid + 1;
            else
                max = mid - 1;
        }
        return ~mid;
    }

    /// <summary>
    /// Inserts a value into a sorted list, but only if the key sought is not already found in the list.
    /// </summary>
    /// <typeparam name="T1">Type of the list</typeparam>
    /// <typeparam name="T2">Type of the key used by the comparer to identify items in the list (should be the same as was used to sort the list).</typeparam>
    /// <param name="list">The list into which an item will be inserted if unique.</param>
    /// <param name="value">The value to be insersted into the list, if unique.</param>
    /// <param name="key">The key used by the comparer to identify items in the list (should be the same as was used to sort the list).</param>
    /// <returns>The index of the item if it was inserted. Otherwise if an existing item was found in the list, the compliment (which will be less than 0) of the index of the existing item is returned.</returns>
    public static int InsertSortedUnique<T1, T2>(IList<T1> list, T1 value, T2 key) where T1: IComparable<T2>
    {
        if (list.Count == 0)
        {
            list.Add(value);
            return 0;
        }
        if (list[list.Count - 1].CompareTo(key) < 0)
        {
            list.Add(value);
            return list.Count - 1;
        }
        if (list[0].CompareTo(key) > 0)
        {
            list.Insert(0, value);
            return 0;
        }

        int index = BinarySearch(list, key);

        if (index < 0)
        {
            index = ~index;
            list.Insert(index, value);
        }

        return index;
    }

    /// <summary>
    /// Inserts a value into a sorted list, but only if the key sought is not already found in the list.
    /// </summary>
    /// <typeparam name="T">Type of the list</typeparam>
    /// <param name="list">The list into which an item will be inserted if unique.</param>
    /// <param name="value">The value to be insersted into the list, if unique.</param>
    /// <returns>The index of the item if it was inserted. Otherwise if an existing item was found in the list, the compliment (which will be less than 0) of the index of the existing item is returned.</returns>
    public static int InsertSortedUnique<T>(this List<T> list, T value) where T: IComparable<T>
    {
        if (list.Count == 0)
        {
            list.Add(value);
            return 0;
        }
        if (list[list.Count - 1].CompareTo(value) < 0)
        {
            list.Add(value);
            return list.Count-1;
        }
        if (list[0].CompareTo(value) > 0)
        {
            list.Insert(0, value);
            return 0;
        }
        
        int index = list.BinarySearch(value);

        if (index < 0)
        {
            index = ~index;
            list.Insert(index, value);
        }

        return index;
    }

    /// <summary>
    /// Inserts a value into a sorted list, but only if the key sought is not already found in the list.
    /// </summary>
    /// <typeparam name="T1">Type of the list</typeparam>
    /// <typeparam name="T2">Type of the key used by the comparer to identify items in the list (should be the same as was used to sort the list).</typeparam>
    /// <param name="list">The list into which an item will be inserted if unique.</param>
    /// <param name="value">The value to be insersted into the list, if unique.</param>
    /// <param name="key">The key used by the comparer to identify items in the list (should be the same as was used to sort the list).</param>
    /// <param name="comparer">The method to use for comparing items in the lsit.</param>
    /// <returns>The index of the item if it was inserted. Otherwise if an existing item was found in the list, the compliment (which will be less than 0) of the index of the existing item is returned.</returns>
    public static int InsertSortedUnique<T1, T2>(IList<T1> list, T1 value, T2 key, AsymmetricalCompare<T1, T2> comparer)
    {
        int index = BinarySearch<T1, T2>(list, key, comparer);

        // See if the key already exists:
        if (index < 0)
        {
            // Key does not exist, so add the value and return the index of where it was inserted into the list:
            index = ~index;
            list.Insert(index, value);
            return index;
        }

        // The key already existed, so return the compliment of its index instead of the index to show that it wasn't added:
        return ~index;
    }

    /// <summary>
    /// Inserts a value into a sorted list, maintaining sorting.\
    /// </summary>
    /// <typeparam name="T1">Type of the list</typeparam>
    /// <typeparam name="T2">Type of the key used by the comparer to identify items in the list (should be the same as was used to sort the list).</typeparam>
    /// <param name="list">The list into which an item will be inserted.</param>
    /// <param name="value">The value to be insersted into the list.</param>
    /// <param name="key">The key used by the comparer to identify items in the list (should be the same as was used to sort the list).</param>
    /// <returns>The index of where the item was inserted.</returns>
    public static int InsertSorted<T1, T2>(IList<T1> list, T1 value, T2 key) where T1: IComparable<T2>
    {
        if (list.Count == 0)
        {
            list.Add(value);
            return 0;
        }
        if (list[list.Count - 1].CompareTo(key) <= 0)
        {
            list.Add(value);
            return list.Count - 1;
        }
        if (list[0].CompareTo(key) >= 0)
        {
            list.Insert(0, value);
            return 0;
        }

        int index = BinarySearch(list, key);

        if (index < 0)
            index = ~index;

        list.Insert(index, value);

        return index;
    }

    /// <summary>
    /// Inserts a value into a sorted list, maintaining sorting.\
    /// </summary>
    /// <typeparam name="T1">Type of the list</typeparam>
    /// <typeparam name="T2">Type of the key used by the comparer to identify items in the list (should be the same as was used to sort the list).</typeparam>
    /// <param name="list">The list into which an item will be inserted.</param>
    /// <param name="value">The value to be insersted into the list.</param>
    /// <param name="key">The key used by the comparer to identify items in the list (should be the same as was used to sort the list).</param>
    /// <param name="comparer">The method to use for comparing items in the lsit.</param>
    /// <returns>The index of where the item was inserted.</returns>
    public static int InsertSorted<T1, T2>(IList<T1> list, T1 value, T2 key, AsymmetricalCompare<T1, T2> comparer)
    {
        if (list.Count == 0)
        {
            list.Add(value);
            return 0;
        }
        if (comparer(list[list.Count - 1], key) <= 0)
        {
            list.Add(value);
            return list.Count - 1;
        }
        if (comparer(list[0], key) >= 0)
        {
            list.Insert(0, value);
            return 0;
        }

        int index = BinarySearch(list, key, comparer);

        if (index < 0)
            index = ~index;

        list.Insert(index, value);

        return index;
    }

    /// Calculates the optimal side of squares to be fit into a rectangle
    /// Inputs: x, y: width and height of the available area.
    ///         n: number of squares to fit
    /// Returns: the optimal side of the fitted squares
    /// (Code reworked from: https://math.stackexchange.com/questions/466198/algorithm-to-get-the-maximum-size-of-n-squares-that-fit-into-a-rectangle-with-a
    public static double FitSquares(double x, double y, int n)
    {
        double sx, sy;

        var px = Math.Ceiling(Math.Sqrt(n * x / y));
        if (Math.Floor(px * y / x) * px < n)
        {
            sx = y / Math.Ceiling(px * y / x);
        }
        else
        {
            sx = x / px;
        }

        var py = Math.Ceiling(Math.Sqrt(n * y / x));
        if (Math.Floor(py * x / y) * py < n)
        {
            sy = x / Math.Ceiling(x * py / y);
        }
        else
        {
            sy = y / py;
        }

        return Math.Max(sx, sy);
    }

    public static float InverseLerp(float xx, float yy, float value)
    {
        return (value - xx) / (yy - xx);
    }

    public static void EnableComponent(Component c, bool enable)
    {
        if (c is MonoBehaviour)
        {
            ((MonoBehaviour)c).enabled = enable;
            return;
        }

        var type = c.GetType();
        var propInfo = type.GetProperty("enabled");

        if (propInfo == null)
        {
            Debug.LogError("Unable to disable/enable component of type " + type.Name + " on GameObject " + c.gameObject.name + " because it doesn't have an \"enabled\" property.", c.gameObject);
            return;
        }

        propInfo.SetValue(c, enable);
    }


    /// <summary>
    /// Reads text from the specified file and returns it as a string.
    /// </summary>
    /// <param name="path">The path of the file to be read. If path is not rooted, Application.persistentDataPath is assumed.</param>
    /// <param name="dontRootPath">If true, the path is assumed to be full and correct and will not be rooted.</param>
    /// <returns></returns>
    public static string ReadTextFile(ref string path, bool dontRootPath = false)
    {
        if (!dontRootPath)
        {
            if (!Path.IsPathRooted(path))
            {
                path = Path.GetFullPath(Path.Combine(Application.persistentDataPath, path));
            }
        }

        if (!File.Exists(path))
            return null;

        using (var reader = new StreamReader(path))
        {
            if (reader == null)
            {
                Debug.LogError("Unable to open file for reading: " + path);
                return null;
            }

            string text = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();

            return text;
        }
    }

    /// <summary>
    /// Write text out to a file at the specified path. Will create the necessary folders if none already exist.
    /// If path is not rooted, Application.persistentDataPath is assumed to be the root.
    /// Final output path is stored back in the "path" argument.
    /// </summary>
    /// <param name="text">Text to be written to disk.</param>
    /// <param name="path">Path, including filename, to be written. This will also receive the full path used if the supplied path was not rooted.</param>
    /// <param name="dontRootPath">If set to true, the path is assumed to be correct and full and will not attempt to be rooted.</param>
    /// <returns>True if write succeeded, false otherwise.</returns>
    public static bool WriteTextFile(string text, ref string path, bool dontRootPath = false)
    {
        if (!dontRootPath)
        {
            if (!Path.IsPathRooted(path))
            {
                path = Path.GetFullPath(Path.Combine(Application.persistentDataPath, path));
                Debug.Log("Supplied output path was not rooted, assuming Application.persistentDataPath. (" + path + ")");
            }
        }

        var dir = Path.GetDirectoryName(path);

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        using (var writer = new StreamWriter(path, false))
        {
            if (writer == null)
            {
                Debug.LogError("Unable to open file for writing: " + path);
                return false;
            }

            writer.Write(text);
            writer.Close();
            writer.Dispose();
        }

        return true;
    }

    /// <summary>
    /// Deletes a file in the user's persistent data path.
    /// </summary>
    /// <param name="path">Path to the file (full rooted path will be generated if only partially supplied)</param>
    /// <returns>True if succeeded, false otherwise</returns>
    public static bool DeleteUserFile(ref string path)
    {
        if (!Path.IsPathRooted(path))
        {
            path = Path.GetFullPath(Path.Combine(Application.persistentDataPath, path));
            Debug.Log("Supplied output path was not rooted, assuming Application.persistentDataPath. (" + path + ")");
        }

        var dir = Path.GetDirectoryName(path);

        if (!Directory.Exists(dir))
        {
            Debug.Log("Specified path \"" + path + "\" does not exist.");
            return false;
        }

        File.Delete(path);

        return true;
    }

    /// <summary>
    /// Gets a list of all file in the specified user folder that matches the supplied extension.
    /// </summary>
    /// <param name="path">Subfolder, if any, off the user's data path.</param>
    /// <param name="ext">The extention all files should have (the extension WITHOUT the leading '.')</param>
    /// <returns>Returns a list of matching files, null otherwise.</returns>
    public static string[] GetUserFilesWithExtension(ref string path, string ext)
    {
        if (!Path.IsPathRooted(path))
        {
            path = Path.GetFullPath(Path.Combine(Application.persistentDataPath, path));
            Debug.Log("Supplied output path was not rooted, assuming Application.persistentDataPath. (" + path + ")");
        }

        var dir = Path.GetDirectoryName(path);

        if (!Directory.Exists(dir))
        {
            Debug.Log("Specified path \"" + path + "\" does not exist.");
            return null;
        }

        return System.IO.Directory.GetFiles(dir, "*." + ext, SearchOption.AllDirectories);
    }

    /// <summary>
    /// Creates a subfolder under the user folder (Applicaiton.persistentDataPath) by the specified name.
    /// </summary>
    /// <param name="path">Path of the subfolder (should be relative to user folder)</param>
    /// <returns>True if a new folder was created, false if it wasn't, or if it already existed.</returns>
    public static bool CreateUserFolder(string path)
    {
        path = GetRootedUserDataPath(path);

        if (Directory.Exists(path))
        {
            return false;
        }
        else
        {
            if (null == Directory.CreateDirectory(path))
                return false;
        }

        return true;
    }

    public static string GetRootedUserDataPath(string path)
    {
        if (!Path.IsPathRooted(path))
        {
            path = Path.GetFullPath(Path.Combine(Application.persistentDataPath, path));
        }

        return path;
    }

    public interface ITypedSerializationSurrogate : ISerializationSurrogate
    {
        System.Type SerializedType { get; }
    }

    public sealed class Vector3SerializationSurrogate : ITypedSerializationSurrogate
    {
        public System.Type SerializedType { get { return typeof(Vector3); } }

        // Method called to serialize a Vector3 object
        public void GetObjectData(System.Object obj,
                                  SerializationInfo info, StreamingContext context)
        {

            Vector3 v3 = (Vector3)obj;
            info.AddValue("x", v3.x);
            info.AddValue("y", v3.y);
            info.AddValue("z", v3.z);
            //Debug.Log(v3);
        }

        // Method called to deserialize a Vector3 object
        public System.Object SetObjectData(System.Object obj,
                                           SerializationInfo info, StreamingContext context,
                                           ISurrogateSelector selector)
        {

            Vector3 v3 = (Vector3)obj;
            v3.x = info.GetSingle("x");
            v3.y = info.GetSingle("y");
            v3.z = info.GetSingle("z");
            obj = v3;
            return obj;   // Formatters ignore this return value //Seems to have been fixed!
        }
    }

    /// <summary>
    /// Detects differences between the reference and delta objects and copies only those
    /// into the target object.
    /// </summary>
    /// <param name="reference">The reference object.</param>
    /// <param name="delta">The object in which changes are to be found from the reference.</param>
    /// <param name="target">The target object into which any discovered changes should be copied.</param>
    /// <returns>The modified target object. IF this is a class, the original will be modified. If a struct, the return value contains the modified struct.</returns>
    public static object CopyDifferences(object reference, object delta, object target)
    {
        var output = target;
        var fi = reference.GetType().GetFields();

        for (int i = 0; i < fi.Length; ++i)
        {
            var f = fi[i];

            if (!f.GetValue(reference).Equals(f.GetValue(delta)))
            {
                f.SetValue(output, f.GetValue(delta));
            }
        }

        var pi = reference.GetType().GetProperties();

        for (int i = 0; i < pi.Length; ++i)
        {
            var p = pi[i];

            if (!p.GetValue(reference).Equals(p.GetValue(delta)))
            {
                p.SetValue(output, p.GetValue(delta));
            }
        }

        return output;
    }

    /// <summary>
    /// Detects differences between the reference and delta objects and copies only those
    /// into the target object.
    /// </summary>
    /// <param name="reference">The reference object.</param>
    /// <param name="delta">The object in which changes are to be found from the reference.</param>
    /// <param name="targets">The target objects into which any discovered changes should be copied.</param>
    public static void CopyDifferences(object reference, object delta, object[] targets)
    {
        var fi = reference.GetType().GetFields();

        for (int i = 0; i < fi.Length; ++i)
        {
            var f = fi[i];

            if (!f.GetValue(reference).Equals(f.GetValue(delta)))
            {
                for (int j=0; j < targets.Length; ++j)
                    f.SetValue(targets[j], f.GetValue(delta));
            }
        }

        var pi = reference.GetType().GetProperties();

        for (int i = 0; i < pi.Length; ++i)
        {
            var p = pi[i];

            if (!p.GetValue(reference).Equals(p.GetValue(delta)))
            {
                for (int j = 0; j < targets.Length; ++j)
                    p.SetValue(targets[j], p.GetValue(delta));
            }
        }
    }

    public static void Log(object msg, UnityEngine.Object obj = null)
    {
#if DEBUG || SHOW_CONSOLE
        Debug.Log(msg, obj);
#endif
    }

    public static void LogWarning(object msg, UnityEngine.Object obj = null)
    {
#if DEBUG || SHOW_CONSOLE
        Debug.LogWarning(msg, obj);
#endif
    }

    public static void LogError(object msg, UnityEngine.Object obj = null)
    {
#if DEBUG || SHOW_CONSOLE
        Debug.LogError(msg, obj);
#endif
    }

    #region Unity Editor Routines
#if UNITY_EDITOR
    /// <summary>
    /// Extension method to copy all settings on a component.
    /// </summary>
    /// <typeparam name="T">The type of component to be copied</typeparam>
    /// <returns></returns>
    public static T Copy<T>(this Component comp, T other) where T : Component
    {
        Type type = comp.GetType();
        if (type != other.GetType()) return null; // type mis-match
        BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Default | BindingFlags.DeclaredOnly;

        var pinfos = from property in type.GetProperties(flags)
                     where !property.CustomAttributes.Any(attribute => attribute.AttributeType == typeof(ObsoleteAttribute))
                     select property;

        foreach (var pinfo in pinfos)
        {
            // Commented out becuase we don't need it now, but may need it later, in which case, uncomment it:
            //if (!pinfo.CanWrite)
            //    continue;
            
            if (pinfo.PropertyType.Equals(typeof(Material)) || pinfo.PropertyType.Equals(typeof(Material[])))
            {
                // Don't copy the material or the materials array of a renderer since that causes instancing we don't want at edit time. Only let sharedMaterial and sharedMaterials slip through:
                if (string.Compare(pinfo.Name.ToLower(), "material") == 0 || string.Compare(pinfo.Name.ToLower(), "materials") == 0)
                    continue;
            }

            if (pinfo.PropertyType.Equals(typeof(Mesh)))
            {
                // Don't copy the mesh of a MeshFilter since that causes instancing we don't want at edit time. Only let sharedMesh slip through:
                if (string.Compare(pinfo.Name.ToLower(), "mesh") == 0)
                    continue;
            }

            if (pinfo.PropertyType.Equals(typeof(Transform)))
            {
                // Don't set transform parents via reflection
                if (string.Compare(pinfo.Name.ToLower(), "parent") == 0 || string.Compare(pinfo.Name.ToLower(), "parentinternal") == 0)
                    continue;
            }

            if (pinfo.CanWrite)
            {
                try
                {
                    pinfo.SetValue(comp, pinfo.GetValue(other, null), null);
                }
                catch (System.Exception e)
                {
                    Debug.LogError(e.Message);
                } // In case of NotImplementedException being thrown. For some reason specifying that exception didn't seem to catch it, so I didn't catch anything specific.
            }
        }
        FieldInfo[] finfos = type.GetFields(flags);
        foreach (var finfo in finfos)
        {
            finfo.SetValue(comp, finfo.GetValue(other));
        }
        return comp as T;
    }

    // Extension method to allow adding a copy of a component in a single line.
    // Usage: var newComponent = go.AddComponent<MyComponent>(componentToCopy);
    public static T AddComponent<T>(this GameObject go, T toAdd) where T : Component
    {
        return go.AddComponent<T>().Copy(toAdd) as T;
    }
#endif
    #endregion

}
