using MauiMvvmSample.ViewModels;

namespace MauiMvvmSample.Views;

public sealed partial class MainPage
{
	public MainPage(MainPageViewModel viewModel)
	{
        this.BindingContext = viewModel;
		this.InitializeComponent();
	}
}

