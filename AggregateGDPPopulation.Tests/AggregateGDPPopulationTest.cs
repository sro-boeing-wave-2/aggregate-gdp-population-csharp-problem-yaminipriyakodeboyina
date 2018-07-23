using System;
using Xunit;
using AggregateGDPPopulation;
using System.IO;
using Newtonsoft.Json.Linq;

namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        
        [Fact]
        public void Test1()
        {
            Class1.country();
            string s = File.ReadAllText(@"D:\c#codes\gdp\output\output.json");
            string str = File.ReadAllText(@"D:\c#codes\gdp\AggregateGDPPopulation.Tests\expected-output.json");
            Assert.Equal(s, str);
        }
    }
}
