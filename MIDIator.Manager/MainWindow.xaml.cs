﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Refigure;
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

        public MainWindow()
        {
            InitializeComponent();
            Manager = new Manager();
            btnStartStop.Background = new SolidColorBrush(Color.FromArgb(255, 118, 255, 3));
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

        public void Log(string text)
        {
            txtOutput.Text += text + Environment.NewLine;
        }

        private void Start()
        {
            Log("Starting...");
            Manager.Start((ex) => txtOutput.Text = txtOutput.Text + ex.Message + Environment.NewLine, this.ExitApplication);
            btnStartStop.Background = new SolidColorBrush(Color.FromArgb(255, 118, 255, 3));
            ((Image) ((StackPanel) btnStartStop.Content).Children[0]).Source =
                new BitmapImage(new Uri("stop-circle-outline.png", UriKind.Relative));

            //launch browser with client running
            Process.Start(Config.Get("WebClient.BaseAddress"));

            Log("Started successfully.");
        }

        private void Stop()
        {
            Log("Stopping...");

            Manager.Stop();
            btnStartStop.Background = new SolidColorBrush(Color.FromArgb(255, 255, 87, 34));
            ((Image)((StackPanel)btnStartStop.Content).Children[0]).Source =
                new BitmapImage(new Uri("play-circle.png", UriKind.Relative));

            Log("Stopped successfully.");
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
