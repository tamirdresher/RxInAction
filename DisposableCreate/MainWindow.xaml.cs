using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DisposableCreate.Annotations;

namespace DisposableCreate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        private IEnumerable<string> _newsItems;
        private bool _isBusy;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        public bool IsBusy
        {
            get { return _isBusy; }
            set
            {
                if (value == _isBusy) return;
                _isBusy = value;
                OnPropertyChanged();
            }
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            RefreshNewsAsync();
        }

        public IEnumerable<string> NewsItems
        {
            get { return _newsItems; }
            set
            {
                if (Equals(value, _newsItems)) return;
                _newsItems = value;
                OnPropertyChanged();
            }
        }

        private async void RefreshNewsAsync()
        {
            IsBusy = true;
            NewsItems = Enumerable.Empty<string>();
            using (Disposable.Create(() => IsBusy = false))
            {
                NewsItems = await DownloadNewsItems();
            }
        }

        private async void RefreshNews2Async()
        {
            NewsItems = Enumerable.Empty<string>();
            using (StartBusy())
            {
                NewsItems = await DownloadNewsItems();
            }
        }

        public IDisposable StartBusy()
        {
            IsBusy = true;
            return new ContextDisposable(
                SynchronizationContext.Current,
                Disposable.Create(() => IsBusy = false));
        }


        private async Task<IEnumerable<string>>  DownloadNewsItems()
        {
            await Task.Delay(2000);
            return Enumerable.Range(1, 10).Select(i => "News Items " + i);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
