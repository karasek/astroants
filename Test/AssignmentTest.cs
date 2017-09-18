using ants;
using Xunit;

namespace Test
{
    public class AssignmentTest
    {
        [Fact]
        public void JsonIsParsedCorrectly()
        {
            var json = @"{
""id"": ""2727"",
""startedTimestamp"": 1503929807498,
""map"": {
""areas"": [""5-R"", ""1-RDL"", ""10-DL"", ""2-RD"", ""1-UL"", ""1-UD"",
 ""2-RU"", ""1-RL"", ""2-UL""]
},
""astroants"": {""x"": 1, ""y"": 0 },
""sugar"": { ""x"": 2, ""y"": 1 }
}";
            var assignment = new AssignmentJsonLoader().Load(json);
            Assert.Equal("[1,0]", assignment.Astroants.ToString());
            Assert.Equal(9, assignment.Map.Areas.Length);
        }

        [Fact]
        public void CellIsParsedCorrectly()
        {
            var cell = Cell.ParseFromAssignment("2-UL");
            Assert.Equal(2, cell.Price);
            Assert.Equal("UL", cell.Directions);
        }
    }
}
