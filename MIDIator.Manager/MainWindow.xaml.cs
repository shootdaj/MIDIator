using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Refigure;
using XamlAnimatedGif;
using Image = System.Windows.Controls.Image;

namespace MIDIator.Manager
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Manager Manager { get; }

		public bool Running => Manager.Running;

		private bool LaunchWebUIOnStart { get; set; }
		private bool AutostartOnAppStart { get; set; }

		public MainWindow()
		{
			InitializeComponent();
			Manager = new Manager();
			chkLaunchWebUI.IsChecked = Config.GetAsBoolSilent("Manager.LaunchWebUI") != null
				? Config.GetAsBoolSilent("Manager.LaunchWebUI")
				: false;

			chkAutostartOnAppStart.IsChecked = Config.GetAsBoolSilent("Manager.AutostartOnAppStart") != null
				? Config.GetAsBoolSilent("Manager.AutostartOnAppStart")
				: false;

			Loaded += (sender, args) =>
			{
				if (AutostartOnAppStart)
					Start();
			};
		}

		public void ExitApplication()
		{
			Dispatcher.Invoke(() =>
			{
				Stop();
				Close();
			});
		}

		protected override void OnStateChanged(EventArgs e)
		{
			if (WindowState == WindowState.Minimized)
				Hide();

			base.OnStateChanged(e);
		}

		private void btnStartStop_Click(object sender, RoutedEventArgs e)
		{
			if (!Manager.Running)
			{
				Start();
			}
			else
			{
				Stop();
			}
		}

		public void Log(string text)
		{
		    if (!string.IsNullOrEmpty(text))
		    {
		        txtOutput.Text += text + Environment.NewLine;
		        txtOutput.ScrollToEnd();
		    }
		}

		private void Start()
		{
			var image = ((Image)((StackPanel)btnStartStop.Content).Children[0]);
			AnimationBehavior.SetSourceUri(image, new Uri("loading.gif", UriKind.Relative));
			Log("Starting...");

			var task = new Task(() => Manager.Start(ExitApplication));

			task.ContinueWith(prevTask =>
			{
				if (prevTask.IsFaulted || prevTask.IsCanceled)
				{
				    Log(prevTask.Exception?.ToString());
					SetStartButton(image);
				}
				else
				{
					SetStopButton(image);

					//launch browser with client running
					if (LaunchWebUIOnStart)
						LaunchWebUI();

					Log("Started successfully.");
				}
			}, TaskScheduler.FromCurrentSynchronizationContext());

			task.Start();
		}

		private void SetStopButton(Image image)
		{
			btnStartStopTooltip.Content = "Click to Stop. Right-click to launch Web UI.";
			btnStartStop.Style = Resources["RoundButtonTemplateGreen"] as Style;
			AnimationBehavior.SetSourceUri(image, new Uri("stop-circle.png", UriKind.Relative));
		}

		private void SetStartButton(Image image)
		{
			btnStartStopTooltip.Content = "Click to Start";
			btnStartStop.Style = Resources["RoundButtonTemplateOrange"] as Style;
			AnimationBehavior.SetSourceUri(image, new Uri("play-circle.png", UriKind.Relative));
		}

		public static void LaunchWebUI()
		{
			Process.Start(Config.Get("WebClient.BaseAddress"));
		}

		private void Stop()
		{
			var image = ((Image)((StackPanel)btnStartStop.Content).Children[0]);
			AnimationBehavior.SetSourceUri(image, new Uri("loading.gif", UriKind.Relative));
			Log("Stopping...");

			bool success = false;
			string errorMessage = string.Empty;

			var thread = new Thread(() =>
			{
				try
				{
					Manager.Stop();
					success = true;

				}
				catch (Exception ex)
				{
					errorMessage = ex.ToString();
					success = false;
				}
			});
			thread.Start();
			thread.Join();

			if (success)
			{
				SetStartButton(image);
				Log("Stopped successfully.");
			}
			else
			{
				SetStopButton(image);
				Log(errorMessage);
			}
		}

		protected override void OnClosing(CancelEventArgs e)
		{
			if (Manager.Running)
				Stop();
			Manager.Dispose();
			base.OnClosing(e);
		}

		private void chkLaunchWebUI_Checked(object sender, RoutedEventArgs e)
		{
			// ReSharper disable once PossibleInvalidOperationException
			LaunchWebUIOnStart = chkLaunchWebUI.IsChecked.Value;
			Config.Set("Manager.LaunchWebUI", chkLaunchWebUI.IsChecked.Value ? "true" : "false");
		}

		private void chkAutostartOnAppStart_Checked(object sender, RoutedEventArgs e)
		{
			// ReSharper disable once PossibleInvalidOperationException
			AutostartOnAppStart = chkAutostartOnAppStart.IsChecked.Value;
			Config.Set("Manager.AutostartOnAppStart", chkAutostartOnAppStart.IsChecked.Value ? "true" : "false");
		}

		private void BtnStartStop_OnMouseRightButtonUp(object sender, MouseButtonEventArgs e)
		{
			if (Manager.Running)
				LaunchWebUI();
		}
	}
}
