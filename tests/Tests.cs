using System.Text;
using NUnit.Framework;
using src;

namespace tests
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test()
        {
            var sb = new StringBuilder("a");
            for (int i = 0; i < 50; i++)
                sb.Append("😀");


            var writer1 = new ByteBufferWriter();
            writer1.WriteLengthEncodedStringOld(sb);

            var realPayload = writer1.ToPayloadData().Memory.ToArray();


            var writer2 = new ByteBufferWriter();
            writer2.WriteLengthEncodedStringNew(sb);

            var payload = writer2.ToPayloadData();
            var data = payload.Memory.ToArray();

            Assert.AreEqual(realPayload, data);
        }

    }
}