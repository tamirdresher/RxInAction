using System;
using System.Reactive.Linq;
using System.Windows;

namespace CreatingPeriodicUpdatableView {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private IDisposable _subscription;

        public MainWindow() {
            this.InitializeComponent();

            var updatesWebService = new UpdatesWebService();
            this._subscription = Observable
            .Interval(TimeSpan.FromSeconds(3))
            .Take(3) // we dont want to run the example forever, so we'll do only 3 updates
            .SelectMany(_ => updatesWebService.GetUpdatesAsync())
            .SelectMany(updates => updates)
            .ObserveOnDispatcher()
            .Subscribe(message => this.messages.Items.Add(message));
        }
    }
}
