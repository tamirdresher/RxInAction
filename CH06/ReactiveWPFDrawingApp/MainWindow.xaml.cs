using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;
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

namespace ReactiveWPFDrawingApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private IDisposable _subscription;

        public MainWindow()
        {
            InitializeComponent();



            var mouseDowns = Observable.FromEventPattern<MouseButtonEventArgs>(this, "MouseDown");
            var mouseUp = Observable.FromEventPattern<MouseButtonEventArgs>(this, "MouseUp");
            var movements = Observable.FromEventPattern<MouseEventArgs>(this, "MouseMove");




            Polyline line = null;
           _subscription = 
                movements
                .SkipUntil(
                    mouseDowns.Do(_ =>
                    {
                        line = new Polyline() { Stroke = Brushes.Black, StrokeThickness = 3 };
                        canvas.Children.Add(line);
                    }))
                .TakeUntil(mouseUp)
                .Select(m => m.EventArgs.GetPosition(this))
                .Repeat()
                .Subscribe(pos => line.Points.Add(pos));




        }
    }
}
