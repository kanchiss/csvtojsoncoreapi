using CsvHelper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net.Http.Json;

namespace csvtojsoncoreapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private string jsonData= "";

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("csvtojson")]
        public IList<Dictionary<string, string>> GetCSVToJSON()
        {
            var csv = new List<string[]>();
            var file = System.IO.File.ReadAllLines(@"E:\KANCHI\PHOTO\kanchi.csv");
            foreach (string line in file)
                csv.Add(line.Split(','));
            var key = file[0].Split(',');

            var data = new List<Dictionary<string, string>>();

            for (int i = 1; i < file.Length; i++)
            {
                var objResult = new Dictionary<string, string>();
                for (int j = 0; j < key.Length; j++)
                    objResult.Add(key[j], csv[i][j]);

                data.Add(objResult);
            }

            jsonData = JsonConvert.SerializeObject(data);

            return data;

        }

        [HttpGet("csvtojsonstring")]
        public string GetCSVToJSONString()
        {
            var csv = new List<string[]>();
            var file = System.IO.File.ReadAllLines(@"E:\KANCHI\PHOTO\kanchi.csv");
            foreach (string line in file)
                csv.Add(line.Split(','));
            var key = file[0].Split(',');

            var data = new List<Dictionary<string, string>>();

            for (int i = 1; i < file.Length; i++)
            {
                var objResult = new Dictionary<string, string>();
                for (int j = 0; j < key.Length; j++)
                    objResult.Add(key[j], csv[i][j]);

                data.Add(objResult);
            }

            jsonData = JsonConvert.SerializeObject(data);

            return jsonData;

        }

        [HttpGet("jsontocsv")]
        public string GetJSONToCSV()
        {
            jsonData = "[{\"Emp\":\"200000\",\"Job\":\"2001010\",\"Phase\":\"1200000\",\"Day\":\"Mon\",\"Hours\":\"8\"},{\"Emp\":\"200000\",\"Job\":\"2001010\",\"Phase\":\"1200000\",\"Day\":\"Tue\",\"Hours\":\"10\"},{\"Emp\":\"200000\",\"Job\":\"2001010\",\"Phase\":\"1300000\",\"Day\":\"Wed\",\"Hours\":\"8\"},{\"Emp\":\"200000\",\"Job\":\"2001010\",\"Phase\":\"1200000\",\"Day\":\"Thu\",\"Hours\":\"8\"},{\"Emp\":\"200000\",\"Job\":\"2001010\",\"Phase\":\"1200000\",\"Day\":\"Fri\",\"Hours\":\"6\"},{\"Emp\":\"300000\",\"Job\":\"3001000\",\"Phase\":\"1200000\",\"Day\":\"Mon\",\"Hours\":\"8\"},{\"Emp\":\"300000\",\"Job\":\"3001000\",\"Phase\":\"1300000\",\"Day\":\"Tue\",\"Hours\":\"8\"},{\"Emp\":\"300000\",\"Job\":\"3001000\",\"Phase\":\"1200000\",\"Day\":\"Wed\",\"Hours\":\"8\"},{\"Emp\":\"300000\",\"Job\":\"3001000\",\"Phase\":\"1200000\",\"Day\":\"Thu\",\"Hours\":\"8\"},{\"Emp\":\"300000\",\"Job\":\"3001000\",\"Phase\":\"1300000\",\"Day\":\"Fri\",\"Hours\":\"8\"}]";

            var data = JsonConvert.DeserializeObject<ExpandoObject[]>(jsonData);

            using (TextWriter writer = new StreamWriter(@"E:\KANCHI\PHOTO\sample.csv"))
            {
                using (var csv = new CsvWriter(writer, System.Globalization.CultureInfo.CurrentCulture))
                {
                    csv.WriteRecords((data as IList<dynamic>));
                }

                return writer.ToString();

            }

        }
    }
}