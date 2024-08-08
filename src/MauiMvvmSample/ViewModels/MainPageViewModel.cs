using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using MauiMvvmSample.Options;
using MauiMvvmSample.Services;
using Microsoft.Extensions.Configuration;

namespace MauiMvvmSample.ViewModels;

public sealed partial class MainPageViewModel : ObservableObject
{
    private readonly BackgroundService backgroundService;

    private readonly ServerSettingsOption serverSettingsOption;

    [ObservableProperty]
    private string title = string.Empty;

    [ObservableProperty]
    private string host = string.Empty;

    [ObservableProperty]
    private TimeSpan since = TimeSpan.Zero;

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(this.UpperCaseText))]
    private string inputText = string.Empty;

    [ObservableProperty]
    private string profileListTitle = string.Empty;

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(this.AddProfileCommand))]
    private string profileName = string.Empty;

    [ObservableProperty]
    private DateTime profileBirthday = DateTime.Today;

    public MainPageViewModel(BackgroundService backgroundService, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration, nameof(configuration));
        this.backgroundService = backgroundService;
        this.serverSettingsOption = new ServerSettingsOption();
        configuration.GetSection(ServerSettingsOption.SectionName).Bind(this.serverSettingsOption);
        this.Title = $"{nameof(MauiMvvmSample)} ({this.serverSettingsOption.Env})";
        this.Host = this.serverSettingsOption.Host;
        WeakReferenceMessenger.Default.Register<ValueChangedMessage<TimeSpan>>(this, (_, message) =>
        {
            this.Since = message.Value;
        });
        this.backgroundService.Start(TimeSpan.FromSeconds(1));
    }

    public string UpperCaseText => this.InputText.ToUpperInvariant();

    public ObservableCollection<ProfileViewModel> Profiles { get; } = [];

    private bool CanAddProfile => !string.IsNullOrEmpty(this.ProfileName);

    [RelayCommand(CanExecute = nameof(this.CanAddProfile))]
    private void AddProfile()
    {
        if (string.IsNullOrEmpty(this.ProfileName))
        {
            return;
        }

        this.Profiles.Add(new ProfileViewModel
        {
            Id = Guid.NewGuid(),
            Name = this.ProfileName,
            Birthday = this.ProfileBirthday,
        });
        this.ProfileName = string.Empty;
    }
}
