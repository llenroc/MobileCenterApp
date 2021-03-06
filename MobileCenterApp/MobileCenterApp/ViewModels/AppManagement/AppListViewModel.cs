﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MobileCenterApp
{
	public class AppListViewModel : BaseViewModel
	{
		public AppListViewModel()
		{
			Title = "Apps";
		}

		SimpleDatabaseSource<AppClass> items;
		public SimpleDatabaseSource<AppClass> Items { 
			get { return items; }
			set { ProcPropertyChanged(ref items, value); }
		}

		public ICommand CreateCommand { get; private set; } = new Command(async (obj) =>
		{
			try
			{
				await NavigationService.PushModalAsync(new CreateAppViewModel());
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		});

		public ICommand DeleteCommand { get; private set; } = new Command(async (obj) =>
		{
			try
			{
				var app = ((MenuItem)obj).CommandParameter as AppClass;
				var success = await SyncManager.Shared.DeleteApp(app);
				if(!success)
					App.Current.MainPage.DisplayActionSheet("Error deleting the app. Please try again", "Ok", null);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex);
			}
		});

		void Shared_AppsChanged(object sender, EventArgs e)
		{
			Items = new SimpleDatabaseSource<AppClass> { Database = Database.Main };
		}

		public override async Task Refresh()
		{
			Items = new SimpleDatabaseSource<AppClass> { Database = Database.Main };
			await SyncManager.Shared.SyncApps();
		}

		public override async void OnAppearing()
		{
			base.OnAppearing();
			NotificationManager.Shared.AppsChanged += Shared_AppsChanged;

		}

		public override void OnDisappearing()
		{
			base.OnDisappearing();
			NotificationManager.Shared.AppsChanged -= Shared_AppsChanged;
		}
	}
}
