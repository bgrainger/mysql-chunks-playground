using System;
using System.Text;

namespace src
{
#if !NET45 && !NET461 && !NET471 && !NETSTANDARD1_3 && !NETSTANDARD2_0 && !NETSTANDARD2_1 && !NETCOREAPP2_1
	//TODO: better naming
	public class SlicedEncoder
	{
		//TODO: thread-safe
		private char? _countingSurrogate;
		private char? _preparationSurrogate;

		public int GetUtf8ByteCount(ReadOnlySpan<char> span)
		{
            var containsSurrogateAtBeginning = BitConverter.IsLittleEndian
                ? char.IsLowSurrogate(span[0])
                : char.IsHighSurrogate(span[0]);
            var containsSurrogateAtEnding = BitConverter.IsLittleEndian
                ? char.IsHighSurrogate(span[^1])
                : char.IsLowSurrogate(span[^1]);

            if (containsSurrogateAtBeginning || containsSurrogateAtEnding)
			{
				var startPosition = containsSurrogateAtBeginning ? 1 : 0;
				var length = containsSurrogateAtEnding ? span.Length - 1 : span.Length;

				var operationalSpan = span.Slice(startPosition, length - startPosition);

				var totalCount = Encoding.UTF8.GetByteCount(operationalSpan);

				if (containsSurrogateAtBeginning)
				{
					if (!_countingSurrogate.HasValue)
					{
						throw new Exception("Missed surrogate character");
					}

					totalCount += Encoding.UTF8.GetByteCount(new[] { _countingSurrogate.Value, span[0] });
				}

				if (containsSurrogateAtEnding)
				{
					_countingSurrogate = span[^1];
				}

				return totalCount;
			}
			else
			{
				return Encoding.UTF8.GetByteCount(span);
			}
		}

		public void PrepareUtf16Span(ReadOnlyMemory<char> mem, out string? previouslySplitted, out ReadOnlySpan<char> output)
		{
			var firstChar =mem.Slice(0, 1).Span[0];
            var lastChar = mem.Slice(mem.Length - 1, 1).Span[0];

			var containsSurrogateAtBeginning = BitConverter.IsLittleEndian
                ? char.IsLowSurrogate(firstChar)
                : char.IsHighSurrogate(firstChar);
            var containsSurrogateAtEnding = BitConverter.IsLittleEndian
                ? char.IsHighSurrogate(lastChar)
                : char.IsLowSurrogate(lastChar);

			previouslySplitted = null;

			if (containsSurrogateAtBeginning || containsSurrogateAtEnding)
			{
				var startPosition = containsSurrogateAtBeginning ? 1 : 0;
				var length = containsSurrogateAtEnding ? mem.Length - 1 : mem.Length;

				output = mem.Slice(startPosition, length - startPosition).Span;

				if (containsSurrogateAtBeginning)
				{
					if (!_preparationSurrogate.HasValue)
					{
						throw new Exception("Missed surrogate character");
					}

					previouslySplitted = new string(new[] { _preparationSurrogate.Value, firstChar });

				}

				if (containsSurrogateAtEnding)
				{
					_preparationSurrogate = lastChar;
				}
			}
			else
			{
				previouslySplitted = null;
				output = mem.Span;
			}
		}
	}
#endif
}
