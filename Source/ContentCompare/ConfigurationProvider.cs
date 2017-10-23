using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace SGContent
{
    public class ConfigurationProvider
    {
        public IConfigurationRoot Configuration { get; }
        public AppSettings AppSettings { get; }
        private readonly ILogger logger;

        public ConfigurationProvider(ILoggerFactory loggerFactory)
        {
            logger = loggerFactory.CreateLogger<ConfigurationProvider>();
            //TODO: Change this from scoreguide branch to another
            string appSettingsUrl = "https://raw.githubusercontent.com/SmarterApp/AP_ItemSampler/scoreguide/SmarterBalanced.SampleItems/src/SmarterBalanced.SampleItems.Web/appsettings.json";
            
            SaveDependency(appSettingsUrl, "appsettings.json");
            logger.LogInformation("Successfully downloaded dependencies");

            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.Override.json", optional: true);
            Configuration = builder.Build();
            AppSettings appSettings = new AppSettings();

            Configuration.Bind(appSettings);
            AppSettings = appSettings;
            logger.LogInformation("Successfully loaded configuration");
        }

        public void DownloadConfigFiles()
        {
            string itemsPatchUrl = "https://raw.githubusercontent.com/SmarterApp/AP_ItemSampler/master/SmarterBalanced.SampleItems/ClaimConfigurations/ItemsPatch.xml";
            SaveDependency(itemsPatchUrl, AppSettings.SbContent.PatchXMLPath);

            string accessibilityUrl = "https://raw.githubusercontent.com/osu-cass/AccessibilityAccommodationConfigurations/05bf8f52863bce54142c9f3bc36db02e475258f4/AccessibilityConfig.xml";
            SaveDependency(accessibilityUrl, AppSettings.SbContent.AccommodationsXMLPath);

            string claimsUrl = "https://raw.githubusercontent.com/SmarterApp/AP_ItemSampler/scoreguide/SmarterBalanced.SampleItems/ClaimConfigurations/ClaimsConfig.xml";
            SaveDependency(claimsUrl, AppSettings.SbContent.ClaimsXMLPath);

            string interactionsUrl = "https://raw.githubusercontent.com/SmarterApp/AP_ItemSampler/scoreguide/SmarterBalanced.SampleItems/InteractionTypeConfigurations/InteractionTypes.xml";
            SaveDependency(interactionsUrl, AppSettings.SbContent.InteractionTypesXMLPath);

            string standardsUrl = "https://raw.githubusercontent.com/SmarterApp/AP_ItemSampler/scoreguide/SmarterBalanced.SampleItems/CoreStandardsConfigurations/CoreStandards.xml";
            SaveDependency(standardsUrl, AppSettings.SbContent.CoreStandardsXMLPath);
        }

        static void SaveDependency(string url, string fileName)
        {
            using (var client = new HttpClient())
            {
                using (var result = client.GetAsync(url).Result)
                {
                    if (result.IsSuccessStatusCode)
                    {
                        string contents = result.Content.ReadAsStringAsync().Result;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                        File.WriteAllText(path, contents);
                    }
                }
            }
        }
    }
}
