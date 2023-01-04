using System;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ReactiveWPFDrawingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            IObservable<EventPattern<MouseButtonEventArgs>> mouseDowns =
                Observable.FromEventPattern<MouseButtonEventArgs>(this, "MouseDown");

            IObservable<EventPattern<MouseButtonEventArgs>> mouseUp =
                Observable.FromEventPattern<MouseButtonEventArgs>(this, "MouseUp");

            IObservable<EventPattern<MouseEventArgs>> movements =
                Observable.FromEventPattern<MouseEventArgs>(this, "MouseMove");

            Polyline line = null;

            this._subscription =
                movements
                .SkipUntil(
                    mouseDowns.Do(_ => {
                        line = new Polyline() { Stroke = Brushes.Black, StrokeThickness = 3 };
                        this.canvas.Children.Add(line);
                    }))
                .TakeUntil(mouseUp)
                .Select(m => m.EventArgs.GetPosition(this))
                .Repeat()
                .Subscribe(pos => line.Points.Add(pos));
        }

        private IDisposable _subscription;
    }
}
