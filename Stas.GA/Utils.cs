using System;
using System.Collections;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using V2 = System.Numerics.Vector2;

namespace Stas.GA;

public enum PattNams : byte {
    PlayerInventory, GameStates, FileRoot, AreaChangeCounter,
    GameWindowScaleValues, TerrainRotatorHelper, TerrainRotationSelector
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Patt {
    public PattNams patt;
    public long ptr;

    public byte[] ToByte() {
        return BYTE.Concat(new byte[] { (byte)patt }, BitConverter.GetBytes(ptr));
    }
    public void Fill(BinaryReader br) {
        patt = (PattNams)br.ReadByte();
        ptr = br.ReadInt64();
    }
    public override string ToString() {
        return patt + "=" + ptr.ToString("X");
    }
}
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct Patt_wrapp {
    public IntPtr ptr;
    public int ba_size;
}
public abstract class iSave {
    public virtual string fname {
        get {
            var exe_dir = AppContext.BaseDirectory;
            //var exe_dir = Assembly.GetExecutingAssembly().Location;
            var dir = Path.GetDirectoryName(exe_dir);
            return Path.Combine(dir, tname + ".sett");
        }
    }
    public string tname { get { return GetType().Name; } }
    public string tname_full { get { return GetType().FullName; } }
    public DateTime created_date { get; }
    public iSave() { //_extension
        created_date = DateTime.Now;
    }
    public virtual T Load<T>() where T : iSave, new() {
        try {
            if (!File.Exists(fname)) {
                ui.AddToLog(tname + ".loading err: Not founf=[" + fname + "]", MessType.Error);
                FILE.SaveAsJson(this, fname);
                return new T();
            }
            else
                return FILE.LoadJson<T>(fname);
        }
        catch (Exception) {
            if (File.Exists(fname))
                File.Delete(fname);
            FILE.SaveAsJson(this, fname);
            return new T();
        }
    }
    public virtual void Save() {
        FILE.SaveAsJson(this, fname);
    }
    public override string ToString() {
        return tname;
    }
}
public abstract class iSett : iSave {
    public override T Load<T>() {
        Debug.Assert(!string.IsNullOrEmpty(fname) && fname.EndsWith(".sett"));
        return base.Load<T>();
    }
    public override void Save() {
        Debug.Assert(!string.IsNullOrEmpty(fname) && fname.EndsWith(".sett"));
        base.Save();
    }
}
public class MacrosKey {
    public string name { get; set; } = null!;
    public string key { get; set; } = null!;
    public override string ToString() {
        return name + "[" + key + "]";
    }
}
public class Macros {
    public string name { get; set; }
    public string code { get; set; }
    public bool sys { get; set; }
    public string def_key { get; set; }
    public override string ToString() {
        return name;
    }
}
public class MacrosNamedList : iSett {
    public BindingList<Macros> macroses { get; set; }
    public MacrosNamedList() {
        macroses = new BindingList<Macros>();
    }
}
internal sealed class V2Converter : JsonConverter<V2> {
    public override V2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
        var str = reader.GetString();
        var fa = str.Split(",");
        return new V2(float.Parse(fa[0]), float.Parse(fa[1]));
    }

    public override void Write(Utf8JsonWriter writer, V2 value, JsonSerializerOptions options) {
        writer.WriteStringValue(value.X + ", " + value.Y);
    }
}
public class MurmurHash2Simple {
    public static UInt32 Hash(Byte[] data) {
        return Hash(data, 0xc58f1a7b);
    }
    const UInt32 m = 0x5bd1e995;
    const Int32 r = 24;

    static UInt32 Hash(Byte[] data, UInt32 seed) {
        Int32 length = data.Length;
        if (length == 0)
            return 0;
        UInt32 h = seed ^ (UInt32)length;
        Int32 currentIndex = 0;
        while (length >= 4) {
            UInt32 k = BitConverter.ToUInt32(data, currentIndex);
            k *= m;
            k ^= k >> r;
            k *= m;

            h *= m;
            h ^= k;
            currentIndex += 4;
            length -= 4;
        }
        switch (length) {
            case 3:
                h ^= BitConverter.ToUInt16(data, currentIndex);
                h ^= (UInt32)data[currentIndex + 2] << 16;
                h *= m;
                break;
            case 2:
                h ^= BitConverter.ToUInt16(data, currentIndex);
                h *= m;
                break;
            case 1:
                h ^= data[currentIndex];
                h *= m;
                break;
            default:
                break;
        }

        // Do a few final mixes of the hash to ensure the last few
        // bytes are well-incorporated.

        h ^= h >> 13;
        h *= m;
        h ^= h >> 15;

        return h;
    }
}
public class Color4 {
    public float W { get; set; }
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}
public class FILE {
    internal sealed class V2Converter : JsonConverter<V2> {
        public override V2 Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            var str = reader.GetString();
            var fa = str.Split(",");
            return new V2(float.Parse(fa[0]), float.Parse(fa[1]));
        }

