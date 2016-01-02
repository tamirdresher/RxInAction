using System;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading;
using System.Windows;
using Helpers;

namespace ObserveAndSubscribeOn
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Observable.FromEventPattern(TextBox, "TextChanged")
                .Select(_ => TextBox.Text)
                .Throttle(TimeSpan.FromMilliseconds(400))
                .ObserveOn(DispatcherScheduler.Current)
                .Subscribe(s => ThrottledResults.Items.Add(s));
        }

            
    }
}
