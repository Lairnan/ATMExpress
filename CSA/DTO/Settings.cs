using System.Text;
using Microsoft.Extensions.Configuration;

namespace CSA.DTO;

public static class Settings
{
    public static string BaseUrl { get; private set; }
    public static string LogPath { get; private set; }

    #region Init.
    static Settings()
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory() + "/config")
            .AddJsonFile("mainsettings.json", true, true)
            .Build();

        BaseUrl = configuration["AppSettings:baseurl"]!;
        LogPath = configuration["AppSettings:logPath"]!;
    }
    #endregion
}