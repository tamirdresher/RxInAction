using System;
using System.Collections.Generic;
using System.Linq;
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

namespace SearchScreenWithDistinctUntilChanged
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

Observable.FromEventPattern(SearchTerm, "TextChanged")
    .Select(_ => SearchTerm.Text)
    .Throttle(TimeSpan.FromMilliseconds(400))
    .DistinctUntilChanged()
    .ObserveOnDispatcher()
    .Subscribe(s => Terms.Items.Add(s));
        }
    }
}
