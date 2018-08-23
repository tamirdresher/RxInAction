using System;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows;

namespace ObserveAndSubscribeOn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            Observable.FromEventPattern(this.TextBox, "TextChanged")
                .Select(_ => this.TextBox.Text)
                .Throttle(TimeSpan.FromMilliseconds(400))
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(s => this.ThrottledResults.Items.Add(s));
        }
    }
}
