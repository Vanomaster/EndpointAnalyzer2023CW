using Google.Protobuf;

namespace Common.Extensions;

public static class ByteStringExtensions
{
    public static ByteString ToByteString(this Guid id)
    {
        return UnsafeByteOperations.UnsafeWrap(id.ToByteArray());
    }

    public static Guid ToGuid(this ByteString byteString)
    {
        return new Guid(byteString.Memory.ToArray());
    }

    public static ByteString ToByteString(this byte[] bytes)
    {
        return UnsafeByteOperations.UnsafeWrap(bytes);
    }

    public static byte[] ToBytes(this ByteString byteString)
    {
        return byteString.Memory.ToArray();
    }
}