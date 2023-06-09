using MemoryPack;

namespace Common.Extensions;

public static class BytesExtensions
{
    public static byte[] ToBytes<TObject>(this TObject obj)
    {
        return MemoryPackSerializer.Serialize(obj);
    }

    public static TObject? ToObject<TObject>(this byte[] bytes)
    {
        return MemoryPackSerializer.Deserialize<TObject>(bytes);
    }
}