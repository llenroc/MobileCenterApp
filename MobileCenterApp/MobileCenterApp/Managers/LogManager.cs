﻿using System;
using System.Collections.Generic;
using Microsoft.Azure.Mobile.Analytics;
namespace MobileCenterApp
{
	public class LogManager
	{
		public static LogManager Shared { get; set; } = new LogManager();

		public void LogEvent(string evt, Dictionary<string, string> data = null)
		{
			Analytics.TrackEvent(evt, data);
		}

		public void UserLoggedIn(string username)
		{
			LogEvent("Login", new Dictionary<string, string> { { "Username", username } });
		}

		public void PageView(string title,Dictionary<string,string> data = null)
		{
			if (data == null)
				data = new Dictionary<string, string> ();
			data["Page Title"] = title;
			LogEvent("Page View", data);
		}

	}
}
