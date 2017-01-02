using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Refigure;
using XamlAnimatedGif;
using Color = System.Windows.Media.Color;
using Image = System.Windows.Controls.Image;

namespace MIDIator.Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Manager Manager { get; set; }

        private bool LaunchWebUI { get; set; }
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
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();

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
            txtOutput.Text += text + Environment.NewLine;
            txtOutput.ScrollToEnd();
        }

        private void Start()
        {
            var image = ((Image) ((StackPanel) btnStartStop.Content).Children[0]);
            AnimationBehavior.SetSourceUri(image, new Uri("loading.gif", UriKind.Relative));
            Log("Starting...");

            Task.Run(() =>
            {
                Manager.Start((ex) => txtOutput.Text = txtOutput.Text + ex.Message + Environment.NewLine,
                    this.ExitApplication);
            }).ContinueWith((prevTask) =>
            {
                Dispatcher.Invoke(() =>
                {
                    btnStartStopTooltip.Content = "Click to Stop";
                    btnStartStop.Style = this.Resources["RoundButtonTemplateGreen"] as Style;
                    AnimationBehavior.SetSourceUri(image, new Uri("stop-circle.png", UriKind.Relative));
                    
                    //launch browser with client running
                    if (LaunchWebUI)
                        Process.Start(Config.Get("WebClient.BaseAddress"));

                    Log("Started successfully.");
                });
            });
        }

        private void Stop()
        {
            var image = ((Image)((StackPanel)btnStartStop.Content).Children[0]);
            AnimationBehavior.SetSourceUri(image, new Uri("loading.gif", UriKind.Relative));
            Log("Stopping...");

            Task.Run(() => {
                Manager.Stop();
            }).ContinueWith((prevTask) =>
            {
                Dispatcher.Invoke(() =>
                {
                    btnStartStopTooltip.Content = "Click to Start";
                    btnStartStop.Style = this.Resources["RoundButtonTemplateOrange"] as Style;
                    AnimationBehavior.SetSourceUri(image, new Uri("play-circle.png", UriKind.Relative));
                    Log("Stopped successfully.");
                });
            });
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
            LaunchWebUI = chkLaunchWebUI.IsChecked.Value;
            Config.Set("Manager.LaunchWebUI", chkLaunchWebUI.IsChecked.Value ? "true" : "false");
        }

        private void chkAutostartOnAppStart_Checked(object sender, RoutedEventArgs e)
        {
            // ReSharper disable once PossibleInvalidOperationException
            AutostartOnAppStart = chkAutostartOnAppStart.IsChecked.Value;
            Config.Set("Manager.AutostartOnAppStart", chkAutostartOnAppStart.IsChecked.Value ? "true" : "false");
        }
    }
}
