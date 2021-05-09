using System;
using System.Collections.Generic;
using System.Text;
using BenchmarkDotNet.Attributes;
using src;

namespace perf
{
    [MemoryDiagnoser]
    [CsvMeasurementsExporter]
    public class PerfTests
    {
        public const int SmallSbSize = 100;
        public const int MediumSbSize = 500;
        public const int LargeSbSize = 1000;
        public const int ExLargeSbSize = 10000;

        public StringBuilder _sbShortSurrogates { get; set; }

        public StringBuilder _sbMediumSurrogates { get; set; }

        public StringBuilder _sbLongSurrogates { get; set; }

        public StringBuilder _sbExLongSurrogates { get; set; }

        public StringBuilder _sbShort { get; set; }

        public StringBuilder _sbMedium { get; set; }

        public StringBuilder _sbLong { get; set; }

        public StringBuilder _sbExLong { get; set; }


        [GlobalSetup]
        public void GlobalSetup()
        {
            _sbShort = new StringBuilder(new string('a', SmallSbSize));
            _sbShortSurrogates = new StringBuilder("a");
            for (int i = 0; i < SmallSbSize; i++)
                _sbShortSurrogates.Append("😀");


            _sbMedium = new StringBuilder(new string('a', MediumSbSize));
            _sbMediumSurrogates = new StringBuilder("a");
            for (int i = 0; i < MediumSbSize; i++)
                _sbMediumSurrogates.Append("😀");

            _sbLong = new StringBuilder(new string('a', LargeSbSize));
            _sbLongSurrogates = new StringBuilder("a");
            for (int i = 0; i < LargeSbSize; i++)
                _sbLongSurrogates.Append("😀");

            _sbExLong = new StringBuilder(new string('a', ExLargeSbSize));
            _sbExLongSurrogates = new StringBuilder("a");
            for (int i = 0; i < ExLargeSbSize; i++)
                _sbExLongSurrogates.Append("😀");
        }

        [Benchmark]
        public void OldBehaviorShort()
        {
            var writer1 = new ByteBufferWriter();
            writer1.WriteLengthEncodedStringOld(_sbShort);

            var payload = writer1.ToPayloadData().Memory.ToArray();
        }
        [Benchmark]
        public void NewBehaviorShort()
        {
            var writer2 = new ByteBufferWriter();
            writer2.WriteLengthEncodedStringNew(_sbShort);

            var payload = writer2.ToPayloadData().Memory.ToArray();
        }


        [Benchmark]
        public void OldBehaviorShortSurrogate()
        {
            var writer1 = new ByteBufferWriter();
            writer1.WriteLengthEncodedStringOld(_sbShortSurrogates);

            var payload = writer1.ToPayloadData().Memory.ToArray();
        }
        [Benchmark]
        public void NewBehaviorShortSurrogates()
        {
            var writer2 = new ByteBufferWriter();
            writer2.WriteLengthEncodedStringNew(_sbShortSurrogates);

            var payload = writer2.ToPayloadData().Memory.ToArray();
        }


        [Benchmark]
        public void OldBehaviorMedium()
        {
            var writer1 = new ByteBufferWriter();
            writer1.WriteLengthEncodedStringOld(_sbMedium);

            var payload = writer1.ToPayloadData().Memory.ToArray();
        }
        [Benchmark]
        public void NewBehaviorMedium()
        {
            var writer2 = new ByteBufferWriter();
            writer2.WriteLengthEncodedStringNew(_sbMedium);

            var payload = writer2.ToPayloadData().Memory.ToArray();
        }




        [Benchmark]
        public void OldBehaviorMediumSurrogates()
        {
            var writer1 = new ByteBufferWriter();
            writer1.WriteLengthEncodedStringOld(_sbMediumSurrogates);

            var payload = writer1.ToPayloadData().Memory.ToArray();
        }
        [Benchmark]
        public void NewBehaviorMediumSurrogates()
        {
            var writer2 = new ByteBufferWriter();
            writer2.WriteLengthEncodedStringNew(_sbMediumSurrogates);

            var payload = writer2.ToPayloadData().Memory.ToArray();
        }



        [Benchmark]
        public void OldBehaviorLong()
        {
            var writer1 = new ByteBufferWriter();
            writer1.WriteLengthEncodedStringOld(_sbLong);

            var payload = writer1.ToPayloadData().Memory.ToArray();
        }
        [Benchmark]
        public void NewBehaviorLong()
        {
            var writer2 = new ByteBufferWriter();
            writer2.WriteLengthEncodedStringNew(_sbLong);

            var payload = writer2.ToPayloadData().Memory.ToArray();
        }


        [Benchmark]
        public void OldBehaviorLongSurrogates()
        {
            var writer1 = new ByteBufferWriter();
            writer1.WriteLengthEncodedStringOld(_sbLongSurrogates);

            var payload = writer1.ToPayloadData().Memory.ToArray();
        }
        [Benchmark]
        public void NewBehaviorLongSurrogates()
        {
            var writer2 = new ByteBufferWriter();
            writer2.WriteLengthEncodedStringNew(_sbLongSurrogates);

            var payload = writer2.ToPayloadData().Memory.ToArray();
        }


        [Benchmark]
        public void OldBehaviorExLong()
        {
            var writer1 = new ByteBufferWriter();
            writer1.WriteLengthEncodedStringOld(_sbExLong);

            var payload = writer1.ToPayloadData().Memory.ToArray();
        }
        [Benchmark]
        public void NewBehaviorExLong()
        {
            var writer2 = new ByteBufferWriter();
            writer2.WriteLengthEncodedStringNew(_sbExLong);

            var payload = writer2.ToPayloadData().Memory.ToArray();
        }



        [Benchmark]
        public void OldBehaviorExLongSurrogates()
        {
            var writer1 = new ByteBufferWriter();
            writer1.WriteLengthEncodedStringOld(_sbExLongSurrogates);

            var payload = writer1.ToPayloadData().Memory.ToArray();
        }
        [Benchmark]
        public void NewBehaviorExLongSurrogates()
        {
            var writer2 = new ByteBufferWriter();
            writer2.WriteLengthEncodedStringNew(_sbExLongSurrogates);

            var payload = writer2.ToPayloadData().Memory.ToArray();
        }
    }
}
