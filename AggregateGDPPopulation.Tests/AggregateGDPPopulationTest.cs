using System;
using Xunit;
using AggregateGDPPopulation;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;
namespace AggregateGDPPopulation.Tests
{
    public class UnitTest1
    {
        
        [Fact]
       async public void Test1()
        {
           await AggregateGDPPopulation.calAggregate();
            Fileoperation fileobj1 = new Fileoperation();
            var actualoutput = fileobj1.reader("../../../../AggregateGDPPopulation/output/output.json");
            var expectedoutput = fileobj1.reader("../../../expected-output.json");
            string str1 = await actualoutput;
            string str2 = await expectedoutput;
            Assert.Equal(str1,str2);
        }
    }
}
