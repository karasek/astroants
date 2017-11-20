using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using ants;
using Xunit;
using Xunit.Abstractions;

namespace Test
{
    public class SpeedTest
    {
        readonly ITestOutputHelper _output;

        public SpeedTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task MeasureNewtonsoft()
        {
            var loader = new AssignmentJsonLoader();
            await WriteSpeed(loader);
        }

        [Fact]
        public async Task MeasureUtf8Json()
        {
            var loader = new AssignmentJsonLoaderUtf8();
            await WriteSpeed(loader);
        }

        async Task WriteSpeed(IAssignmentJsonLoader loader)
        {
            var memory = new MemoryStream();
            using (var str = File.OpenRead(@"..\..\..\data\data.json"))
            {
                await str.CopyToAsync(memory);
            }
            var sw = new Stopwatch();
            
            for (var i = 0; i < 100; i++)
            {
                memory.Seek(0, SeekOrigin.Begin);
                sw.Start();
                var s = loader.Load(memory);
                sw.Stop();
                Assert.Equal("1866", s.Id);
            }
            _output.WriteLine($"Avg. parsed in {sw.ElapsedMilliseconds / 100} ms");
        }
    }
}