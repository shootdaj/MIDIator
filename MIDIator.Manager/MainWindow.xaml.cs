using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
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
using Refigure;

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
            Loaded += (sender, args) =>
            {
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

        private void Start()
        {
            Manager.Start((ex) => txtOutput.Text = txtOutput.Text + ex.Message + Environment.NewLine, this.ExitApplication);
            btnStartStop.Content = "Stop";
            lblStatus.Content = "Running";

            //launch browser with client running
            Process.Start(Config.Get("WebClient.BaseAddress"));
        }

        private void Stop()
        {
            Manager.Stop();
            btnStartStop.Content = "Start";
            lblStatus.Content = "Stopped";
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (Manager.Running)
                Stop();
            Manager.Dispose();
            base.OnClosing(e);
        }
    }
}