        public override void Write(Utf8JsonWriter writer, V2 value, JsonSerializerOptions options) {
            writer.WriteStringValue(value.X + ", " + value.Y);
        }
    }
    static JsonReaderOptions comment_opt = new JsonReaderOptions {
        CommentHandling = JsonCommentHandling.Allow
    };


    public static void SaveAsJson<T>(T t, string fname) {
        var opt = new JsonSerializerOptions {
            WriteIndented = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            IgnoreReadOnlyProperties = false,
            IncludeFields = false
        };
        opt.Converters.Add(new V2Converter());
        var str = JsonSerializer.Serialize<object>(t, opt);
        str.SafelyWriteToFile(fname);
    }

    public static T LoadJson<T>(string fname, Action if_err = null) {
        var str = File.ReadAllText(fname);
        if (str.Length == 0) {
            if_err?.Invoke();
            return default(T);
        }
        try {
            var opt = new JsonSerializerOptions {
                WriteIndented = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                IgnoreReadOnlyProperties = false,
                IncludeFields = false
            };
            opt.Converters.Add(new V2Converter());
            return JsonSerializer.Deserialize<T>(str, opt);
        }
        catch (Exception) {
            if_err?.Invoke();
            return default(T);
        }
    }
}
public class JSON {
    public class ValueTupleFactory : JsonConverterFactory {
        public override bool CanConvert(Type typeToConvert) {
            Type iTuple = typeToConvert.GetInterface("System.Runtime.CompilerServices.ITuple");
            return iTuple != null;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options) {
            Type[] genericArguments = typeToConvert.GetGenericArguments();

            Type converterType = genericArguments.Length switch {
                1 => typeof(ValueTupleConverter<>).MakeGenericType(genericArguments),
                2 => typeof(ValueTupleConverter<,>).MakeGenericType(genericArguments),
                3 => typeof(ValueTupleConverter<,,>).MakeGenericType(genericArguments),
                // And add other cases as needed
                _ => throw new NotSupportedException(),
            };
            return (JsonConverter)Activator.CreateInstance(converterType);
        }
    }

    public class ValueTupleConverter<T1> : JsonConverter<ValueTuple<T1>> {
        public override ValueTuple<T1> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            ValueTuple<T1> result = default;

            if (!reader.Read()) {
                throw new JsonException();
            }

            while (reader.TokenType != JsonTokenType.EndObject) {
                if (reader.ValueTextEquals("Item1") && reader.Read()) {
                    result.Item1 = JsonSerializer.Deserialize<T1>(ref reader, options);
                }
                else {
                    throw new JsonException();
                }
                reader.Read();
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, ValueTuple<T1> value, JsonSerializerOptions options) {
            writer.WriteStartObject();
            writer.WritePropertyName("Item1");
            JsonSerializer.Serialize<T1>(writer, value.Item1, options);
            writer.WriteEndObject();
        }
    }

    public class ValueTupleConverter<T1, T2> : JsonConverter<ValueTuple<T1, T2>> {
        public override (T1, T2) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            (T1, T2) result = default;

            if (!reader.Read()) {
                throw new JsonException();
            }

            while (reader.TokenType != JsonTokenType.EndObject) {
                if (reader.ValueTextEquals("Item1") && reader.Read()) {
                    result.Item1 = JsonSerializer.Deserialize<T1>(ref reader, options);
                }
                else if (reader.ValueTextEquals("Item2") && reader.Read()) {
                    result.Item2 = JsonSerializer.Deserialize<T2>(ref reader, options);
                }
                else {
                    throw new JsonException();
                }
                reader.Read();
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, (T1, T2) value, JsonSerializerOptions options) {
            writer.WriteStartObject();
            writer.WritePropertyName("Item1");
            JsonSerializer.Serialize<T1>(writer, value.Item1, options);
            writer.WritePropertyName("Item2");
            JsonSerializer.Serialize<T2>(writer, value.Item2, options);
            writer.WriteEndObject();
        }
    }

    public class ValueTupleConverter<T1, T2, T3> : JsonConverter<ValueTuple<T1, T2, T3>> {
        public override (T1, T2, T3) Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {
            (T1, T2, T3) result = default;

            if (!reader.Read()) {
                throw new JsonException();
            }

            while (reader.TokenType != JsonTokenType.EndObject) {
                if (reader.ValueTextEquals("Item1") && reader.Read()) {
                    result.Item1 = JsonSerializer.Deserialize<T1>(ref reader, options);
                }
                else if (reader.ValueTextEquals("Item2") && reader.Read()) {
                    result.Item2 = JsonSerializer.Deserialize<T2>(ref reader, options);
                }
                else if (reader.ValueTextEquals("Item3") && reader.Read()) {
                    result.Item3 = JsonSerializer.Deserialize<T3>(ref reader, options);
                }
                else {
                    ui.AddToLog("ValueTupleConverter err: debug me", MessType.Error);
                }
                reader.Read();
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, (T1, T2, T3) value, JsonSerializerOptions options) {
            writer.WriteStartObject();
            writer.WritePropertyName("Item1");
            JsonSerializer.Serialize<T1>(writer, value.Item1, options);
            writer.WritePropertyName("Item2");
            JsonSerializer.Serialize<T2>(writer, value.Item2, options);
            writer.WritePropertyName("Item3");
            JsonSerializer.Serialize<T3>(writer, value.Item3, options);
            writer.WriteEndObject();
        }
    }

