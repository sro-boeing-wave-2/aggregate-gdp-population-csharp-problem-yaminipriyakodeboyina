using System;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace AggregateGDPPopulation
{
    public class Countrygdppopulation
    {
        public float GDP_2012;
        public float POPULATION_2012;
    }
    public class Fileoperation
    {
        async public Task<string> reader(string filepath)
        {
            StreamReader sr = new StreamReader(filepath);
            string line = await sr.ReadToEndAsync();
            return line;
        }
    
        async public Task writer(string filepath, string obj)
        {
            using (StreamWriter sr = new StreamWriter(filepath))
            {
                await sr.WriteAsync(obj);
            }
        }
    }
    public class DeserializeobjectToDictionary
    {
        public Dictionary<string,string> deserialize(string str)
        {
            var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(str);
            return values;
        }
    }
    public class Aggregate
    {
        public string aggregateGDPPopulation(string data,Dictionary<string,string> values)
        {
            Dictionary<string, Countrygdppopulation> result = new Dictionary<string, Countrygdppopulation>();
            string[] array = data.Split('\n');
            array[0] = array[0].Replace("\"", "");
            string[] headers = array[0].Split(',');
            int countryname = Array.IndexOf(headers, "Country Name");
            int gdpindex = Array.IndexOf(headers, "GDP Billions (USD) 2012");
            int popindex = Array.IndexOf(headers, "Population (Millions) 2012");
            foreach (string s in array)
            {
                string[] str = s.Replace("\"", "").Split(',');
                if (values.ContainsKey(str[countryname]) == true)
                {
                    if (result.ContainsKey(values[str[countryname]]) == false)
                    {
                        Countrygdppopulation cobj = new Countrygdppopulation();
                        result.Add(values[str[countryname]], cobj);
                        result[values[str[countryname]]].GDP_2012 = float.Parse(str[gdpindex]);
                        result[values[str[countryname]]].POPULATION_2012 = float.Parse(str[popindex]);

                    }
                    else
                    {
                        result[values[str[countryname]]].GDP_2012 += float.Parse(str[gdpindex]);
                        result[values[str[countryname]]].POPULATION_2012 += float.Parse(str[popindex]);
                    }
                }
            }
            string json = JsonConvert.SerializeObject(result, Formatting.Indented);
            return json;
        }

    }


    public class AggregateGDPPopulation
    {
        async public static Task calAggregate()
        {
            Fileoperation fileobj = new Fileoperation();
            var data =await fileobj.reader("../../../../AggregateGDPPopulation/data/datafile.csv");
            var mapper = await fileobj.reader("../../../../AggregateGDPPopulation/continent.json");
            DeserializeobjectToDictionary dobj = new DeserializeobjectToDictionary();
            var values = dobj.deserialize(mapper);
            Aggregate aggregateobj = new Aggregate();
            string str = aggregateobj.aggregateGDPPopulation(data, values);
            await fileobj.writer("../../../../AggregateGDPPopulation/output/output.json", str);
        }
    }







    
}