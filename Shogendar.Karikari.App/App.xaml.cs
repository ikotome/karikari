﻿namespace Shogendar.Karikari.App;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new MainPage()) { Title = "Shogendar.Karikari.App" };
	}
}
