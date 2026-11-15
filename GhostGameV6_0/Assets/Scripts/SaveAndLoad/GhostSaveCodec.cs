using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

public sealed class GhostSaveData
{
    public int Mission;
    public float X;
    public float Y;
    public float Z;
    public string[] Keys;
    public string[] Notes;
}

/// <summary>
/// Versioned text save format. Rejects anything that is not this format.
/// </summary>
public static class GhostSaveCodec
{
    public const string Magic = "GHOSTSAVE1";
    public const int Version = 1;
    public const int MaxBytes = 64 * 1024;
    public const int MaxItems = 64;
    public const int MaxStringLength = 128;
    public const int MaxMission = 12;
    public const float MaxCoordinate = 1000000f;

    public static bool TryEncode(GhostSaveData data, out byte[] bytes)
    {
        bytes = null;
        if (data == null)
            return false;

        string[] keys = data.Keys ?? Array.Empty<string>();
        string[] notes = data.Notes ?? Array.Empty<string>();
        if (!IsMission(data.Mission) || !IsCoordinate(data.X) || !IsCoordinate(data.Y) || !IsCoordinate(data.Z))
            return false;
        if (!ValidItems(keys) || !ValidItems(notes))
            return false;

        var builder = new StringBuilder();
        builder.Append("{\"v\":").Append(Version.ToString(CultureInfo.InvariantCulture));
        builder.Append(",\"mission\":").Append(data.Mission.ToString(CultureInfo.InvariantCulture));
        builder.Append(",\"x\":").Append(data.X.ToString("R", CultureInfo.InvariantCulture));
        builder.Append(",\"y\":").Append(data.Y.ToString("R", CultureInfo.InvariantCulture));
        builder.Append(",\"z\":").Append(data.Z.ToString("R", CultureInfo.InvariantCulture));
        builder.Append(",\"keys\":");
        AppendArray(builder, keys);
        builder.Append(",\"notes\":");
        AppendArray(builder, notes);
        builder.Append('}');

        byte[] encoded = Encoding.UTF8.GetBytes(Magic + "\n" + builder.ToString());
        if (encoded.Length > MaxBytes)
            return false;

        bytes = encoded;
        return true;
    }

    public static bool TryDecode(byte[] bytes, out GhostSaveData data)
    {
        data = null;
        if (bytes == null || bytes.Length == 0 || bytes.Length > MaxBytes)
            return false;
        if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
            return false;

        string text = Encoding.UTF8.GetString(bytes);
        if (text.IndexOf('\0') >= 0)
            return false;

        string prefix = Magic + "\n";
        if (!text.StartsWith(prefix, StringComparison.Ordinal))
            return false;

        var parser = new Parser(text.Substring(prefix.Length));
        return parser.TryParse(out data);
    }

    static bool IsMission(int mission)
    {
        return mission >= 0 && mission <= MaxMission;
    }

    static bool IsCoordinate(float value)
    {
        return !float.IsNaN(value) && !float.IsInfinity(value) && Math.Abs(value) <= MaxCoordinate;
    }

    static bool ValidItems(string[] items)
    {
        if (items == null || items.Length > MaxItems)
            return false;

        for (int i = 0; i < items.Length; i++)
        {
            if (!IsSafeText(items[i]))
                return false;
        }

        return true;
    }