    public static T FromUTF8Byte<T>(byte[] inp, int start = 0) {
        var str = Encoding.UTF8.GetString(inp, start, inp.Length - start);
        return JsonSerializer.Deserialize<T>(str);
    }

    public static byte[] ToUT8Byte<T>(T t) {
        var opt = new JsonSerializerOptions {
            WriteIndented = true,
            IgnoreReadOnlyProperties = true,
            IncludeFields = false
        };
        // DateTime .ToString("yyyy-MM-dd HH:mm:ss");
        return JsonSerializer.SerializeToUtf8Bytes<object>(t, opt);
    }
    public static T FromZipByte<T>(byte[] zba) {
        var opt = new JsonSerializerOptions {
            WriteIndented = true,
            IgnoreReadOnlyProperties = false,
            IncludeFields = false
        };
        if (zba == null) {
            ui.AddToLog("JSON.FromZipByte err: ba == null", MessType.Error);
            return default(T);
        }
        else {
            var ba = ZIP.UnZip(zba);
            try {
                return JsonSerializer.Deserialize<T>(ba, opt);
            }
            catch (Exception ex) {
                ui.AddToLog("JSON.FromZipByte err:" + ex.Message, MessType.Error);
                return default(T);
            }
        }
    }
    public static byte[] ToZipByte<T>(T t) {
        try {
            var ba = ToUT8Byte(t);
            var zip = ZIP.ToZip(ba);
            return zip;
        }
        catch (Exception ex) {
            ui.AddToLog("JSON.ToZipByte ERR:" + ex.Message, MessType.Error);
            return null;
        }
    }
}
public class ZIP {
    public static string UnZipToString(byte[] ba) {
        if (ba == null)
            return null;
        var sb = new StringBuilder();
        var raw = UnZip(ba);
        if (raw == null) {
            ui.AddToLog("UnZipToString error: UnZip(ba)==null", MessType.Error);
            return null;
        }
        using (var ms = new MemoryStream(raw)) {
            var buffer = new byte[1024 * 1024];
            int done;
            while ((done = ms.Read(buffer, 0, buffer.Length)) > 0) {
                var temp = new byte[done];
                Array.Copy(buffer, 0, temp, 0, done);
                sb.Append(Encoding.UTF8.GetString(temp));
            }
        }
        return sb.ToString();
    }
    public static byte[] UnZip(byte[] bytes) {
        using (var msi = new MemoryStream(bytes))
        using (var mso = new MemoryStream()) {
            try {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress)) {
                    CopyTo(gs, mso);
                    return mso.ToArray();
                }
            }
            catch (Exception ex) {
                if (ex.Message.Contains("The magic number in GZip header is not correct.")) {
                    return bytes;
                }
                else { ui.AddToLog("UnZip err: " + ex.Message, MessType.Error); }
                return default;
            }
        }
    }
    public static byte[] ToZip(string str) {
        var ba = Encoding.UTF8.GetBytes(str);
        return ToZip(ba);
    }
    public static byte[] ToZip(byte[] ba) {
        using (var msi = new MemoryStream(ba))
        using (var mso = new MemoryStream()) {
            using (var gs = new GZipStream(mso, CompressionMode.Compress)) {
                CopyTo(msi, gs);
            }
            return mso.ToArray();
        }
    }
    public static void CopyTo(Stream src, Stream dest) {
        try {
            byte[] bytes = new byte[4096];
            int cnt;
            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0) {
                dest.Write(bytes, 0, cnt);
            }
        }
        catch (Exception ex) {
            ui.AddToLog("ZIP.CopyTo [Err]: " + ex.Message, MessType.Error);
        }
    }
}
public class BYTE {
    public static byte[] Concat(byte[] a, byte[] b) {
        Debug.Assert(a != null && b != null, Environment.StackTrace);
        byte[] buffer1 = new byte[a.Length + b.Length];
        Buffer.BlockCopy(a, 0, buffer1, 0, a.Length);
        Buffer.BlockCopy(b, 0, buffer1, a.Length, b.Length);
        return buffer1;
    }
    public static byte[] Concat(byte[] a, byte[] b, byte[] c) {
        return Concat(Concat(a, b), c);
    }
    public static string ToHexString(byte[] ba) {
        var sb = new StringBuilder(ba.Length * 2);
        foreach (byte b in ba) {
            // can be "x2" if you want lowercase & X2 for upper
            sb.Append(b.ToString("x2"));
        }
        return sb.ToString();
    }
    public static IEnumerable Combine(byte[] first, byte[] second) {
        foreach (byte b in first)
            yield return b;

        foreach (byte b in second)
            yield return b;
    }
    private static byte[] Combine(byte[][] arrays) {
        byte[] bytes = new byte[arrays.Sum(a => a.Length)];
        int offset = 0;

        foreach (byte[] array in arrays) {
            Buffer.BlockCopy(array, 0, bytes, offset, array.Length);
            offset += array.Length;
        }

        return bytes;
    }

}
