using System;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace AggregateGDPPopulation
{
    public class countrygdppop
    {
        public float GDP_2012;
        public float POPULATION_2012;
    }
    public class Class1
    {
           async public static Task<string> reader(string filepath)
           {
                 StreamReader sr = new StreamReader(filepath);
                 string line = await sr.ReadToEndAsync();
                 return line;
           }
            async public static Task writer(string filepath,string obj)
            {
                using (StreamWriter sr = new StreamWriter(filepath))
                {
                    await sr.WriteAsync(obj);
                }  
            }
         async public static Task Calgdppopasync()
           {
            try
            {
                var data = reader("../../../../AggregateGDPPopulation/data/datafile.csv");
                var mapper = reader("../../../../AggregateGDPPopulation/continent.json");
                await Task.WhenAll(data, mapper);
                Dictionary<string, countrygdppop> result = new Dictionary<string, countrygdppop>();
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(mapper.Result);
                string[] array = data.Result.Split('\n');
                array[0] = array[0].Replace("\"", "");
                string[] headers = array[0].Split(',');
                int countryname = Array.IndexOf(headers, "Country Name");
                int gdpindex = Array.IndexOf(headers, "GDP Billions (USD) 2012");
                int popindex = Array.IndexOf(headers, "Population (Millions) 2012");
                foreach (string s in array)
                {
                    string[] str = s.Replace("\"","").Split(',');
                    if (values.ContainsKey(str[countryname]) == true)
                    {
                        if (result.ContainsKey(values[str[countryname]]) == false)
                        {
                            countrygdppop cobj = new countrygdppop();
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
               await writer("../../../../AggregateGDPPopulation/output/output.json", json);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
         }
    }
}