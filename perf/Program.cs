using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Running;

namespace perf
{
    public class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<PerfTests>();
        }
    }
}
