using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace SGContent
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(AppSettingsProvider.Instance.Configuration["AppSettings:ContentCompareDirectory"]);
        }
    }
}
