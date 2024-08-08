namespace MauiMvvmSample.Options;

public sealed class ServerSettingsOption
{
    public const string SectionName = "Config:ServerSettings";

    public string Env { get; set; } = string.Empty;

    public string Host { get; set; } = string.Empty;
}
