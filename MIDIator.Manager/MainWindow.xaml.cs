using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MIDIator.Manager
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private Manager Manager { get; set; }

		public MainWindow()
		{
			InitializeComponent();
			Manager = new Manager();
		}

		protected override void OnStateChanged(EventArgs e)
		{
			if (WindowState == System.Windows.WindowState.Minimized)
				this.Hide();

			base.OnStateChanged(e);
		}

		private void btnStartStop_Click(object sender, RoutedEventArgs e)
		{
			if (!Manager.Running)
			{
			    Manager.Start((ex) => txtOutput.Text = txtOutput.Text + ex.Message + Environment.NewLine);
				btnStartStop.Content = "Stop";
				lblStatus.Content = "Running";
			}
			else
			{
				Manager.Stop();
				btnStartStop.Content = "Start";
				lblStatus.Content = "Stopped";
			}
		}

		private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
		{
			Manager.Dispose();
		}
	}
}
