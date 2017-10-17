using Newtonsoft.Json;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System;
using System.IO;

namespace SGContent
{
    class Program
    {
        static void Main(string[] args)
        {
            var settings = LoadAppSettings();
            Console.WriteLine("Hello World!");
        }

        static AppSettings LoadAppSettings()
        {
            string path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            string appSettingsJson = File.ReadAllText(path);
            AppSettings settings = JsonConvert.DeserializeObject<AppSettings>(appSettingsJson);
            return settings;
        }
    }
}