    static bool IsSafeText(string value)
    {
        if (value == null || value.Length > MaxStringLength)
            return false;

        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];
            if (c < 32 || c == 127 || c == '<' || c == '>')
                return false;
        }

        return true;
    }

    static void AppendArray(StringBuilder builder, string[] items)
    {
        builder.Append('[');
        for (int i = 0; i < items.Length; i++)
        {
            if (i > 0)
                builder.Append(',');
            AppendString(builder, items[i]);
        }
        builder.Append(']');
    }

    static void AppendString(StringBuilder builder, string value)
    {
        builder.Append('"');
        for (int i = 0; i < value.Length; i++)
        {
            char c = value[i];
            switch (c)
            {
                case '\\':
                    builder.Append("\\\\");
                    break;
                case '"':
                    builder.Append("\\\"");
                    break;
                default:
                    builder.Append(c);
                    break;
            }
        }
        builder.Append('"');
    }

    sealed class Parser
    {
        readonly string text;
        int index;

        public Parser(string text)
        {
            this.text = text;
        }

        public bool TryParse(out GhostSaveData data)
        {
            data = null;
            if (!Eat('{'))
                return false;

            bool sawVersion = false;
            bool sawMission = false;
            bool sawX = false;
            bool sawY = false;
            bool sawZ = false;
            bool sawKeys = false;
            bool sawNotes = false;
            int version = 0;
            int mission = 0;
            float x = 0f;
            float y = 0f;
            float z = 0f;
            string[] keys = null;
            string[] notes = null;

            SkipWs();
            if (Peek('}'))
                return false;

            while (true)
            {
                if (!TryString(out string key))
                    return false;
                if (!Eat(':'))
                    return false;

                switch (key)
                {
                    case "v":
                        if (sawVersion || !TryInt(out version))
                            return false;
                        sawVersion = true;
                        break;
                    case "mission":
                        if (sawMission || !TryInt(out mission))
                            return false;
                        sawMission = true;
                        break;
                    case "x":
                        if (sawX || !TryFloat(out x))
                            return false;
                        sawX = true;
                        break;
                    case "y":
                        if (sawY || !TryFloat(out y))
                            return false;
                        sawY = true;
                        break;
                    case "z":
                        if (sawZ || !TryFloat(out z))
                            return false;
                        sawZ = true;
                        break;
                    case "keys":
                        if (sawKeys || !TryStringArray(out keys))
                            return false;
                        sawKeys = true;
                        break;
                    case "notes":
                        if (sawNotes || !TryStringArray(out notes))
                            return false;
                        sawNotes = true;
                        break;
                    default:
                        return false;
                }

                SkipWs();
                if (Eat(','))
                {
                    SkipWs();
                    continue;
                }

                if (Eat('}'))
                    break;

                return false;
            }

            if (!End())
                return false;
            if (!sawVersion || !sawMission || !sawX || !sawY || !sawZ || !sawKeys || !sawNotes)
                return false;
            if (version != Version || !IsMission(mission) || !IsCoordinate(x) || !IsCoordinate(y) || !IsCoordinate(z))
                return false;
            if (!ValidItems(keys) || !ValidItems(notes))
                return false;

            data = new GhostSaveData
            {
                Mission = mission,
                X = x,
                Y = y,
                Z = z,
                Keys = keys,
                Notes = notes
            };
            return true;
        }

        bool End()
        {
            SkipWs();
            return index == text.Length;
        }

        void SkipWs()
        {
            while (index < text.Length && IsWs(text[index]))
                index++;
        }

        static bool IsWs(char c)
        {
            return c == ' ' || c == '\n' || c == '\r' || c == '\t';
        }

        bool Peek(char c)
        {
            SkipWs();
            return index < text.Length && text[index] == c;
        }

        bool Eat(char c)
        {
            SkipWs();
            if (index < text.Length && text[index] == c)
            {
                index++;
                return true;
            }

            return false;
        }

        bool TryInt(out int value)
        {
            value = 0;
            SkipWs();
            int start = index;
            if (index < text.Length && text[index] == '-')
                index++;

            int digitStart = index;
            while (index < text.Length && text[index] >= '0' && text[index] <= '9')
                index++;

            if (index == digitStart)
                return false;

            string token = text.Substring(start, index - start);
            if (token.Length > 1 && token[0] == '0')
                return false;
            if (token.Length > 2 && token[0] == '-' && token[1] == '0')
                return false;
            if (token == "-0")
                return false;

            return int.TryParse(token, NumberStyles.Integer, CultureInfo.InvariantCulture, out value);
        }

        bool TryFloat(out float value)
        {
            value = 0f;
            SkipWs();
            int start = index;
            if (index < text.Length && text[index] == '-')
                index++;

            bool sawDigit = false;
            while (index < text.Length && text[index] >= '0' && text[index] <= '9')
            {
                sawDigit = true;
                index++;
            }

            if (index < text.Length && text[index] == '.')
            {
                index++;
                while (index < text.Length && text[index] >= '0' && text[index] <= '9')
                {
                    sawDigit = true;
                    index++;
                }
            }

            if (index < text.Length && (text[index] == 'e' || text[index] == 'E'))
            {
                index++;
                if (index < text.Length && (text[index] == '+' || text[index] == '-'))
                    index++;

                int exponentDigits = 0;
                while (index < text.Length && text[index] >= '0' && text[index] <= '9')
                {
                    exponentDigits++;
                    index++;
                }

                if (exponentDigits == 0)
                    return false;
            }

            if (!sawDigit)
                return false;

            string token = text.Substring(start, index - start);
            if (!double.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out double parsed))
                return false;
            if (double.IsNaN(parsed) || double.IsInfinity(parsed) || parsed > float.MaxValue || parsed < -float.MaxValue)
                return false;

            value = (float)parsed;
            return true;
        }

        bool TryString(out string value)
        {
            value = null;
            SkipWs();
            if (index >= text.Length || text[index] != '"')
                return false;

            index++;
            var builder = new StringBuilder();
            while (index < text.Length)
            {
                char c = text[index++];
                if (c == '"')
                {
                    value = builder.ToString();
                    return true;
                }

                if (c == '\\')
                {
                    if (index >= text.Length)
                        return false;

                    char escaped = text[index++];
                    switch (escaped)
                    {
                        case '"':
                        case '\\':
                        case '/':
                            builder.Append(escaped);
                            break;
                        case 'b':
                            builder.Append('\b');
                            break;
                        case 'f':
                            builder.Append('\f');
                            break;
                        case 'n':
                            builder.Append('\n');
                            break;
                        case 'r':
                            builder.Append('\r');
                            break;
                        case 't':
                            builder.Append('\t');
                            break;
                        case 'u':
                            if (index + 4 > text.Length)
                                return false;
                            string hex = text.Substring(index, 4);
                            index += 4;
                            if (!int.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int code))
                                return false;
                            builder.Append((char)code);
                            break;
                        default:
                            return false;
                    }
                }
                else if (c < 32)
                {
                    return false;
                }
                else
                {
                    builder.Append(c);
                }

                if (builder.Length > MaxStringLength)
                    return false;
            }

            return false;
        }

        bool TryStringArray(out string[] values)
        {
            values = null;
            if (!Eat('['))
                return false;

            var items = new List<string>();
            SkipWs();
            if (Eat(']'))
            {
                values = Array.Empty<string>();
                return true;
            }

            while (true)
            {
                if (!TryString(out string item))
                    return false;
                if (items.Count >= MaxItems)
                    return false;

                items.Add(item);
                SkipWs();
                if (Eat(','))
                {
                    SkipWs();
                    continue;
                }

                if (Eat(']'))
                    break;

                return false;
            }

            values = items.ToArray();
            return true;
        }
    }
}
