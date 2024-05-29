using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace MauiMvvmSample.ViewModels;

public sealed partial class ProfileViewModel : ObservableObject
{
    [ObservableProperty]
    private Guid id = Guid.Empty;

    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private DateTime birthday = DateTime.MinValue;
}
