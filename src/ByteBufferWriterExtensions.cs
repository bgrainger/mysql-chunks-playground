using System;
using System.Text;

namespace src
{
	public static class ByteBufferWriterExtensions
	{

#if !NET45 && !NET461 && !NET471 && !NETSTANDARD1_3 && !NETSTANDARD2_0 && !NETSTANDARD2_1 && !NETCOREAPP2_1
		static SlicedEncoder? m_slicedEncoder;
#endif

		public static void WriteLengthEncodedInteger(this ByteBufferWriter writer, ulong value)
		{
			switch (value)
			{
				case < 251:
					writer.Write((byte)value);
					break;

				case < 65536:
					writer.Write((byte)0xfc);
					writer.Write((ushort)value);
					break;

				case < 16777216:
					writer.Write((uint)((value << 8) | 0xfd));
					break;

				default:
					writer.Write((byte)0xfe);
					writer.Write(value);
					break;
			}
		}

#if NET45 || NETSTANDARD1_3
		public static void WriteLengthEncodedString(this ByteBufferWriter writer, string value)
		{
			var byteCount = Encoding.UTF8.GetByteCount(value);
			writer.WriteLengthEncodedInteger((ulong) byteCount);
			writer.Write(value);
		}
#else
		public static void WriteLengthEncodedString(this ByteBufferWriter writer, string value) => writer.WriteLengthEncodedString(value.AsSpan());

		public static void WriteLengthEncodedString(this ByteBufferWriter writer, ReadOnlySpan<char> value)
		{
			var byteCount = Encoding.UTF8.GetByteCount(value);
			writer.WriteLengthEncodedInteger((ulong)byteCount);
			writer.Write(value);
		}
#endif

        public static void WriteLengthEncodedStringOld(this ByteBufferWriter writer, StringBuilder builder)
        {
            writer.WriteLengthEncodedString(builder.ToString());
		}

		public static void WriteLengthEncodedStringNew(this ByteBufferWriter writer, StringBuilder builder)
		{
#if !NET45 && !NET461 && !NET471 && !NETSTANDARD1_3 && !NETSTANDARD2_0 && !NETSTANDARD2_1 && !NETCOREAPP2_1

			m_slicedEncoder ??= new SlicedEncoder();

            var totalLength = 0;

            foreach (var chunk in builder.GetChunks())
            {
                totalLength += m_slicedEncoder.GetUtf8ByteCount(chunk.Span);
            }

			writer.WriteLengthEncodedInteger((ulong)totalLength);
            
            string? previousUtf16 = null;
			ReadOnlySpan<char> utf16Span = null;
			foreach (var chunk in builder.GetChunks())
			{
				m_slicedEncoder.PrepareUtf16Span(chunk, out previousUtf16, out utf16Span);

				if (previousUtf16 != null)
				{
					writer.Write(previousUtf16);
				}

				writer.Write(utf16Span);
			}
#else
			writer.WriteLengthEncodedString(builder.ToString());
#endif
		}

		public static void WriteNullTerminatedString(this ByteBufferWriter writer, string value)
		{
			writer.Write(value);
			writer.Write((byte)0);
		}
	}
}
