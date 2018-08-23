using System;
using System.Linq;
using System.Reactive.Linq;
using System.Windows;

namespace SearchScreenWithDistinctUntilChanged
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            Observable.FromEventPattern(this.SearchTerm, "TextChanged")
                .Select(_ => this.SearchTerm.Text)
                .Throttle(TimeSpan.FromMilliseconds(400))
                .DistinctUntilChanged()
                .ObserveOnDispatcher()
                .Subscribe(s => this.Terms.Items.Add(s));
        }
    }
}
