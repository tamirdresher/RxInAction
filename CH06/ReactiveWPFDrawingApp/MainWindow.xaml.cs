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
        public MainWindow()
        {
            InitializeComponent();



            var mouseDowns = Observable.FromEventPattern<MouseButtonEventArgs>(this, "MouseDown");
            var mouseUp = Observable.FromEventPattern<MouseButtonEventArgs>(this, "MouseUp");
            var movements = Observable.FromEventPattern<MouseEventArgs>(this, "MouseMove");




            Polyline line = new Polyline() { Stroke = Brushes.Black, StrokeThickness = 3 };
            canvas.Children.Add(line);

movements
    .Select(m => m.EventArgs.GetPosition(this))
    .Repeat()
    .Subscribe(pos => line.Points.Add(pos));



            //movements
            //   .SkipUntil(mouseDowns.Do(_ =>
            //   {
            //       line = new Polyline() { Stroke = Brushes.Black, StrokeThickness = 3 };
            //       canvas.Children.Add(line);
            //   }))
               
            //   .TakeUntil(mouseUp)
            //   .Select(m => m.EventArgs.GetPosition(this))
            //   .Repeat()
            //   .Subscribe(pos => line.Points.Add(pos));

            //mouseDowns
            //    //.Do(_ =>
            //    //{
            //    //    line = new Polyline() { Stroke = Brushes.Black, StrokeThickness = 3 };
            //    //    canvas.Children.Add(line);
            //    //})
            //    .SelectMany(_ => movements.TakeUntil(mouseUp))
            //    .Select(m => m.EventArgs.GetPosition(this))
            //    .Subscribe(pos => line.Points.Add(pos));
            //.Subscribe(pos => canvas.Children.Add(new Line {X1=pos.X,X2= pos.X+1, Y1=pos.Y, Y2 = pos.Y+1, Stroke = Brushes.Black,StrokeThickness = 1}));


        }
    }
}
