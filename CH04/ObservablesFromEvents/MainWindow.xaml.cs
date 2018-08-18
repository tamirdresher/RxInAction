using Helpers;
using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;

namespace ObservablesFromEvents {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            this.InitializeComponent();
            IObservable<EventPattern<RoutedEventArgs>> clicks =
                            Observable.FromEventPattern<RoutedEventHandler, RoutedEventArgs>(
                                h => this.theButton.Click += h,
                                h => this.theButton.Click -= h);

            //IObservable<EventPattern<object>> clicks = Observable.FromEventPattern(theButton, "Click");

            // the message will be written to VS output window
            clicks.SubscribeConsole();

            // the message will be written in the TextBox
            clicks.Subscribe(eventPattern => this.output.Text += "button clicked" + Environment.NewLine);
        }
    }
}
