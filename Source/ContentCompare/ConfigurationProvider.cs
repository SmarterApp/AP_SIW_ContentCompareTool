using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

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
            logger.LogInformation("Successfully downloaded settings");

            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.Override.json", optional: true);
            Configuration = builder.Build();
            AppSettings appSettings = new AppSettings();

            Configuration.Bind(appSettings);
            AppSettings = appSettings;
            
        }

        public void DownloadConfigFiles()
        {
            string itemsPatchUrl = "https://raw.githubusercontent.com/SmarterApp/AP_ItemSampler/master/SmarterBalanced.SampleItems/ClaimConfigurations/ItemsPatch.xml";
            string accessibilityUrl = "https://raw.githubusercontent.com/osu-cass/AccessibilityAccommodationConfigurations/05bf8f52863bce54142c9f3bc36db02e475258f4/AccessibilityConfig.xml";
            string claimsUrl = "https://raw.githubusercontent.com/SmarterApp/AP_ItemSampler/scoreguide/SmarterBalanced.SampleItems/ClaimConfigurations/ClaimsConfig.xml";
            string interactionsUrl = "https://raw.githubusercontent.com/SmarterApp/AP_ItemSampler/scoreguide/SmarterBalanced.SampleItems/InteractionTypeConfigurations/InteractionTypes.xml";
            string standardsUrl = "https://raw.githubusercontent.com/SmarterApp/AP_ItemSampler/scoreguide/SmarterBalanced.SampleItems/CoreStandardsConfigurations/CoreStandards.xml";

            Task.WaitAll(
                SaveDependencyAsync(itemsPatchUrl, AppSettings.SbContent.PatchXMLPath),
                SaveDependencyAsync(accessibilityUrl, AppSettings.SbContent.AccommodationsXMLPath),
                SaveDependencyAsync(claimsUrl, AppSettings.SbContent.ClaimsXMLPath),
                SaveDependencyAsync(interactionsUrl, AppSettings.SbContent.InteractionTypesXMLPath),
                SaveDependencyAsync(standardsUrl, AppSettings.SbContent.CoreStandardsXMLPath));
            logger.LogInformation("Successfully loaded configuration");
        }

        private async Task SaveDependencyAsync(string url, string fileName)
        {
            await Task.Run(() => SaveDependency(url, fileName));
        }

        private void SaveDependency(string url, string fileName)
        {
            using (var client = new HttpClient())
            {
                using (var result = client.GetAsync(url).Result)
                {
                    if (result.IsSuccessStatusCode)
                    {
                        string contents = result.Content.ReadAsStringAsync().Result;
                        string path = Path.Combine(Directory.GetCurrentDirectory(), fileName);
                        Directory.CreateDirectory(Path.GetDirectoryName(path));
                        if (File.Exists(path))
                        {
                            logger.LogInformation($"File exists, removing {fileName}");
                            File.Delete(path);
                        }

                        File.WriteAllText(path, contents);
                    }
                }
            }
        }
    }
}
