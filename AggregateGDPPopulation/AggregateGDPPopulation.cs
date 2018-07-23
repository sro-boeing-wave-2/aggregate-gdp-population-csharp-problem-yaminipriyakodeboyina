using System;
using System.IO;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AggregateGDPPopulation
{
    public class countrygdppop
    {
        public float GDP_2012 = 0;
        public float POPULATION_2012=0;
    }
    public class Class1
    {
        public static void country()
        {
            Dictionary<string, countrygdppop> result = new Dictionary<string, countrygdppop>();
            try
            {
                StreamReader sr = new StreamReader("../../../../AggregateGDPPopulation/data/datafile.csv") ;
                string line = sr.ReadToEnd();
                sr.Close();
                string[] array = line.Split('\n');
                array[0] = array[0].Replace("\"","");
                string[] headers = array[0].Split(','); 
                int countryname= Array.IndexOf(headers, "Country Name");
                int gdpindex = Array.IndexOf(headers, "GDP Billions (USD) 2012");
                int popindex = Array.IndexOf(headers, "Population (Millions) 2012");
                var values = JsonConvert.DeserializeObject<Dictionary<string, string>>(File.ReadAllText("../../../../AggregateGDPPopulation/continent.json"));
                foreach (string s in array)
                {
                    string[] str = s.Replace("\"", "").Split(',');
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
                File.WriteAllText("../../../../AggregateGDPPopulation/output/output.json", json);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

    }
}