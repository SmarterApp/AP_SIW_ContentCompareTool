using Microsoft.Extensions.Configuration;
using SmarterBalanced.SampleItems.Dal.Configurations.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace SGContent
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }
        public AppSettings AppSettings { get;}

        private static Startup instance;
        private static readonly object _lock = new object();


        public Startup()
        {

            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.Override.json", optional: true);
            Configuration = builder.Build();
            AppSettings appSettings = new AppSettings();

            Configuration.Bind(appSettings);
            AppSettings = appSettings;
        }

        public static Startup Instance
        {
            get
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new Startup();
                    }
                    return instance;
                }

            }
        }


    }
}
