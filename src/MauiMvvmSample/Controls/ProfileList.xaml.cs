using System.Collections;
using System.Collections.Generic;
using MauiMvvmSample.ViewModels;
using Microsoft.Maui.Controls;

namespace MauiMvvmSample.Controls;

public sealed partial class ProfileList
{
    public static readonly BindableProperty TitleProperty = BindableProperty.Create(
        nameof(Title),
        typeof(string),
        typeof(ProfileList),
        string.Empty,
        propertyChanged: OnTitleChanged);

    public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create(
        nameof(ItemsSource),
        typeof(IEnumerable<ProfileViewModel>),
        typeof(ProfileList),
        propertyChanged: OnItemsSourceChanged);

    public ProfileList()
    {
        this.InitializeComponent();
    }

    public string Title
    {
        get => (string)this.GetValue(TitleProperty);
        set => this.SetValue(TitleProperty, value);
    }

    public IEnumerable<ProfileViewModel> ItemsSource
    {
        get => (IEnumerable<ProfileViewModel>)this.GetValue(ItemsSourceProperty);
        set => this.SetValue(ItemsSourceProperty, value);
    }

    private static void OnTitleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not ProfileList profileList)
        {
            return;
        }

        if (profileList.TitleLabel is null)
        {
            return;
        }

        profileList.TitleLabel.Text = newValue.ToString() ?? string.Empty;
    }

    private static void OnItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (bindable is not ProfileList profileList)
        {
            return;
        }

        if (newValue is not IEnumerable items)
        {
            return;
        }

        profileList.ProfileCollectionView.ItemsSource = items;
    }
}

